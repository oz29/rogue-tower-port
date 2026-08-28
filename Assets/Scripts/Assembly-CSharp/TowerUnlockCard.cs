using UnityEngine;

public class TowerUnlockCard : UpgradeCard
{
	[SerializeField]
	private GameObject towerUIObject;

	[SerializeField]
	private bool isTower = true;

	public override void Upgrade()
	{
		base.Upgrade();
		TowerUnlockManager.instance.UnlockTower(towerUIObject, isTower);
	}
}
