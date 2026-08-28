public class HauntedHouseUpgrade : UpgradeCard
{
	public override void Upgrade()
	{
		base.Upgrade();
		GameManager.instance.hauntedHouseEfficiency++;
	}
}
