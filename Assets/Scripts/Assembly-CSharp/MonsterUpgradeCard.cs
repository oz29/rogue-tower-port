using UnityEngine;
using UnityEngine.UI;

public class MonsterUpgradeCard : UpgradeCard
{
	[SerializeField]
	private GameObject banditObject;

	[SerializeField]
	private Text banditText;

	[SerializeField]
	private int extraTowerDamage;

	[SerializeField]
	private int extraGoldDrop;

	[SerializeField]
	private float manaDropOnDeath;

	[SerializeField]
	private float speedBonus;

	[SerializeField]
	private float slowCapMod;

	[SerializeField]
	private float hasteCapMod;

	public override void Upgrade()
	{
		base.Upgrade();
		MonsterManager.instance.extraTowerDamage += extraTowerDamage;
		MonsterManager.instance.extraGoldDrop += extraGoldDrop;
		MonsterManager.instance.manaDropOnDeath += manaDropOnDeath;
		MonsterManager.instance.speedBonus += speedBonus;
		MonsterManager.instance.slowCapModifier += slowCapMod;
		MonsterManager.instance.hasteCapModifier += hasteCapMod;
		if (extraGoldDrop > 0)
		{
			banditObject.SetActive(value: true);
			if (banditText != null) { banditText.text = ""; banditText.gameObject.SetActive(false); }
		}
		if (manaDropOnDeath > 0f)
		{
			ResourceManager.instance.UpdateManaHUD();
		}
		if ((double)MonsterManager.instance.slowCapModifier >= 0.3)
		{
			AchievementManager.instance.UnlockAchievement("MaxSlow");
		}
		if (MonsterManager.instance.hasteCapModifier <= -0.6f)
		{
			AchievementManager.instance.UnlockAchievement("MaxHaste");
		}
	}
}
