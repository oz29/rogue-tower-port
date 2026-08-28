using UnityEngine;
using UnityEngine.UI;

public class BuildButtonUI : MonoBehaviour
{
	[SerializeField]
	private GameObject tower;

	[SerializeField]
	private TowerFlyweight towerFlyweight;

	[SerializeField]
	private Text priceTag;

	[SerializeField]
	private Text modifiersText;

	[SerializeField]
	private TowerType myTowerType;

	private void Start()
	{
		priceTag.text = towerFlyweight.currentPrice + "g";
		UpdateModifiersText();
	}

	public void Build()
	{
		BuildingManager.instance.EnterBuildMode(tower, towerFlyweight.currentPrice, towerFlyweight.priceIncrease, myTowerType);
	}

	public void UpdatePriceText()
	{
		priceTag.text = towerFlyweight.currentPrice + "g";
	}

	public void UpdateModifiersText()
	{
		if (!(modifiersText != null))
		{
			return;
		}
		string text = "";
		if (TowerManager.instance.GetBonusBaseDamage(myTowerType) > 0)
		{
			text = text + "\n+" + TowerManager.instance.GetBonusBaseDamage(myTowerType) + " Base Damage";
		}
		if (TowerManager.instance.GetBonusBaseDamage(myTowerType) < 0)
		{
			text = text + "\n" + TowerManager.instance.GetBonusBaseDamage(myTowerType) + " Base Damage";
		}
		if (TowerManager.instance.GetBonusHealthDamage(myTowerType) > 0)
		{
			text = text + "\n+" + TowerManager.instance.GetBonusHealthDamage(myTowerType) + " Health Damage";
		}
		if (TowerManager.instance.GetBonusArmorDamage(myTowerType) > 0)
		{
			text = text + "\n+" + TowerManager.instance.GetBonusArmorDamage(myTowerType) + " Armor Damage";
		}
		if (TowerManager.instance.GetBonusShieldDamage(myTowerType) > 0)
		{
			text = text + "\n+" + TowerManager.instance.GetBonusShieldDamage(myTowerType) + " Shield Damage";
		}
		if (TowerManager.instance.GetCritChance(myTowerType) + TowerManager.instance.GetCritChanceLevelMultiplier(myTowerType) > 0f)
		{
			text = text + "\n+(" + (int)(TowerManager.instance.GetCritChance(myTowerType) * 100f);
			if (TowerManager.instance.GetCritChanceLevelMultiplier(myTowerType) > 1f)
			{
				text = text + " + " + (int)TowerManager.instance.GetCritChanceLevelMultiplier(myTowerType) + "xLvl";
			}
			else if (TowerManager.instance.GetCritChanceLevelMultiplier(myTowerType) > 0f)
			{
				text += " + level";
			}
			text += ")% Crit";
		}
		if (TowerManager.instance.GetStunChance(myTowerType) > 0f)
		{
			text = text + "\n+" + Mathf.FloorToInt(TowerManager.instance.GetStunChance(myTowerType) * 101f) + "% Freeze Chance";
		}
		if (TowerManager.instance.GetBonusRange(myTowerType) > 0f)
		{
			text = text + "\n+" + TowerManager.instance.GetBonusRange(myTowerType) + " Range";
		}
		if (TowerManager.instance.GetBonusBlast(myTowerType) > 0f)
		{
			text = text + "\n+" + (int)(TowerManager.instance.GetBonusBlast(myTowerType) * 100f) + "% Blast Radius";
		}
		if (TowerManager.instance.GetBonusSlow(myTowerType) > 0f)
		{
			text = text + "\n+" + (int)(TowerManager.instance.GetBonusSlow(myTowerType) * 100f) + "% Slow";
		}
		if (TowerManager.instance.GetBonusBleed(myTowerType) > 0f)
		{
			text = text + "\n+" + (int)(TowerManager.instance.GetBonusBleed(myTowerType) * 100f) + "% Bleed";
		}
		if (TowerManager.instance.GetBonusBurn(myTowerType) > 0f)
		{
			text = text + "\n+" + (int)(TowerManager.instance.GetBonusBurn(myTowerType) * 100f) + "% Burn";
		}
		if (TowerManager.instance.GetBonusPoison(myTowerType) > 0f)
		{
			text = text + "\n+" + (int)(TowerManager.instance.GetBonusPoison(myTowerType) * 100f) + "% Poison";
		}
		modifiersText.text = text;
	}
}
