using UnityEngine;

public class IncomeGenerator : MonoBehaviour
{
	public int incomePerRound;

	public float incomeTimesLevel;

	[SerializeField]
	private DamageTracker.IncomeType myIncomeType;

	public int netGold { get; private set; }

	protected virtual void Start()
	{
		SpawnManager.instance.incomeGenerators.Add(this);
		netGold = 0;
	}

	public virtual void GenerateIncome()
	{
		int num = incomePerRound + (int)(incomeTimesLevel * (float)SpawnManager.instance.level);
		if (num > 0)
		{
			ResourceManager.instance.AddMoney(num);
			netGold += num;
			DamageTracker.instance.AddIncome(myIncomeType, num);
			SFXManager.instance.PlaySound(Sound.CoinShort, base.transform.position);
			DamageNumber component = ObjectPool.instance.SpawnObject(ObjectPool.ObjectType.DamageNumber, base.transform.position, Quaternion.identity).GetComponent<DamageNumber>();
			component.SetText("+" + num + "g", "Grey", 1f);
			component.SetHoldTime(2.5f);
		}
	}

	public void RemoveIncomeGeneration()
	{
		SpawnManager.instance.incomeGenerators.Remove(this);
	}
}
