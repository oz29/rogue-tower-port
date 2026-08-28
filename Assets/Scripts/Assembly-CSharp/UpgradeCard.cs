using System.Collections.Generic;
using UnityEngine;

public class UpgradeCard : MonoBehaviour
{
	[SerializeField]
	private string unlockName;

	[SerializeField]
	private bool unlockedByDefault;

	public bool unlocked;

	public string title;

	public Sprite image;

	public string description;

	public List<UpgradeCard> unlocks = new List<UpgradeCard>();

	private void Awake()
	{
		if (unlockedByDefault || PlayerPrefs.GetInt(unlockName, 0) == 1)
		{
			unlocked = true;
		}
		else
		{
			unlocked = false;
		}
	}

	public virtual void Upgrade()
	{
	}
}
