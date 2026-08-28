using UnityEngine;

public class MonsterManager : MonoBehaviour
{
	public static MonsterManager instance;

	public int extraTowerDamage;

	public int extraGoldDrop;

	public float manaDropOnDeath;

	public float speedBonus;

	public int bonusDamageOnBleed;

	public int bonusDamageOnBurn;

	public int bonusDamageOnPoison;

	public int bonusDamageOnStun;

	public float poisonSlowPercent;

	public float burnSpeedDamagePercentBonus;

	public float bleedingCritChance;

	public float bleedPop;

	public float burnPop;

	public float poisonPop;

	public float slowCapModifier;

	public float hasteCapModifier;

	private void Awake()
	{
		instance = this;
	}
}
