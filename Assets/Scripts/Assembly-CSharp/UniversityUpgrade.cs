using UnityEngine;

public class UniversityUpgrade : UpgradeCard
{
	[SerializeField]
	private int bonus = 1;

	public override void Upgrade()
	{
		base.Upgrade();
		GameManager.instance.universityBonus += bonus;
	}
}
