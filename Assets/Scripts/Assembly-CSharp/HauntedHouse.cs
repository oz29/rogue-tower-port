using UnityEngine;

public class HauntedHouse : IncomeGenerator, IBuildable
{
	[SerializeField]
	private LayerMask graveLayerMask;

	[SerializeField]
	private GameObject UIObject;

	[SerializeField]
	private int goldBackOnDemolish;

	private bool isGathering;

	private int graveCount;

	private int manaUsed;

	private float timer;

	private SimpleUI myUI;

	protected override void Start()
	{
		base.Start();
		DetectGraves();
	}

	private void Update()
	{
		if (myUI != null && isGathering)
		{
			string text = "Mana used: " + manaUsed;
			text = text + "\nNearby graves: x" + graveCount;
			text = text + "\nTax efficiency: x" + GameManager.instance.hauntedHouseEfficiency;
			text = text + "\nDeath tax due: " + Mathf.Max((int)Mathf.Sqrt(manaUsed * GameManager.instance.hauntedHouseEfficiency * graveCount), 1) + "g";
			text = text + "\nNet tax collected: " + base.netGold + "g.";
			myUI.SetDiscriptionText(text);
		}
		if (!isGathering || !SpawnManager.instance.combat)
		{
			return;
		}
		if (timer <= 0f)
		{
			if (ResourceManager.instance.CheckMana(1))
			{
				ResourceManager.instance.SpendMana(1);
				manaUsed++;
				timer = 1f;
			}
		}
		else
		{
			timer -= Time.deltaTime;
		}
	}

	public override void GenerateIncome()
	{
		incomePerRound = Mathf.Max((int)Mathf.Sqrt(manaUsed * GameManager.instance.hauntedHouseEfficiency * graveCount), 1);
		base.GenerateIncome();
		manaUsed = 0;
		incomePerRound = 1;
	}

	public void SetStats()
	{
	}

	private void DetectGraves()
	{
		if (Physics.Raycast(base.transform.position + new Vector3(1f, 1f, 0f), -base.transform.up, out var hitInfo, 1f, graveLayerMask, QueryTriggerInteraction.Ignore))
		{
			CheckGrave(hitInfo);
		}
		if (Physics.Raycast(base.transform.position + new Vector3(-1f, 1f, 0f), -base.transform.up, out hitInfo, 1f, graveLayerMask, QueryTriggerInteraction.Ignore))
		{
			CheckGrave(hitInfo);
		}
		if (Physics.Raycast(base.transform.position + new Vector3(0f, 1f, 1f), -base.transform.up, out hitInfo, 1f, graveLayerMask, QueryTriggerInteraction.Ignore))
		{
			CheckGrave(hitInfo);
		}
		if (Physics.Raycast(base.transform.position + new Vector3(0f, 1f, -1f), -base.transform.up, out hitInfo, 1f, graveLayerMask, QueryTriggerInteraction.Ignore))
		{
			CheckGrave(hitInfo);
		}
	}

	private void CheckGrave(RaycastHit hit)
	{
		if (hit.collider.GetComponent<Grave>() != null && (double)Mathf.Abs(hit.collider.transform.position.y - base.transform.position.y) <= 0.001)
		{
			graveCount++;
			isGathering = true;
		}
	}

	public void SpawnUI()
	{
		myUI = Object.Instantiate(UIObject, base.transform.position, Quaternion.identity).GetComponent<SimpleUI>();
		myUI.SetDemolishable(base.gameObject, goldBackOnDemolish);
	}

	public void Demolish()
	{
		RemoveIncomeGeneration();
	}
}
