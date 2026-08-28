using UnityEngine;

public class GoldRushCard : UpgradeCard
{
	[SerializeField]
	private int goldGain;

	[SerializeField]
	private float speedBonus;

	public override void Upgrade()
	{
		base.Upgrade();
		MonsterManager.instance.speedBonus += speedBonus;
		ResourceManager.instance.AddMoney(goldGain);
	}
}
