using UnityEngine;

public class ManaGenUpgradeCard : UpgradeCard
{
	[SerializeField]
	private float manaPercentRegenIncrease;

	[SerializeField]
	private int manaBankMaxManaIncrease;

	public override void Upgrade()
	{
		base.Upgrade();
		ResourceManager.instance.AddPercentManaGathering(manaPercentRegenIncrease);
		ResourceManager.instance.UpdateManaHUD();
		if (manaBankMaxManaIncrease != 0)
		{
			ResourceManager.instance.UpgradeManaBankMaxMana(manaBankMaxManaIncrease);
		}
	}
}
