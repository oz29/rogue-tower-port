using System.Collections.Generic;
using UnityEngine;

public class TowerFlyweight : MonoBehaviour
{
	private HashSet<Tower> towers = new HashSet<Tower>();

	[SerializeField]
	private BuildButtonUI myBuildButton;

	[SerializeField]
	private int basePrice;

	[SerializeField]
	public int priceIncrease;

	[SerializeField]
	public int currentPrice;

	[SerializeField]
	public int sellPrice;

	[SerializeField]
	public float bonusRange;

	[SerializeField]
	public int bonusBaseDamage;

	[SerializeField]
	public int bonusHealthDamage;

	[SerializeField]
	public int bonusArmorDamage;

	[SerializeField]
	public int bonusShieldDamage;

	[SerializeField]
	public float bonusSlow;

	[SerializeField]
	public float bonusBleed;

	[SerializeField]
	public float bonusBurn;

	[SerializeField]
	public float bonusPoison;

	[SerializeField]
	public float bonusBlast;

	[SerializeField]
	public float critChance;

	[SerializeField]
	public float critChanceLevelMultiplier;

	[SerializeField]
	public float stunChance;

	[SerializeField]
	public float manaConsumptionAddition;

	[SerializeField]
	private TowerType towerTypeForDamageTracker;

	public void AddNewTower(Tower newTower)
	{
		towers.Add(newTower);
		currentPrice = basePrice + towers.Count * priceIncrease;
		sellPrice = basePrice + (towers.Count - 1) * priceIncrease;
		myBuildButton.UpdatePriceText();
	}

	public void RemoveTower(Tower newTower)
	{
		towers.Remove(newTower);
		currentPrice = basePrice + towers.Count * priceIncrease;
		sellPrice = basePrice + (towers.Count - 1) * priceIncrease;
		myBuildButton.UpdatePriceText();
		ResourceManager.instance.AddMoney(basePrice + towers.Count * priceIncrease);
		DamageTracker.instance.AddCost(towerTypeForDamageTracker, -(basePrice + towers.Count * priceIncrease));
	}

	public void UpdateTowerStats()
	{
		foreach (Tower tower in towers)
		{
			tower.SetStats();
		}
		if (myBuildButton != null)
		{
			myBuildButton.UpdateModifiersText();
		}
	}

	public void AddManaAddition(float bonus)
	{
		manaConsumptionAddition += bonus;
		foreach (Tower tower in towers)
		{
			tower.SetStats();
		}
		if (myBuildButton != null)
		{
			myBuildButton.UpdateModifiersText();
		}
	}

	public void AddStunChance(float bonus)
	{
		stunChance += bonus;
		foreach (Tower tower in towers)
		{
			tower.SetStats();
		}
		if (myBuildButton != null)
		{
			myBuildButton.UpdateModifiersText();
		}
	}

	public void AddCritChance(float bonus)
	{
		critChance += bonus;
		foreach (Tower tower in towers)
		{
			tower.SetStats();
		}
		if (myBuildButton != null)
		{
			myBuildButton.UpdateModifiersText();
		}
	}

	public void AddCritChanceLevelMultiplier(float bonus)
	{
		critChanceLevelMultiplier += bonus;
		foreach (Tower tower in towers)
		{
			tower.SetStats();
		}
		if (myBuildButton != null)
		{
			myBuildButton.UpdateModifiersText();
		}
	}

	public void AddBonusRange(float bonus)
	{
		bonusRange += bonus;
		foreach (Tower tower in towers)
		{
			tower.SetStats();
		}
		if (myBuildButton != null)
		{
			myBuildButton.UpdateModifiersText();
		}
	}

	public void AddBonusBaseDamage(int bonus)
	{
		bonusBaseDamage += bonus;
		foreach (Tower tower in towers)
		{
			tower.SetStats();
		}
		if (myBuildButton != null)
		{
			myBuildButton.UpdateModifiersText();
		}
	}

	public void AddBonusHealthDamage(int bonus)
	{
		bonusHealthDamage += bonus;
		foreach (Tower tower in towers)
		{
			tower.SetStats();
		}
		if (myBuildButton != null)
		{
			myBuildButton.UpdateModifiersText();
		}
	}

	public void AddBonusArmorDamage(int bonus)
	{
		bonusArmorDamage += bonus;
		foreach (Tower tower in towers)
		{
			tower.SetStats();
		}
		if (myBuildButton != null)
		{
			myBuildButton.UpdateModifiersText();
		}
	}

	public void AddBonusShieldDamage(int bonus)
	{
		bonusShieldDamage += bonus;
		foreach (Tower tower in towers)
		{
			tower.SetStats();
		}
		if (myBuildButton != null)
		{
			myBuildButton.UpdateModifiersText();
		}
	}

	public void AddBonusSlow(float bonus)
	{
		bonusSlow += bonus;
		foreach (Tower tower in towers)
		{
			tower.SetStats();
		}
		if (myBuildButton != null)
		{
			myBuildButton.UpdateModifiersText();
		}
	}

	public void AddBonusBleed(float bonus)
	{
		bonusBleed += bonus;
		foreach (Tower tower in towers)
		{
			tower.SetStats();
		}
		if (myBuildButton != null)
		{
			myBuildButton.UpdateModifiersText();
		}
	}

	public void AddBonusBurn(float bonus)
	{
		bonusBurn += bonus;
		foreach (Tower tower in towers)
		{
			tower.SetStats();
		}
		if (myBuildButton != null)
		{
			myBuildButton.UpdateModifiersText();
		}
	}

	public void AddBonusPoison(float bonus)
	{
		bonusPoison += bonus;
		foreach (Tower tower in towers)
		{
			tower.SetStats();
		}
		if (myBuildButton != null)
		{
			myBuildButton.UpdateModifiersText();
		}
	}

	public void AddBonusBlast(float bonus)
	{
		bonusBlast += bonus;
		foreach (Tower tower in towers)
		{
			tower.SetStats();
		}
		if (myBuildButton != null)
		{
			myBuildButton.UpdateModifiersText();
		}
	}
}
