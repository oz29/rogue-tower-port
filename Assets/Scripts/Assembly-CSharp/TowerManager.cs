using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
	public static TowerManager instance;

	[SerializeField]
	private TowerFlyweight global;

	[SerializeField]
	private TowerFlyweight crossbow;

	[SerializeField]
	private TowerFlyweight morter;

	[SerializeField]
	private TowerFlyweight teslaCoil;

	[SerializeField]
	private TowerFlyweight flameThrower;

	[SerializeField]
	private TowerFlyweight poisonSprayer;

	[SerializeField]
	private TowerFlyweight frostKeep;

	[SerializeField]
	private TowerFlyweight radar;

	[SerializeField]
	private TowerFlyweight obelisk;

	[SerializeField]
	private TowerFlyweight particleCannon;

	[SerializeField]
	private TowerFlyweight shredder;

	[SerializeField]
	private TowerFlyweight encampment;

	[SerializeField]
	private TowerFlyweight lookout;

	[SerializeField]
	private TowerFlyweight siphon;

	public HashSet<TowerType> towersUsed = new HashSet<TowerType>();

	private bool usedAllTowers;

	public float obeliskTimeOnTargetMultiplier;

	private void Awake()
	{
		instance = this;
	}

	public void AddNewTower(Tower t, TowerType type)
	{
		if (type != TowerType.Siphon)
		{
			towersUsed.Add(type);
		}
		if (!usedAllTowers)
		{
			usedAllTowers = AchievementManager.instance.CheckAllTowers();
		}
		switch (type)
		{
		case TowerType.Crossbow:
			crossbow.AddNewTower(t);
			break;
		case TowerType.Morter:
			morter.AddNewTower(t);
			break;
		case TowerType.TeslaCoil:
			teslaCoil.AddNewTower(t);
			break;
		case TowerType.FlameThrower:
			flameThrower.AddNewTower(t);
			break;
		case TowerType.PoisonSprayer:
			poisonSprayer.AddNewTower(t);
			break;
		case TowerType.Frost:
			frostKeep.AddNewTower(t);
			break;
		case TowerType.Radar:
			radar.AddNewTower(t);
			break;
		case TowerType.Obelisk:
			obelisk.AddNewTower(t);
			break;
		case TowerType.ParticleCannon:
			particleCannon.AddNewTower(t);
			break;
		case TowerType.Shredder:
			shredder.AddNewTower(t);
			break;
		case TowerType.Encampment:
			encampment.AddNewTower(t);
			break;
		case TowerType.Lookout:
			lookout.AddNewTower(t);
			break;
		default:
			Debug.LogError("Failed to add tower to flyweight");
			break;
		}
	}

	public void RemoveTower(Tower t, TowerType type)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			crossbow.RemoveTower(t);
			break;
		case TowerType.Morter:
			morter.RemoveTower(t);
			break;
		case TowerType.TeslaCoil:
			teslaCoil.RemoveTower(t);
			break;
		case TowerType.FlameThrower:
			flameThrower.RemoveTower(t);
			break;
		case TowerType.PoisonSprayer:
			poisonSprayer.RemoveTower(t);
			break;
		case TowerType.Frost:
			frostKeep.RemoveTower(t);
			break;
		case TowerType.Obelisk:
			obelisk.RemoveTower(t);
			break;
		case TowerType.Radar:
			radar.RemoveTower(t);
			break;
		case TowerType.ParticleCannon:
			particleCannon.RemoveTower(t);
			break;
		case TowerType.Shredder:
			shredder.RemoveTower(t);
			break;
		case TowerType.Encampment:
			encampment.RemoveTower(t);
			break;
		case TowerType.Lookout:
			lookout.RemoveTower(t);
			break;
		default:
			Debug.LogError("Failed to add tower to flyweight");
			break;
		}
	}

	public void UpdateAllTowers()
	{
		crossbow.UpdateTowerStats();
		morter.UpdateTowerStats();
		teslaCoil.UpdateTowerStats();
		flameThrower.UpdateTowerStats();
		poisonSprayer.UpdateTowerStats();
		frostKeep.UpdateTowerStats();
		radar.UpdateTowerStats();
		obelisk.UpdateTowerStats();
		particleCannon.UpdateTowerStats();
		shredder.UpdateTowerStats();
		encampment.UpdateTowerStats();
		lookout.UpdateTowerStats();
	}

	public int GetSellPrice(TowerType type)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			return crossbow.sellPrice;
		case TowerType.Morter:
			return morter.sellPrice;
		case TowerType.TeslaCoil:
			return teslaCoil.sellPrice;
		case TowerType.FlameThrower:
			return flameThrower.sellPrice;
		case TowerType.PoisonSprayer:
			return poisonSprayer.sellPrice;
		case TowerType.Frost:
			return frostKeep.sellPrice;
		case TowerType.Obelisk:
			return obelisk.sellPrice;
		case TowerType.Radar:
			return radar.sellPrice;
		case TowerType.ParticleCannon:
			return particleCannon.sellPrice;
		case TowerType.Shredder:
			return shredder.sellPrice;
		case TowerType.Encampment:
			return encampment.sellPrice;
		case TowerType.Lookout:
			return lookout.sellPrice;
		default:
			Debug.LogError("Failed to add tower to flyweight");
			return -1;
		}
	}

	public float GetManaConsumptionBonus(TowerType type)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			return global.manaConsumptionAddition + crossbow.manaConsumptionAddition;
		case TowerType.Morter:
			return global.manaConsumptionAddition + morter.manaConsumptionAddition;
		case TowerType.TeslaCoil:
			return global.manaConsumptionAddition + teslaCoil.manaConsumptionAddition;
		case TowerType.FlameThrower:
			return global.manaConsumptionAddition + flameThrower.manaConsumptionAddition;
		case TowerType.PoisonSprayer:
			return global.manaConsumptionAddition + poisonSprayer.manaConsumptionAddition;
		case TowerType.Frost:
			return global.manaConsumptionAddition + frostKeep.manaConsumptionAddition;
		case TowerType.Radar:
			return global.manaConsumptionAddition + radar.manaConsumptionAddition;
		case TowerType.Obelisk:
			return global.manaConsumptionAddition + obelisk.manaConsumptionAddition;
		case TowerType.ParticleCannon:
			return global.manaConsumptionAddition + particleCannon.manaConsumptionAddition;
		case TowerType.Shredder:
			return global.manaConsumptionAddition + shredder.manaConsumptionAddition;
		case TowerType.Encampment:
			return global.manaConsumptionAddition + encampment.manaConsumptionAddition;
		case TowerType.Lookout:
			return global.manaConsumptionAddition + lookout.manaConsumptionAddition;
		default:
			Debug.Log("Failed to get global bonus");
			return 0f;
		}
	}

	public float GetCritChance(TowerType type)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			return global.critChance + crossbow.critChance;
		case TowerType.Morter:
			return global.critChance + morter.critChance;
		case TowerType.TeslaCoil:
			return global.critChance + teslaCoil.critChance;
		case TowerType.FlameThrower:
			return global.critChance + flameThrower.critChance;
		case TowerType.PoisonSprayer:
			return global.critChance + poisonSprayer.critChance;
		case TowerType.Frost:
			return global.critChance + frostKeep.critChance;
		case TowerType.Radar:
			return global.critChance + radar.critChance;
		case TowerType.Obelisk:
			return global.critChance + obelisk.critChance;
		case TowerType.ParticleCannon:
			return global.critChance + particleCannon.critChance;
		case TowerType.Shredder:
			return global.critChance + shredder.critChance;
		case TowerType.Encampment:
			return global.critChance + encampment.critChance;
		case TowerType.Lookout:
			return global.critChance + lookout.critChance;
		default:
			Debug.Log("Failed to get global bonus");
			return 0f;
		}
	}

	public float GetStunChance(TowerType type)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			return global.stunChance + crossbow.stunChance;
		case TowerType.Morter:
			return global.stunChance + morter.stunChance;
		case TowerType.TeslaCoil:
			return global.stunChance + teslaCoil.stunChance;
		case TowerType.FlameThrower:
			return global.stunChance + flameThrower.stunChance;
		case TowerType.PoisonSprayer:
			return global.stunChance + poisonSprayer.stunChance;
		case TowerType.Frost:
			return global.stunChance + frostKeep.stunChance;
		case TowerType.Radar:
			return global.stunChance + radar.stunChance;
		case TowerType.Obelisk:
			return global.stunChance + obelisk.stunChance;
		case TowerType.ParticleCannon:
			return global.stunChance + particleCannon.stunChance;
		case TowerType.Shredder:
			return global.stunChance + shredder.stunChance;
		case TowerType.Encampment:
			return global.stunChance + encampment.stunChance;
		case TowerType.Lookout:
			return global.stunChance + lookout.stunChance;
		default:
			Debug.Log("Failed to get global bonus");
			return 0f;
		}
	}

	public float GetCritChanceLevelMultiplier(TowerType type)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			return global.critChanceLevelMultiplier + crossbow.critChanceLevelMultiplier;
		case TowerType.Morter:
			return global.critChanceLevelMultiplier + morter.critChanceLevelMultiplier;
		case TowerType.TeslaCoil:
			return global.critChanceLevelMultiplier + teslaCoil.critChanceLevelMultiplier;
		case TowerType.FlameThrower:
			return global.critChanceLevelMultiplier + flameThrower.critChanceLevelMultiplier;
		case TowerType.PoisonSprayer:
			return global.critChanceLevelMultiplier + poisonSprayer.critChanceLevelMultiplier;
		case TowerType.Frost:
			return global.critChanceLevelMultiplier + frostKeep.critChanceLevelMultiplier;
		case TowerType.Radar:
			return global.critChanceLevelMultiplier + radar.critChanceLevelMultiplier;
		case TowerType.Obelisk:
			return global.critChanceLevelMultiplier + obelisk.critChanceLevelMultiplier;
		case TowerType.ParticleCannon:
			return global.critChanceLevelMultiplier + particleCannon.critChanceLevelMultiplier;
		case TowerType.Shredder:
			return global.critChanceLevelMultiplier + shredder.critChanceLevelMultiplier;
		case TowerType.Encampment:
			return global.critChanceLevelMultiplier + encampment.critChanceLevelMultiplier;
		case TowerType.Lookout:
			return global.critChanceLevelMultiplier + lookout.critChanceLevelMultiplier;
		default:
			Debug.Log("Failed to get global bonus");
			return 0f;
		}
	}

	public float GetBonusRange(TowerType type)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			return global.bonusRange + crossbow.bonusRange;
		case TowerType.Morter:
			return global.bonusRange + morter.bonusRange;
		case TowerType.TeslaCoil:
			return global.bonusRange + teslaCoil.bonusRange;
		case TowerType.FlameThrower:
			return global.bonusRange + flameThrower.bonusRange;
		case TowerType.PoisonSprayer:
			return global.bonusRange + poisonSprayer.bonusRange;
		case TowerType.Frost:
			return global.bonusRange + frostKeep.bonusRange;
		case TowerType.Radar:
			return global.bonusRange + radar.bonusRange;
		case TowerType.Obelisk:
			return global.bonusRange + obelisk.bonusRange;
		case TowerType.ParticleCannon:
			return global.bonusRange + particleCannon.bonusRange;
		case TowerType.Shredder:
			return global.bonusRange + shredder.bonusRange;
		case TowerType.Encampment:
			return global.bonusRange + encampment.bonusRange;
		case TowerType.Lookout:
			return global.bonusRange + lookout.bonusRange;
		default:
			Debug.Log("Failed to get global bonus");
			return 0f;
		}
	}

	public int GetBonusBaseDamage(TowerType type)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			return global.bonusBaseDamage + crossbow.bonusBaseDamage;
		case TowerType.Morter:
			return global.bonusBaseDamage + morter.bonusBaseDamage;
		case TowerType.TeslaCoil:
			return global.bonusBaseDamage + teslaCoil.bonusBaseDamage;
		case TowerType.FlameThrower:
			return global.bonusBaseDamage + flameThrower.bonusBaseDamage;
		case TowerType.PoisonSprayer:
			return global.bonusBaseDamage + poisonSprayer.bonusBaseDamage;
		case TowerType.Frost:
			return global.bonusBaseDamage + frostKeep.bonusBaseDamage;
		case TowerType.Radar:
			return global.bonusBaseDamage + radar.bonusBaseDamage;
		case TowerType.Obelisk:
			return global.bonusBaseDamage + obelisk.bonusBaseDamage;
		case TowerType.ParticleCannon:
			return global.bonusBaseDamage + particleCannon.bonusBaseDamage;
		case TowerType.Shredder:
			return global.bonusBaseDamage + shredder.bonusBaseDamage;
		case TowerType.Encampment:
			return global.bonusBaseDamage + encampment.bonusBaseDamage;
		case TowerType.Lookout:
			return global.bonusBaseDamage + lookout.bonusBaseDamage;
		default:
			Debug.Log("Failed to get global bonus");
			return 0;
		}
	}

	public int GetBonusHealthDamage(TowerType type)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			return global.bonusHealthDamage + crossbow.bonusHealthDamage;
		case TowerType.Morter:
			return global.bonusHealthDamage + morter.bonusHealthDamage;
		case TowerType.TeslaCoil:
			return global.bonusHealthDamage + teslaCoil.bonusHealthDamage;
		case TowerType.FlameThrower:
			return global.bonusHealthDamage + flameThrower.bonusHealthDamage;
		case TowerType.PoisonSprayer:
			return global.bonusHealthDamage + poisonSprayer.bonusHealthDamage;
		case TowerType.Frost:
			return global.bonusHealthDamage + frostKeep.bonusHealthDamage;
		case TowerType.Radar:
			return global.bonusHealthDamage + radar.bonusHealthDamage;
		case TowerType.Obelisk:
			return global.bonusHealthDamage + obelisk.bonusHealthDamage;
		case TowerType.ParticleCannon:
			return global.bonusHealthDamage + particleCannon.bonusHealthDamage;
		case TowerType.Shredder:
			return global.bonusHealthDamage + shredder.bonusHealthDamage;
		case TowerType.Encampment:
			return global.bonusHealthDamage + encampment.bonusHealthDamage;
		case TowerType.Lookout:
			return global.bonusHealthDamage + lookout.bonusHealthDamage;
		default:
			Debug.Log("Failed to get global bonus");
			return 0;
		}
	}

	public int GetBonusArmorDamage(TowerType type)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			return global.bonusArmorDamage + crossbow.bonusArmorDamage;
		case TowerType.Morter:
			return global.bonusArmorDamage + morter.bonusArmorDamage;
		case TowerType.TeslaCoil:
			return global.bonusArmorDamage + teslaCoil.bonusArmorDamage;
		case TowerType.FlameThrower:
			return global.bonusArmorDamage + flameThrower.bonusArmorDamage;
		case TowerType.PoisonSprayer:
			return global.bonusArmorDamage + poisonSprayer.bonusArmorDamage;
		case TowerType.Frost:
			return global.bonusArmorDamage + frostKeep.bonusArmorDamage;
		case TowerType.Radar:
			return global.bonusArmorDamage + radar.bonusArmorDamage;
		case TowerType.Obelisk:
			return global.bonusArmorDamage + obelisk.bonusArmorDamage;
		case TowerType.ParticleCannon:
			return global.bonusArmorDamage + particleCannon.bonusArmorDamage;
		case TowerType.Shredder:
			return global.bonusArmorDamage + shredder.bonusArmorDamage;
		case TowerType.Encampment:
			return global.bonusArmorDamage + encampment.bonusArmorDamage;
		case TowerType.Lookout:
			return global.bonusArmorDamage + lookout.bonusArmorDamage;
		default:
			Debug.Log("Failed to get global bonus");
			return 0;
		}
	}

	public int GetBonusShieldDamage(TowerType type)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			return global.bonusShieldDamage + crossbow.bonusShieldDamage;
		case TowerType.Morter:
			return global.bonusShieldDamage + morter.bonusShieldDamage;
		case TowerType.TeslaCoil:
			return global.bonusShieldDamage + teslaCoil.bonusShieldDamage;
		case TowerType.FlameThrower:
			return global.bonusShieldDamage + flameThrower.bonusShieldDamage;
		case TowerType.PoisonSprayer:
			return global.bonusShieldDamage + poisonSprayer.bonusShieldDamage;
		case TowerType.Frost:
			return global.bonusShieldDamage + frostKeep.bonusShieldDamage;
		case TowerType.Radar:
			return global.bonusShieldDamage + radar.bonusShieldDamage;
		case TowerType.Obelisk:
			return global.bonusShieldDamage + obelisk.bonusShieldDamage;
		case TowerType.ParticleCannon:
			return global.bonusShieldDamage + particleCannon.bonusShieldDamage;
		case TowerType.Shredder:
			return global.bonusShieldDamage + shredder.bonusShieldDamage;
		case TowerType.Encampment:
			return global.bonusShieldDamage + encampment.bonusShieldDamage;
		case TowerType.Lookout:
			return global.bonusShieldDamage + lookout.bonusShieldDamage;
		default:
			Debug.Log("Failed to get global bonus");
			return 0;
		}
	}

	public float GetBonusSlow(TowerType type)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			return global.bonusSlow + crossbow.bonusSlow;
		case TowerType.Morter:
			return global.bonusSlow + morter.bonusSlow;
		case TowerType.TeslaCoil:
			return global.bonusSlow + teslaCoil.bonusSlow;
		case TowerType.FlameThrower:
			return global.bonusSlow + flameThrower.bonusSlow;
		case TowerType.PoisonSprayer:
			return global.bonusSlow + poisonSprayer.bonusSlow;
		case TowerType.Frost:
			return global.bonusSlow + frostKeep.bonusSlow;
		case TowerType.Radar:
			return global.bonusSlow + radar.bonusSlow;
		case TowerType.Obelisk:
			return global.bonusSlow + obelisk.bonusSlow;
		case TowerType.ParticleCannon:
			return global.bonusSlow + particleCannon.bonusSlow;
		case TowerType.Shredder:
			return global.bonusSlow + shredder.bonusSlow;
		case TowerType.Encampment:
			return global.bonusSlow + encampment.bonusSlow;
		case TowerType.Lookout:
			return global.bonusSlow + lookout.bonusSlow;
		default:
			Debug.Log("Failed to get global bonus");
			return 0f;
		}
	}

	public float GetBonusBleed(TowerType type)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			return global.bonusBleed + crossbow.bonusBleed;
		case TowerType.Morter:
			return global.bonusBleed + morter.bonusBleed;
		case TowerType.TeslaCoil:
			return global.bonusBleed + teslaCoil.bonusBleed;
		case TowerType.FlameThrower:
			return global.bonusBleed + flameThrower.bonusBleed;
		case TowerType.PoisonSprayer:
			return global.bonusBleed + poisonSprayer.bonusBleed;
		case TowerType.Frost:
			return global.bonusBleed + frostKeep.bonusBleed;
		case TowerType.Radar:
			return global.bonusBleed + radar.bonusBleed;
		case TowerType.Obelisk:
			return global.bonusBleed + obelisk.bonusBleed;
		case TowerType.ParticleCannon:
			return global.bonusBleed + particleCannon.bonusBleed;
		case TowerType.Shredder:
			return global.bonusBleed + shredder.bonusBleed;
		case TowerType.Encampment:
			return global.bonusBleed + encampment.bonusBleed;
		case TowerType.Lookout:
			return global.bonusBleed + lookout.bonusBleed;
		default:
			Debug.Log("Failed to get global bonus");
			return 0f;
		}
	}

	public float GetBonusBurn(TowerType type)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			return global.bonusBurn + crossbow.bonusBurn;
		case TowerType.Morter:
			return global.bonusBurn + morter.bonusBurn;
		case TowerType.TeslaCoil:
			return global.bonusBurn + teslaCoil.bonusBurn;
		case TowerType.FlameThrower:
			return global.bonusBurn + flameThrower.bonusBurn;
		case TowerType.PoisonSprayer:
			return global.bonusBurn + poisonSprayer.bonusBurn;
		case TowerType.Frost:
			return global.bonusBurn + frostKeep.bonusBurn;
		case TowerType.Radar:
			return global.bonusBurn + radar.bonusBurn;
		case TowerType.Obelisk:
			return global.bonusBurn + obelisk.bonusBurn;
		case TowerType.ParticleCannon:
			return global.bonusBurn + particleCannon.bonusBurn;
		case TowerType.Shredder:
			return global.bonusBurn + shredder.bonusBurn;
		case TowerType.Encampment:
			return global.bonusBurn + encampment.bonusBurn;
		case TowerType.Lookout:
			return global.bonusBurn + lookout.bonusBurn;
		default:
			Debug.Log("Failed to get global bonus");
			return 0f;
		}
	}

	public float GetBonusPoison(TowerType type)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			return global.bonusPoison + crossbow.bonusPoison;
		case TowerType.Morter:
			return global.bonusPoison + morter.bonusPoison;
		case TowerType.TeslaCoil:
			return global.bonusPoison + teslaCoil.bonusPoison;
		case TowerType.FlameThrower:
			return global.bonusPoison + flameThrower.bonusPoison;
		case TowerType.PoisonSprayer:
			return global.bonusPoison + poisonSprayer.bonusPoison;
		case TowerType.Frost:
			return global.bonusPoison + frostKeep.bonusPoison;
		case TowerType.Radar:
			return global.bonusPoison + radar.bonusPoison;
		case TowerType.Obelisk:
			return global.bonusPoison + obelisk.bonusPoison;
		case TowerType.ParticleCannon:
			return global.bonusPoison + particleCannon.bonusPoison;
		case TowerType.Shredder:
			return global.bonusPoison + shredder.bonusPoison;
		case TowerType.Encampment:
			return global.bonusPoison + encampment.bonusPoison;
		case TowerType.Lookout:
			return global.bonusPoison + lookout.bonusPoison;
		default:
			Debug.Log("Failed to get global bonus");
			return 0f;
		}
	}

	public float GetBonusBlast(TowerType type)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			return global.bonusBlast + crossbow.bonusBlast;
		case TowerType.Morter:
			return global.bonusBlast + morter.bonusBlast;
		case TowerType.TeslaCoil:
			return global.bonusBlast + teslaCoil.bonusBlast;
		case TowerType.FlameThrower:
			return global.bonusBlast + flameThrower.bonusBlast;
		case TowerType.PoisonSprayer:
			return global.bonusBlast + poisonSprayer.bonusBlast;
		case TowerType.Frost:
			return global.bonusBlast + frostKeep.bonusBlast;
		case TowerType.Radar:
			return global.bonusBlast + radar.bonusBlast;
		case TowerType.Obelisk:
			return global.bonusBlast + obelisk.bonusBlast;
		case TowerType.ParticleCannon:
			return global.bonusBlast + particleCannon.bonusBlast;
		case TowerType.Shredder:
			return global.bonusBlast + shredder.bonusBlast;
		case TowerType.Encampment:
			return global.bonusBlast + encampment.bonusBlast;
		case TowerType.Lookout:
			return global.bonusBlast + lookout.bonusBlast;
		default:
			Debug.Log("Failed to get global bonus");
			return 0f;
		}
	}

	public void AddManaConsumption(TowerType type, float value)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			crossbow.AddManaAddition(value);
			break;
		case TowerType.Morter:
			morter.AddManaAddition(value);
			break;
		case TowerType.TeslaCoil:
			teslaCoil.AddManaAddition(value);
			break;
		case TowerType.FlameThrower:
			flameThrower.AddManaAddition(value);
			break;
		case TowerType.PoisonSprayer:
			poisonSprayer.AddManaAddition(value);
			break;
		case TowerType.Frost:
			frostKeep.AddManaAddition(value);
			break;
		case TowerType.Radar:
			radar.AddManaAddition(value);
			break;
		case TowerType.Obelisk:
			obelisk.AddManaAddition(value);
			break;
		case TowerType.ParticleCannon:
			particleCannon.AddManaAddition(value);
			break;
		case TowerType.Shredder:
			shredder.AddManaAddition(value);
			break;
		case TowerType.Encampment:
			encampment.AddManaAddition(value);
			break;
		case TowerType.Lookout:
			lookout.AddManaAddition(value);
			break;
		case TowerType.Global:
			global.AddManaAddition(value);
			UpdateAllTowers();
			break;
		default:
			Debug.Log("Failed to set global bonus");
			break;
		}
	}

	public void AddCritChance(TowerType type, float value)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			crossbow.AddCritChance(value);
			break;
		case TowerType.Morter:
			morter.AddCritChance(value);
			break;
		case TowerType.TeslaCoil:
			teslaCoil.AddCritChance(value);
			break;
		case TowerType.FlameThrower:
			flameThrower.AddCritChance(value);
			break;
		case TowerType.PoisonSprayer:
			poisonSprayer.AddCritChance(value);
			break;
		case TowerType.Frost:
			frostKeep.AddCritChance(value);
			break;
		case TowerType.Radar:
			radar.AddCritChance(value);
			break;
		case TowerType.Obelisk:
			obelisk.AddCritChance(value);
			break;
		case TowerType.ParticleCannon:
			particleCannon.AddCritChance(value);
			break;
		case TowerType.Shredder:
			shredder.AddCritChance(value);
			break;
		case TowerType.Encampment:
			encampment.AddCritChance(value);
			break;
		case TowerType.Lookout:
			lookout.AddCritChance(value);
			break;
		case TowerType.Global:
			global.AddCritChance(value);
			UpdateAllTowers();
			break;
		default:
			Debug.Log("Failed to set global bonus");
			break;
		}
	}

	public void AddStunChance(TowerType type, float value)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			crossbow.AddStunChance(value);
			break;
		case TowerType.Morter:
			morter.AddStunChance(value);
			break;
		case TowerType.TeslaCoil:
			teslaCoil.AddStunChance(value);
			break;
		case TowerType.FlameThrower:
			flameThrower.AddStunChance(value);
			break;
		case TowerType.PoisonSprayer:
			poisonSprayer.AddStunChance(value);
			break;
		case TowerType.Frost:
			frostKeep.AddStunChance(value);
			break;
		case TowerType.Radar:
			radar.AddStunChance(value);
			break;
		case TowerType.Obelisk:
			obelisk.AddStunChance(value);
			break;
		case TowerType.ParticleCannon:
			particleCannon.AddStunChance(value);
			break;
		case TowerType.Shredder:
			shredder.AddStunChance(value);
			break;
		case TowerType.Encampment:
			encampment.AddStunChance(value);
			break;
		case TowerType.Lookout:
			lookout.AddStunChance(value);
			break;
		case TowerType.Global:
			global.AddStunChance(value);
			UpdateAllTowers();
			break;
		default:
			Debug.Log("Failed to set global bonus");
			break;
		}
	}

	public void AddCritChanceLevelMultiplier(TowerType type, float value)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			crossbow.AddCritChanceLevelMultiplier(value);
			break;
		case TowerType.Morter:
			morter.AddCritChanceLevelMultiplier(value);
			break;
		case TowerType.TeslaCoil:
			teslaCoil.AddCritChanceLevelMultiplier(value);
			break;
		case TowerType.FlameThrower:
			flameThrower.AddCritChanceLevelMultiplier(value);
			break;
		case TowerType.PoisonSprayer:
			poisonSprayer.AddCritChanceLevelMultiplier(value);
			break;
		case TowerType.Frost:
			frostKeep.AddCritChanceLevelMultiplier(value);
			break;
		case TowerType.Radar:
			radar.AddCritChanceLevelMultiplier(value);
			break;
		case TowerType.Obelisk:
			obelisk.AddCritChanceLevelMultiplier(value);
			break;
		case TowerType.ParticleCannon:
			particleCannon.AddCritChanceLevelMultiplier(value);
			break;
		case TowerType.Shredder:
			shredder.AddCritChanceLevelMultiplier(value);
			break;
		case TowerType.Encampment:
			encampment.AddCritChanceLevelMultiplier(value);
			break;
		case TowerType.Lookout:
			lookout.AddCritChanceLevelMultiplier(value);
			break;
		case TowerType.Global:
			global.AddCritChanceLevelMultiplier(value);
			UpdateAllTowers();
			break;
		default:
			Debug.Log("Failed to set global bonus");
			break;
		}
	}

	public void AddBonusRange(TowerType type, float value)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			crossbow.AddBonusRange(value);
			break;
		case TowerType.Morter:
			morter.AddBonusRange(value);
			break;
		case TowerType.TeslaCoil:
			teslaCoil.AddBonusRange(value);
			break;
		case TowerType.FlameThrower:
			flameThrower.AddBonusRange(value);
			break;
		case TowerType.PoisonSprayer:
			poisonSprayer.AddBonusRange(value);
			break;
		case TowerType.Frost:
			frostKeep.AddBonusRange(value);
			break;
		case TowerType.Radar:
			radar.AddBonusRange(value);
			break;
		case TowerType.Obelisk:
			obelisk.AddBonusRange(value);
			break;
		case TowerType.ParticleCannon:
			particleCannon.AddBonusRange(value);
			break;
		case TowerType.Shredder:
			shredder.AddBonusRange(value);
			break;
		case TowerType.Encampment:
			encampment.AddBonusRange(value);
			break;
		case TowerType.Lookout:
			lookout.AddBonusRange(value);
			break;
		case TowerType.Global:
			global.AddBonusRange(value);
			UpdateAllTowers();
			break;
		default:
			Debug.Log("Failed to set global bonus");
			break;
		}
	}

	public void AddBonusBaseDamage(TowerType type, int value)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			crossbow.AddBonusBaseDamage(value);
			break;
		case TowerType.Morter:
			morter.AddBonusBaseDamage(value);
			break;
		case TowerType.TeslaCoil:
			teslaCoil.AddBonusBaseDamage(value);
			break;
		case TowerType.FlameThrower:
			flameThrower.AddBonusBaseDamage(value);
			break;
		case TowerType.PoisonSprayer:
			poisonSprayer.AddBonusBaseDamage(value);
			break;
		case TowerType.Frost:
			frostKeep.AddBonusBaseDamage(value);
			break;
		case TowerType.Radar:
			radar.AddBonusBaseDamage(value);
			break;
		case TowerType.Obelisk:
			obelisk.AddBonusBaseDamage(value);
			break;
		case TowerType.ParticleCannon:
			particleCannon.AddBonusBaseDamage(value);
			break;
		case TowerType.Shredder:
			shredder.AddBonusBaseDamage(value);
			break;
		case TowerType.Encampment:
			encampment.AddBonusBaseDamage(value);
			break;
		case TowerType.Lookout:
			lookout.AddBonusBaseDamage(value);
			break;
		case TowerType.Global:
			global.AddBonusBaseDamage(value);
			UpdateAllTowers();
			break;
		default:
			Debug.Log("Failed to set global bonus");
			break;
		}
	}

	public void AddBonusHealthDamage(TowerType type, int value)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			crossbow.AddBonusHealthDamage(value);
			break;
		case TowerType.Morter:
			morter.AddBonusHealthDamage(value);
			break;
		case TowerType.TeslaCoil:
			teslaCoil.AddBonusHealthDamage(value);
			break;
		case TowerType.FlameThrower:
			flameThrower.AddBonusHealthDamage(value);
			break;
		case TowerType.PoisonSprayer:
			poisonSprayer.AddBonusHealthDamage(value);
			break;
		case TowerType.Frost:
			frostKeep.AddBonusHealthDamage(value);
			break;
		case TowerType.Radar:
			radar.AddBonusHealthDamage(value);
			break;
		case TowerType.Obelisk:
			obelisk.AddBonusHealthDamage(value);
			break;
		case TowerType.ParticleCannon:
			particleCannon.AddBonusHealthDamage(value);
			break;
		case TowerType.Shredder:
			shredder.AddBonusHealthDamage(value);
			break;
		case TowerType.Encampment:
			encampment.AddBonusHealthDamage(value);
			break;
		case TowerType.Lookout:
			lookout.AddBonusHealthDamage(value);
			break;
		case TowerType.Global:
			global.AddBonusHealthDamage(value);
			UpdateAllTowers();
			break;
		default:
			Debug.Log("Failed to set global bonus");
			break;
		}
	}

	public void AddBonusArmorDamage(TowerType type, int value)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			crossbow.AddBonusArmorDamage(value);
			break;
		case TowerType.Morter:
			morter.AddBonusArmorDamage(value);
			break;
		case TowerType.TeslaCoil:
			teslaCoil.AddBonusArmorDamage(value);
			break;
		case TowerType.FlameThrower:
			flameThrower.AddBonusArmorDamage(value);
			break;
		case TowerType.PoisonSprayer:
			poisonSprayer.AddBonusArmorDamage(value);
			break;
		case TowerType.Frost:
			frostKeep.AddBonusArmorDamage(value);
			break;
		case TowerType.Radar:
			radar.AddBonusArmorDamage(value);
			break;
		case TowerType.Obelisk:
			obelisk.AddBonusArmorDamage(value);
			break;
		case TowerType.ParticleCannon:
			particleCannon.AddBonusArmorDamage(value);
			break;
		case TowerType.Shredder:
			shredder.AddBonusArmorDamage(value);
			break;
		case TowerType.Encampment:
			encampment.AddBonusArmorDamage(value);
			break;
		case TowerType.Lookout:
			lookout.AddBonusArmorDamage(value);
			break;
		case TowerType.Global:
			global.AddBonusArmorDamage(value);
			UpdateAllTowers();
			break;
		default:
			Debug.Log("Failed to set global bonus");
			break;
		}
	}

	public void AddBonusShieldDamage(TowerType type, int value)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			crossbow.AddBonusShieldDamage(value);
			break;
		case TowerType.Morter:
			morter.AddBonusShieldDamage(value);
			break;
		case TowerType.TeslaCoil:
			teslaCoil.AddBonusShieldDamage(value);
			break;
		case TowerType.FlameThrower:
			flameThrower.AddBonusShieldDamage(value);
			break;
		case TowerType.PoisonSprayer:
			poisonSprayer.AddBonusShieldDamage(value);
			break;
		case TowerType.Frost:
			frostKeep.AddBonusShieldDamage(value);
			break;
		case TowerType.Radar:
			radar.AddBonusShieldDamage(value);
			break;
		case TowerType.Obelisk:
			obelisk.AddBonusShieldDamage(value);
			break;
		case TowerType.ParticleCannon:
			particleCannon.AddBonusShieldDamage(value);
			break;
		case TowerType.Shredder:
			shredder.AddBonusShieldDamage(value);
			break;
		case TowerType.Encampment:
			encampment.AddBonusShieldDamage(value);
			break;
		case TowerType.Lookout:
			lookout.AddBonusShieldDamage(value);
			break;
		case TowerType.Global:
			global.AddBonusShieldDamage(value);
			UpdateAllTowers();
			break;
		default:
			Debug.Log("Failed to set global bonus");
			break;
		}
	}

	public void AddBonusSlow(TowerType type, float value)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			crossbow.AddBonusSlow(value);
			break;
		case TowerType.Morter:
			morter.AddBonusSlow(value);
			break;
		case TowerType.TeslaCoil:
			teslaCoil.AddBonusSlow(value);
			break;
		case TowerType.FlameThrower:
			flameThrower.AddBonusSlow(value);
			break;
		case TowerType.PoisonSprayer:
			poisonSprayer.AddBonusSlow(value);
			break;
		case TowerType.Frost:
			frostKeep.AddBonusSlow(value);
			break;
		case TowerType.Radar:
			radar.AddBonusSlow(value);
			break;
		case TowerType.Obelisk:
			obelisk.AddBonusSlow(value);
			break;
		case TowerType.ParticleCannon:
			particleCannon.AddBonusSlow(value);
			break;
		case TowerType.Shredder:
			shredder.AddBonusSlow(value);
			break;
		case TowerType.Encampment:
			encampment.AddBonusSlow(value);
			break;
		case TowerType.Lookout:
			lookout.AddBonusSlow(value);
			break;
		case TowerType.Global:
			global.AddBonusSlow(value);
			UpdateAllTowers();
			break;
		default:
			Debug.Log("Failed to set global bonus");
			break;
		}
	}

	public void AddBonusBleed(TowerType type, float value)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			crossbow.AddBonusBleed(value);
			break;
		case TowerType.Morter:
			morter.AddBonusBleed(value);
			break;
		case TowerType.TeslaCoil:
			teslaCoil.AddBonusBleed(value);
			break;
		case TowerType.FlameThrower:
			flameThrower.AddBonusBleed(value);
			break;
		case TowerType.PoisonSprayer:
			poisonSprayer.AddBonusBleed(value);
			break;
		case TowerType.Frost:
			frostKeep.AddBonusBleed(value);
			break;
		case TowerType.Radar:
			radar.AddBonusBleed(value);
			break;
		case TowerType.Obelisk:
			obelisk.AddBonusBleed(value);
			break;
		case TowerType.ParticleCannon:
			particleCannon.AddBonusBleed(value);
			break;
		case TowerType.Shredder:
			shredder.AddBonusBleed(value);
			break;
		case TowerType.Encampment:
			encampment.AddBonusBleed(value);
			break;
		case TowerType.Lookout:
			lookout.AddBonusBleed(value);
			break;
		case TowerType.Global:
			global.AddBonusBleed(value);
			UpdateAllTowers();
			break;
		default:
			Debug.Log("Failed to set global bonus");
			break;
		}
	}

	public void AddBonusBurn(TowerType type, float value)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			crossbow.AddBonusBurn(value);
			break;
		case TowerType.Morter:
			morter.AddBonusBurn(value);
			break;
		case TowerType.TeslaCoil:
			teslaCoil.AddBonusBurn(value);
			break;
		case TowerType.FlameThrower:
			flameThrower.AddBonusBurn(value);
			break;
		case TowerType.PoisonSprayer:
			poisonSprayer.AddBonusBurn(value);
			break;
		case TowerType.Frost:
			frostKeep.AddBonusBurn(value);
			break;
		case TowerType.Radar:
			radar.AddBonusBurn(value);
			break;
		case TowerType.Obelisk:
			obelisk.AddBonusBurn(value);
			break;
		case TowerType.ParticleCannon:
			particleCannon.AddBonusBurn(value);
			break;
		case TowerType.Shredder:
			shredder.AddBonusBurn(value);
			break;
		case TowerType.Encampment:
			encampment.AddBonusBurn(value);
			break;
		case TowerType.Lookout:
			lookout.AddBonusBurn(value);
			break;
		case TowerType.Global:
			global.AddBonusBurn(value);
			UpdateAllTowers();
			break;
		default:
			Debug.Log("Failed to set global bonus");
			break;
		}
	}

	public void AddBonusPoison(TowerType type, float value)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			crossbow.AddBonusPoison(value);
			break;
		case TowerType.Morter:
			morter.AddBonusPoison(value);
			break;
		case TowerType.TeslaCoil:
			teslaCoil.AddBonusPoison(value);
			break;
		case TowerType.FlameThrower:
			flameThrower.AddBonusPoison(value);
			break;
		case TowerType.PoisonSprayer:
			poisonSprayer.AddBonusPoison(value);
			break;
		case TowerType.Frost:
			frostKeep.AddBonusPoison(value);
			break;
		case TowerType.Radar:
			radar.AddBonusPoison(value);
			break;
		case TowerType.Obelisk:
			obelisk.AddBonusPoison(value);
			break;
		case TowerType.ParticleCannon:
			particleCannon.AddBonusPoison(value);
			break;
		case TowerType.Shredder:
			shredder.AddBonusPoison(value);
			break;
		case TowerType.Encampment:
			encampment.AddBonusPoison(value);
			break;
		case TowerType.Lookout:
			lookout.AddBonusPoison(value);
			break;
		case TowerType.Global:
			global.AddBonusPoison(value);
			UpdateAllTowers();
			break;
		default:
			Debug.Log("Failed to set global bonus");
			break;
		}
	}

	public void AddBonusBlast(TowerType type, float value)
	{
		switch (type)
		{
		case TowerType.Crossbow:
			crossbow.AddBonusBlast(value);
			break;
		case TowerType.Morter:
			morter.AddBonusBlast(value);
			break;
		case TowerType.TeslaCoil:
			teslaCoil.AddBonusBlast(value);
			break;
		case TowerType.FlameThrower:
			flameThrower.AddBonusBlast(value);
			break;
		case TowerType.PoisonSprayer:
			poisonSprayer.AddBonusBlast(value);
			break;
		case TowerType.Frost:
			frostKeep.AddBonusBlast(value);
			break;
		case TowerType.Radar:
			radar.AddBonusBlast(value);
			break;
		case TowerType.Obelisk:
			obelisk.AddBonusBlast(value);
			break;
		case TowerType.ParticleCannon:
			particleCannon.AddBonusBlast(value);
			break;
		case TowerType.Shredder:
			shredder.AddBonusBlast(value);
			break;
		case TowerType.Encampment:
			encampment.AddBonusBlast(value);
			break;
		case TowerType.Lookout:
			lookout.AddBonusBlast(value);
			break;
		case TowerType.Global:
			global.AddBonusBlast(value);
			UpdateAllTowers();
			break;
		default:
			Debug.Log("Failed to set global bonus");
			break;
		}
	}
}
