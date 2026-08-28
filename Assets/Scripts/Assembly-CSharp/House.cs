using System.Collections.Generic;
using UnityEngine;

public class House : SpawnableObject
{
	private List<Tower> defenders = new List<Tower>();

	[SerializeField]
	private IncomeGenerator myIncomeGenerator;

	protected override void Start()
	{
		base.Start();
		SpawnManager.instance.houses.Add(this);
	}

	public void AddDefender(Tower t)
	{
		if (!defenders.Contains(t))
		{
			defenders.Add(t);
		}
		CheckTowers();
	}

	public void CheckTowers()
	{
		for (int num = defenders.Count - 1; num > -1; num--)
		{
			if (defenders[num] == null)
			{
				defenders.RemoveAt(num);
			}
		}
		myIncomeGenerator.incomeTimesLevel = defenders.Count;
	}

	public override void SpawnUI()
	{
		SimpleUI component = Object.Instantiate(UIObject, base.transform.position, Quaternion.identity).GetComponent<SimpleUI>();
		if (defenders.Count > 0)
		{
			string text = "This house is protected by " + defenders.Count + " towers.";
			text = text + "\nIts next gift will be " + (SpawnManager.instance.level + 1) * defenders.Count + "g.";
			text = text + "\nNet gold gifted: " + myIncomeGenerator.netGold + "g.";
			component.SetDiscriptionText(text);
		}
	}
}
