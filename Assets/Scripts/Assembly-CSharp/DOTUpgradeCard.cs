using UnityEngine;
using UnityEngine.UI;

public class DOTUpgradeCard : UpgradeCard
{
	[SerializeField]
	private int bleedAmount;

	[SerializeField]
	private int burnAmount;

	[SerializeField]
	private int poisonAmount;

	[SerializeField]
	private GameObject bleedUI;

	[SerializeField]
	private GameObject burnUI;

	[SerializeField]
	private GameObject poisonUI;

	[SerializeField]
	private GameObject stunDmgUI;

	[SerializeField]
	private Text bleedText;

	[SerializeField]
	private Text burnText;

	[SerializeField]
	private Text poisonText;

	[SerializeField]
	private Text stunDmgText;

	[SerializeField]
	private int bonusDamageOnBleed;

	[SerializeField]
	private int bonusDamageOnBurn;

	[SerializeField]
	private int bonusDamageOnPoison;

	[SerializeField]
	private int bonusDamageOnStun;

	[SerializeField]
	private float poisonSlowPercent;

	[SerializeField]
	private float burnSpeedDamagePercentBonus;

	[SerializeField]
	private float bleedingCritChance;

	[SerializeField]
	private float bleedPop;

	[SerializeField]
	private float burnPop;

	[SerializeField]
	private float poisonPop;

	public override void Upgrade()
	{
		base.Upgrade();
		GameManager.instance.dotTick += new Vector3(bleedAmount, burnAmount, poisonAmount);
		MonsterManager.instance.bonusDamageOnBleed += bonusDamageOnBleed;
		MonsterManager.instance.bonusDamageOnBurn += bonusDamageOnBurn;
		MonsterManager.instance.bonusDamageOnPoison += bonusDamageOnPoison;
		MonsterManager.instance.bonusDamageOnStun += bonusDamageOnStun;
		MonsterManager.instance.poisonSlowPercent += poisonSlowPercent;
		MonsterManager.instance.burnSpeedDamagePercentBonus += burnSpeedDamagePercentBonus;
		MonsterManager.instance.bleedingCritChance += bleedingCritChance;
		MonsterManager.instance.bleedPop += bleedPop;
		MonsterManager.instance.burnPop += burnPop;
		MonsterManager.instance.poisonPop += poisonPop;
		if (bleedAmount > 0 || bonusDamageOnBleed > 0)
		{
			bleedUI.SetActive(value: true);
			if (bleedText != null) { bleedText.text = ""; bleedText.gameObject.SetActive(false); }
		}
		if (burnAmount > 0 || bonusDamageOnBurn > 0 || burnSpeedDamagePercentBonus > 0f)
		{
			burnUI.SetActive(value: true);
			if (burnText != null) { burnText.text = ""; burnText.gameObject.SetActive(false); }
		}
		if (poisonAmount > 0 || bonusDamageOnPoison > 0)
		{
			poisonUI.SetActive(value: true);
			if (poisonText != null) { poisonText.text = ""; poisonText.gameObject.SetActive(false); }
		}
		if (bonusDamageOnStun > 0)
		{
			stunDmgUI.SetActive(value: true);
			if (stunDmgText != null) { stunDmgText.text = ""; stunDmgText.gameObject.SetActive(false); }
		}
	}
}
