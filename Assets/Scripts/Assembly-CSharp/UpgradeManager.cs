using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
	public static UpgradeManager instance;

	public int xp;

	public int prestige;

	[SerializeField]
	private Text xpText;

	[SerializeField]
	private GameObject prestigeDisplay;

	[SerializeField]
	private Text prestigeText;

	public int unlockedCardCount;

	[SerializeField]
	private UpgradeButton[] allButtons;

	[SerializeField]
	private List<UpgradeButton> cardsWhichRequireCardCount = new List<UpgradeButton>();

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		allButtons = Object.FindObjectsOfType<UpgradeButton>();
		UpgradeButton[] array = allButtons;
		foreach (UpgradeButton upgradeButton in array)
		{
			if (upgradeButton.cardCountRequirement > 0)
			{
				cardsWhichRequireCardCount.Add(upgradeButton);
			}
		}
		unlockedCardCount = PlayerPrefs.GetInt("UnlockedCardCount", 0);
		xp = PlayerPrefs.GetInt("XP", 0);
		xpText.text = "XP: " + xp;
		prestige = PlayerPrefs.GetInt("Prestige", 0);
		if (prestige > 0)
		{
			prestigeDisplay.SetActive(value: true);
			prestigeText.text = "Prestige: " + prestige;
		}
	}

	public void AddXP(int change)
	{
		xp += change;
		PlayerPrefs.SetInt("XP", xp);
		xpText.text = "XP: " + xp;
	}

	public void ResetUpgrades()
	{
		int num = 0;
		UpgradeButton[] array = allButtons;
		foreach (UpgradeButton upgradeButton in array)
		{
			if (upgradeButton.enabled)
			{
				if (upgradeButton.unlocked)
				{
					num += upgradeButton.xpCost;
				}
				upgradeButton.ResetUnlock();
			}
		}
		num += xp / 2;
		Debug.Log("Refunded xp: " + num);
		PlayerPrefs.SetInt("Prestige", prestige + num / 1000);
		xp = 0;
		PlayerPrefs.SetInt("XP", 0);
		PlayerPrefs.SetInt("UnlockedCardCount", 0);
		PlayerPrefs.SetInt("Development", 0);
		PlayerPrefs.SetInt("Record1", 0);
		PlayerPrefs.SetInt("Record2", 0);
		PlayerPrefs.SetInt("Record3", 0);
		LevelLoader.instance.LoadLevel("MainMenu");
	}

	public void CountCard(bool countsAsCardUnlock)
	{
		if (countsAsCardUnlock)
		{
			unlockedCardCount++;
			Debug.Log("Number of unlocked cards: " + unlockedCardCount);
			PlayerPrefs.SetInt("UnlockedCardCount", unlockedCardCount);
			CheckCardCount();
		}
	}

	public void CountDevelopment(bool value)
	{
		if (value)
		{
			int num = PlayerPrefs.GetInt("Development", 0);
			PlayerPrefs.SetInt("Development", num + 1);
			Debug.Log("Development: " + (num + 1));
		}
	}

	public void CheckCardCount()
	{
		foreach (UpgradeButton item in cardsWhichRequireCardCount)
		{
			item.CheckEnabled();
		}
	}
}
