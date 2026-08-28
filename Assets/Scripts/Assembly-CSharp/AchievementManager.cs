using Steamworks;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
	public static AchievementManager instance;

	private int goldSpentOnResearch;

	private int discoveriesMade;

	private int enemiesKilled;

	[SerializeField]
	private string[] towerUnlockStrings;

	public bool shrineSpawned;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		if (SteamManager.Initialized)
		{
			SteamUserStats.GetStat("EnemiesKilled", out enemiesKilled);
			SteamUserStats.GetStat("DiscoveriesMade", out discoveriesMade);
			SteamUserStats.GetStat("GoldOnResearch", out goldSpentOnResearch);
		}
	}

	public void OnSceneClose()
	{
		if (SteamManager.Initialized)
		{
			SteamUserStats.SetStat("EnemiesKilled", enemiesKilled);
			SteamUserStats.SetStat("DiscoveriesMade", discoveriesMade);
			SteamUserStats.SetStat("GoldOnResearch", goldSpentOnResearch);
			SteamUserStats.StoreStats();
		}
	}

	public void ResetSteamStats()
	{
		SteamUserStats.ResetAllStats(bAchievementsToo: true);
	}

	public void FundResearchAchievement(int amt)
	{
		if (goldSpentOnResearch < 100000 && goldSpentOnResearch + amt >= 100000)
		{
			UnlockAchievement("Funding100000");
		}
		else if (goldSpentOnResearch < 10000 && goldSpentOnResearch + amt >= 10000)
		{
			UnlockAchievement("Funding10000");
		}
		else if (goldSpentOnResearch < 1000 && goldSpentOnResearch + amt >= 1000)
		{
			UnlockAchievement("Funding1000");
		}
		goldSpentOnResearch += amt;
	}

	public void NewDiscoveriesAchievement()
	{
		discoveriesMade++;
		if (discoveriesMade == 100)
		{
			UnlockAchievement("Discover100");
		}
		else if (discoveriesMade == 10)
		{
			UnlockAchievement("Discover10");
		}
		else if (discoveriesMade == 1)
		{
			UnlockAchievement("Discover1");
		}
	}

	public void EnemyKilled()
	{
		enemiesKilled++;
		if (enemiesKilled == 1000000)
		{
			UnlockAchievement("Kill1000000Enemies");
		}
		else if (enemiesKilled == 100000)
		{
			UnlockAchievement("Kill100000Enemies");
		}
		else if (enemiesKilled == 10000)
		{
			UnlockAchievement("Kill10000Enemies");
		}
		else if (enemiesKilled == 1000)
		{
			UnlockAchievement("Kill1000Enemies");
		}
		else if (enemiesKilled == 100)
		{
			UnlockAchievement("Kill100Enemies");
		}
	}

	public void UnlockAchievement(string s)
	{
		if (SteamManager.Initialized)
		{
			Debug.Log("Unlocked Achievement: " + s);
			SteamUserStats.SetAchievement(s);
			SteamUserStats.StoreStats();
		}
	}

	public void CheckTowerUnlocks()
	{
		int num = 0;
		string[] array = towerUnlockStrings;
		foreach (string key in array)
		{
			num += PlayerPrefs.GetInt(key, 0);
		}
		if (num == towerUnlockStrings.Length)
		{
			UnlockAchievement("UnlockAllTowers");
		}
	}

	public void BeatLevel(int level)
	{
		int gameMode = GameManager.instance.gameMode;
		if (level == 15)
		{
			switch (gameMode)
			{
			case 1:
				UnlockAchievement("BeatLevel15Mode1");
				break;
			case 2:
				UnlockAchievement("BeatLevel15Mode2");
				break;
			case 3:
				UnlockAchievement("BeatLevel15Mode3");
				break;
			}
		}
		if (level == 25)
		{
			switch (gameMode)
			{
			case 1:
				UnlockAchievement("BeatLevel25Mode1");
				break;
			case 2:
				UnlockAchievement("BeatLevel25Mode2");
				break;
			case 3:
				UnlockAchievement("BeatLevel25Mode3");
				break;
			}
		}
		if (level == 35)
		{
			switch (gameMode)
			{
			case 1:
				UnlockAchievement("BeatLevel35Mode1");
				break;
			case 2:
				UnlockAchievement("BeatLevel35Mode2");
				break;
			case 3:
				UnlockAchievement("BeatLevel35Mode3");
				break;
			}
			if (!shrineSpawned)
			{
				UnlockAchievement("NoShrines");
			}
		}
		if (level == 45)
		{
			switch (gameMode)
			{
			case 1:
				UnlockAchievement("BeatLevel45Mode1");
				break;
			case 2:
				UnlockAchievement("BeatLevel45Mode2");
				break;
			case 3:
				UnlockAchievement("BeatLevel45Mode3");
				break;
			}
		}
	}

	public void TowerLevel(int level)
	{
		switch (level)
		{
		case 10:
			UnlockAchievement("TowerLevel10");
			break;
		case 20:
			UnlockAchievement("TowerLevel20");
			break;
		case 50:
			UnlockAchievement("TowerLevel50");
			break;
		}
	}

	public void CheckTowerTypesVictory()
	{
		int gameMode = GameManager.instance.gameMode;
		if (TowerManager.instance.towersUsed.Contains(TowerType.Crossbow) && TowerManager.instance.towersUsed.Count <= 1)
		{
			UnlockAchievement("OnlyBallista");
			if (gameMode >= 2)
			{
				UnlockAchievement("OnlyBallista2");
			}
			if (gameMode >= 3)
			{
				UnlockAchievement("OnlyBallista3");
			}
		}
		if (!TowerManager.instance.towersUsed.Contains(TowerType.Crossbow))
		{
			UnlockAchievement("NoBallista");
		}
		if ((TowerManager.instance.towersUsed.Contains(TowerType.Crossbow) && TowerManager.instance.towersUsed.Contains(TowerType.Morter) && TowerManager.instance.towersUsed.Count <= 2) || (TowerManager.instance.towersUsed.Contains(TowerType.Morter) && TowerManager.instance.towersUsed.Count <= 1))
		{
			UnlockAchievement("OnlyBallistaMortar");
			if (gameMode >= 2)
			{
				UnlockAchievement("OnlyBallistaMortar2");
			}
			if (gameMode >= 3)
			{
				UnlockAchievement("OnlyBallistaMortar3");
			}
		}
		if ((TowerManager.instance.towersUsed.Contains(TowerType.Crossbow) && TowerManager.instance.towersUsed.Contains(TowerType.TeslaCoil) && TowerManager.instance.towersUsed.Count <= 2) || (TowerManager.instance.towersUsed.Contains(TowerType.TeslaCoil) && TowerManager.instance.towersUsed.Count <= 1))
		{
			UnlockAchievement("OnlyBallistaTesla");
			if (gameMode >= 2)
			{
				UnlockAchievement("OnlyBallistaTesla2");
			}
			if (gameMode >= 3)
			{
				UnlockAchievement("OnlyBallistaTesla3");
			}
		}
		if ((TowerManager.instance.towersUsed.Contains(TowerType.Crossbow) && TowerManager.instance.towersUsed.Contains(TowerType.Frost) && TowerManager.instance.towersUsed.Count <= 2) || (TowerManager.instance.towersUsed.Contains(TowerType.Frost) && TowerManager.instance.towersUsed.Count <= 1))
		{
			UnlockAchievement("OnlyBallistaFrost");
			if (gameMode >= 2)
			{
				UnlockAchievement("OnlyBallistaFrost2");
			}
			if (gameMode >= 3)
			{
				UnlockAchievement("OnlyBallistaFrost3");
			}
		}
		if ((TowerManager.instance.towersUsed.Contains(TowerType.Crossbow) && TowerManager.instance.towersUsed.Contains(TowerType.FlameThrower) && TowerManager.instance.towersUsed.Count <= 2) || (TowerManager.instance.towersUsed.Contains(TowerType.FlameThrower) && TowerManager.instance.towersUsed.Count <= 1))
		{
			UnlockAchievement("OnlyBallistaFlameThrower");
			if (gameMode >= 2)
			{
				UnlockAchievement("OnlyBallistaFlameThrower2");
			}
			if (gameMode >= 3)
			{
				UnlockAchievement("OnlyBallistaFlameThrower3");
			}
		}
		if ((TowerManager.instance.towersUsed.Contains(TowerType.Crossbow) && TowerManager.instance.towersUsed.Contains(TowerType.PoisonSprayer) && TowerManager.instance.towersUsed.Count <= 2) || (TowerManager.instance.towersUsed.Contains(TowerType.PoisonSprayer) && TowerManager.instance.towersUsed.Count <= 1))
		{
			UnlockAchievement("OnlyBallistaPoisonSprayer");
			if (gameMode >= 2)
			{
				UnlockAchievement("OnlyBallistaPoisonSprayer2");
			}
			if (gameMode >= 3)
			{
				UnlockAchievement("OnlyBallistaPoisonSprayer3");
			}
		}
		if ((TowerManager.instance.towersUsed.Contains(TowerType.Crossbow) && TowerManager.instance.towersUsed.Contains(TowerType.Shredder) && TowerManager.instance.towersUsed.Count <= 2) || (TowerManager.instance.towersUsed.Contains(TowerType.Shredder) && TowerManager.instance.towersUsed.Count <= 1))
		{
			UnlockAchievement("OnlyBallistaShredder");
			if (gameMode >= 2)
			{
				UnlockAchievement("OnlyBallistaShredder2");
			}
			if (gameMode >= 3)
			{
				UnlockAchievement("OnlyBallistaShredder3");
			}
		}
		if ((TowerManager.instance.towersUsed.Contains(TowerType.Crossbow) && TowerManager.instance.towersUsed.Contains(TowerType.Encampment) && TowerManager.instance.towersUsed.Count <= 2) || (TowerManager.instance.towersUsed.Contains(TowerType.Encampment) && TowerManager.instance.towersUsed.Count <= 1))
		{
			UnlockAchievement("OnlyBallistaEncampment");
			if (gameMode >= 2)
			{
				UnlockAchievement("OnlyBallistaEncampment2");
			}
			if (gameMode >= 3)
			{
				UnlockAchievement("OnlyBallistaEncampment3");
			}
		}
		if ((TowerManager.instance.towersUsed.Contains(TowerType.Crossbow) && TowerManager.instance.towersUsed.Contains(TowerType.Lookout) && TowerManager.instance.towersUsed.Count <= 2) || (TowerManager.instance.towersUsed.Contains(TowerType.Lookout) && TowerManager.instance.towersUsed.Count <= 1))
		{
			UnlockAchievement("OnlyBallistaLookout");
			if (gameMode >= 2)
			{
				UnlockAchievement("OnlyBallistaLookout2");
			}
			if (gameMode >= 3)
			{
				UnlockAchievement("OnlyBallistaLookout3");
			}
		}
		if ((TowerManager.instance.towersUsed.Contains(TowerType.Crossbow) && TowerManager.instance.towersUsed.Contains(TowerType.Radar) && TowerManager.instance.towersUsed.Count <= 2) || (TowerManager.instance.towersUsed.Contains(TowerType.Radar) && TowerManager.instance.towersUsed.Count <= 1))
		{
			UnlockAchievement("OnlyBallistaRadar");
			if (gameMode >= 2)
			{
				UnlockAchievement("OnlyBallistaRadar2");
			}
			if (gameMode >= 3)
			{
				UnlockAchievement("OnlyBallistaRadar3");
			}
		}
		if ((TowerManager.instance.towersUsed.Contains(TowerType.Crossbow) && TowerManager.instance.towersUsed.Contains(TowerType.Obelisk) && TowerManager.instance.towersUsed.Count <= 2) || (TowerManager.instance.towersUsed.Contains(TowerType.Obelisk) && TowerManager.instance.towersUsed.Count <= 1))
		{
			UnlockAchievement("OnlyBallistaObelisk");
			if (gameMode >= 2)
			{
				UnlockAchievement("OnlyBallistaObelisk2");
			}
			if (gameMode >= 3)
			{
				UnlockAchievement("OnlyBallistaObelisk3");
			}
		}
		if ((TowerManager.instance.towersUsed.Contains(TowerType.Crossbow) && TowerManager.instance.towersUsed.Contains(TowerType.ParticleCannon) && TowerManager.instance.towersUsed.Count <= 2) || (TowerManager.instance.towersUsed.Contains(TowerType.ParticleCannon) && TowerManager.instance.towersUsed.Count <= 1))
		{
			UnlockAchievement("OnlyBallistaParticleCannon");
			if (gameMode >= 2)
			{
				UnlockAchievement("OnlyBallistaParticleCannon2");
			}
			if (gameMode >= 3)
			{
				UnlockAchievement("OnlyBallistaParticleCannon3");
			}
		}
	}

	public bool CheckAllTowers()
	{
		if (!TowerManager.instance.towersUsed.Contains(TowerType.Crossbow))
		{
			return false;
		}
		if (!TowerManager.instance.towersUsed.Contains(TowerType.Morter))
		{
			return false;
		}
		if (!TowerManager.instance.towersUsed.Contains(TowerType.TeslaCoil))
		{
			return false;
		}
		if (!TowerManager.instance.towersUsed.Contains(TowerType.Frost))
		{
			return false;
		}
		if (!TowerManager.instance.towersUsed.Contains(TowerType.FlameThrower))
		{
			return false;
		}
		if (!TowerManager.instance.towersUsed.Contains(TowerType.PoisonSprayer))
		{
			return false;
		}
		if (!TowerManager.instance.towersUsed.Contains(TowerType.Shredder))
		{
			return false;
		}
		if (!TowerManager.instance.towersUsed.Contains(TowerType.Encampment))
		{
			return false;
		}
		if (!TowerManager.instance.towersUsed.Contains(TowerType.Lookout))
		{
			return false;
		}
		if (!TowerManager.instance.towersUsed.Contains(TowerType.Radar))
		{
			return false;
		}
		if (!TowerManager.instance.towersUsed.Contains(TowerType.Obelisk))
		{
			return false;
		}
		if (!TowerManager.instance.towersUsed.Contains(TowerType.ParticleCannon))
		{
			return false;
		}
		UnlockAchievement("UsedAllTowers");
		return true;
	}
}
