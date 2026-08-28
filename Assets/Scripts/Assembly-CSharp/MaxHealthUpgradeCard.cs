using UnityEngine;

public class MaxHealthUpgradeCard : UpgradeCard
{
	[SerializeField]
	private int healthIncrease;

	public override void Upgrade()
	{
		base.Upgrade();
		GameManager.instance.IncreaseTowerHealth(healthIncrease);
	}
}
