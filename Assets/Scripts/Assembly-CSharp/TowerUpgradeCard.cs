using UnityEngine;

public class TowerUpgradeCard : UpgradeCard
{
	[SerializeField]
	private TowerType towerType;

	[SerializeField]
	private float range;

	[SerializeField]
	private int damage;

	[SerializeField]
	private int healthDmg;

	[SerializeField]
	private int armorDmg;

	[SerializeField]
	private int shieldDmg;

	[SerializeField]
	private float slowPercent;

	[SerializeField]
	private float bleedPercent;

	[SerializeField]
	private float burnPercent;

	[SerializeField]
	private float poisonPercent;

	[SerializeField]
	private float blast;

	[SerializeField]
	private float critChance;

	[SerializeField]
	private float critChanceLevelMultiplier;

	[SerializeField]
	private float stunChance;

	[SerializeField]
	private float manaConsumptionAddition;

	[SerializeField]
	private float obeliskTimeOnTargetMultiplier;

	public override void Upgrade()
	{
		if (range != 0f)
		{
			TowerManager.instance.AddBonusRange(towerType, range);
		}
		if (damage != 0)
		{
			TowerManager.instance.AddBonusBaseDamage(towerType, damage);
		}
		if (healthDmg != 0)
		{
			TowerManager.instance.AddBonusHealthDamage(towerType, healthDmg);
		}
		if (armorDmg != 0)
		{
			TowerManager.instance.AddBonusArmorDamage(towerType, armorDmg);
		}
		if (shieldDmg != 0)
		{
			TowerManager.instance.AddBonusShieldDamage(towerType, shieldDmg);
		}
		if (slowPercent != 0f)
		{
			TowerManager.instance.AddBonusSlow(towerType, slowPercent);
		}
		if (bleedPercent != 0f)
		{
			TowerManager.instance.AddBonusBleed(towerType, bleedPercent);
		}
		if (burnPercent != 0f)
		{
			TowerManager.instance.AddBonusBurn(towerType, burnPercent);
		}
		if (poisonPercent != 0f)
		{
			TowerManager.instance.AddBonusPoison(towerType, poisonPercent);
		}
		if (blast != 0f)
		{
			TowerManager.instance.AddBonusBlast(towerType, blast);
		}
		if (critChance != 0f)
		{
			TowerManager.instance.AddCritChance(towerType, critChance);
		}
		if (critChanceLevelMultiplier != 0f)
		{
			TowerManager.instance.AddCritChanceLevelMultiplier(towerType, critChanceLevelMultiplier);
		}
		if (stunChance != 0f)
		{
			TowerManager.instance.AddStunChance(towerType, stunChance);
		}
		if (manaConsumptionAddition != 0f)
		{
			TowerManager.instance.AddManaConsumption(towerType, manaConsumptionAddition);
		}
		if (obeliskTimeOnTargetMultiplier != 0f)
		{
			TowerManager.instance.obeliskTimeOnTargetMultiplier += obeliskTimeOnTargetMultiplier;
		}
		base.Upgrade();
	}
}
