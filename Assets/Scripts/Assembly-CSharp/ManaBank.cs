using UnityEngine;

public class ManaBank : MonoBehaviour, IBuildable
{
	[SerializeField]
	private int gatherRatePerSec;

	[SerializeField]
	private int maxManaIncrease;

	[SerializeField]
	private GameObject UIObject;

	[SerializeField]
	private int goldBackOnDemolish;

	private float timer = 1f;

	private void Start()
	{
		maxManaIncrease += ResourceManager.instance.manaBankBonusMana;
		ResourceManager.instance.UpdateManaGatherRate(gatherRatePerSec);
		ResourceManager.instance.AddMaxMana(maxManaIncrease);
		ResourceManager.instance.manaBanks.Add(this);
	}

	private void Update()
	{
		if (timer <= 0f)
		{
			Gather();
			timer = 1f;
		}
		else if (SpawnManager.instance.combat)
		{
			timer -= Time.deltaTime;
		}
	}

	public void SetStats()
	{
	}

	public void UpgradeMaxMana(int addition)
	{
		ResourceManager.instance.AddMaxMana(addition);
		maxManaIncrease += addition;
	}

	public void SpawnUI()
	{
		SimpleUI component = Object.Instantiate(UIObject, base.transform.position, Quaternion.identity).GetComponent<SimpleUI>();
		component.SetDemolishable(base.gameObject, goldBackOnDemolish);
		component.SetDiscriptionText("This bank currently stores " + maxManaIncrease + " mana. It is generating " + ((float)Mathf.FloorToInt(10f * ResourceManager.instance.manaMaxRegenPercent * (float)maxManaIncrease) / 10f + 1f) + " mana/s through sorcery and clever investing.");
	}

	public void Demolish()
	{
		ResourceManager.instance.UpdateManaGatherRate(-gatherRatePerSec);
		ResourceManager.instance.AddMaxMana(-maxManaIncrease);
		ResourceManager.instance.manaBanks.Remove(this);
	}

	private void Gather()
	{
		ResourceManager.instance.AddMana(gatherRatePerSec);
	}
}
