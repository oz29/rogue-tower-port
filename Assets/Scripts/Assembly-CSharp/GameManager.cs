using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private TileManager tileManager;

	[SerializeField]
	private SpawnManager spawnManager;

	public int gameMode = 3;

	[SerializeField]
	private TerrainTile singleStartTile;

	[SerializeField]
	private Waypoint[] singleStartWaypoints;

	[SerializeField]
	private TerrainTile doubleStartTile;

	[SerializeField]
	private Waypoint[] doubleStartWaypoints;

	[SerializeField]
	private TerrainTile tripleStartTile;

	[SerializeField]
	private Waypoint[] tripleStartWaypoints;

	public bool gameOver;

	public int health;

	public int maxHealth;

	private float healthLoss;

	[SerializeField]
	private Text healthText;

	[SerializeField]
	private Image maskBar;

	[SerializeField]
	private Image healthBar;

	[SerializeField]
	private Image healthLossBar;

	[SerializeField]
	private GameObject gameOverMenu;

	[SerializeField]
	private Text levelsCompletedText;

	[SerializeField]
	private Text xpGainText;

	[SerializeField]
	private Text newRecordText;

	[SerializeField]
	private GameObject victoryScreen;

	[SerializeField]
	private Text vicLevelsText;

	[SerializeField]
	private Text vicLevelXpText;

	[SerializeField]
	private Text vicXpBonusText;

	[SerializeField]
	private Text vicNewRecordText;

	public Vector3 dotTick = 24f * Vector3.one;

	public int hauntedHouseEfficiency = 1;

	public int universityBonus;

	public static GameManager instance;

	private void Awake()
	{
		instance = this;
		Application.targetFrameRate = 60;
		QualitySettings.vSyncCount = 1;
		if (Object.FindObjectOfType<MobileHUD>() == null)
		{
			base.gameObject.AddComponent<MobileHUD>();
		}
		if (Object.FindObjectOfType<RunSaveManager>() == null)
		{
			base.gameObject.AddComponent<RunSaveManager>();
		}
		if (Object.FindObjectOfType<TerraformManager>() == null)
		{
			base.gameObject.AddComponent<TerraformManager>();
		}
		if (RunSaveManager.loadSavedRunOnStart && RunSaveManager.HasSavedRun())
		{
			RunSaveData savedHeader = RunSaveManager.GetSavedRunHeader();
			if (savedHeader != null)
			{
				gameMode = savedHeader.gameMode;
				PlayerPrefs.SetInt("GameMode", gameMode);
			}
		}
		else
		{
			gameMode = PlayerPrefs.GetInt("GameMode", 1);
		}

		if (gameMode == 1)
		{
			if (RunSaveManager.loadSavedRunOnStart && RunSaveManager.HasSavedRun())
			{
				RunSaveData d = RunSaveManager.GetSavedRunHeader();
				if (d != null) singleStartTile.transform.eulerAngles = new Vector3(0f, d.startTileRotationY, 0f);
			}
			else
			{
				singleStartTile.transform.Rotate(0f, 90 * Random.Range(-1, 3), 0f);
			}
			singleStartTile.SetCardinalDirections();
			tileManager.startTile = singleStartTile;
			spawnManager.initialSpawns = singleStartWaypoints;
			doubleStartTile.gameObject.SetActive(value: false);
			tripleStartTile.gameObject.SetActive(value: false);
		}
		else if (gameMode == 2)
		{
			if (RunSaveManager.loadSavedRunOnStart && RunSaveManager.HasSavedRun())
			{
				RunSaveData d = RunSaveManager.GetSavedRunHeader();
				if (d != null) doubleStartTile.transform.eulerAngles = new Vector3(0f, d.startTileRotationY, 0f);
			}
			else
			{
				doubleStartTile.transform.Rotate(0f, 90 * Random.Range(-1, 3), 0f);
			}
			doubleStartTile.SetCardinalDirections();
			tileManager.startTile = doubleStartTile;
			spawnManager.initialSpawns = doubleStartWaypoints;
			singleStartTile.gameObject.SetActive(value: false);
			tripleStartTile.gameObject.SetActive(value: false);
		}
		else if (gameMode == 3)
		{
			if (RunSaveManager.loadSavedRunOnStart && RunSaveManager.HasSavedRun())
			{
				RunSaveData d = RunSaveManager.GetSavedRunHeader();
				if (d != null) tripleStartTile.transform.eulerAngles = new Vector3(0f, d.startTileRotationY, 0f);
			}
			else
			{
				tripleStartTile.transform.Rotate(0f, 90 * Random.Range(-1, 3), 0f);
			}
			tripleStartTile.SetCardinalDirections();
			tileManager.startTile = tripleStartTile;
			spawnManager.initialSpawns = tripleStartWaypoints;
			doubleStartTile.gameObject.SetActive(value: false);
			singleStartTile.gameObject.SetActive(value: false);
		}
	}

	private void Start()
	{
		maxHealth = 100 + 10 * (PlayerPrefs.GetInt("TowerHealth1", 0) + PlayerPrefs.GetInt("TowerHealth2", 0) + PlayerPrefs.GetInt("TowerHealth3", 0) + PlayerPrefs.GetInt("TowerHealth4", 0) + PlayerPrefs.GetInt("TowerHealth5", 0) + PlayerPrefs.GetInt("TowerHealth6", 0) + PlayerPrefs.GetInt("TowerHealth7", 0) + PlayerPrefs.GetInt("TowerHealth8", 0) + PlayerPrefs.GetInt("TowerHealth9", 0) + PlayerPrefs.GetInt("TowerHealth10", 0));
		health = maxHealth;
		healthLoss = maxHealth;
		SetHealthBar();
		int num = 0;
		num = PlayerPrefs.GetInt("BalistaPHealth", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusHealthDamage(TowerType.Crossbow, num);
		}
		num = PlayerPrefs.GetInt("BalistaPArmor", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusArmorDamage(TowerType.Crossbow, num);
		}
		num = PlayerPrefs.GetInt("BalistaPShield", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusShieldDamage(TowerType.Crossbow, num);
		}
		num = PlayerPrefs.GetInt("MortarPHealth", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusHealthDamage(TowerType.Morter, num);
		}
		num = PlayerPrefs.GetInt("MortarPArmor", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusArmorDamage(TowerType.Morter, num);
		}
		num = PlayerPrefs.GetInt("MortarPShield", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusShieldDamage(TowerType.Morter, num);
		}
		num = PlayerPrefs.GetInt("TeslaPHealth", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusHealthDamage(TowerType.TeslaCoil, num);
		}
		num = PlayerPrefs.GetInt("TeslaPArmor", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusArmorDamage(TowerType.TeslaCoil, num);
		}
		num = PlayerPrefs.GetInt("TeslaPShield", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusShieldDamage(TowerType.TeslaCoil, num);
		}
		num = PlayerPrefs.GetInt("FrostPHealth", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusHealthDamage(TowerType.Frost, num);
		}
		num = PlayerPrefs.GetInt("FrostPArmor", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusArmorDamage(TowerType.Frost, num);
		}
		num = PlayerPrefs.GetInt("FrostPShield", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusShieldDamage(TowerType.Frost, num);
		}
		num = PlayerPrefs.GetInt("FlamePHealth", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusHealthDamage(TowerType.FlameThrower, num);
		}
		num = PlayerPrefs.GetInt("FlamePArmor", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusArmorDamage(TowerType.FlameThrower, num);
		}
		num = PlayerPrefs.GetInt("FlamePShield", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusShieldDamage(TowerType.FlameThrower, num);
		}
		num = PlayerPrefs.GetInt("PoisonPHealth", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusHealthDamage(TowerType.PoisonSprayer, num);
		}
		num = PlayerPrefs.GetInt("PoisonPArmor", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusArmorDamage(TowerType.PoisonSprayer, num);
		}
		num = PlayerPrefs.GetInt("PoisonPShield", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusShieldDamage(TowerType.PoisonSprayer, num);
		}
		num = PlayerPrefs.GetInt("ShredderPHealth", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusHealthDamage(TowerType.Shredder, num);
		}
		num = PlayerPrefs.GetInt("ShredderPArmor", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusArmorDamage(TowerType.Shredder, num);
		}
		num = PlayerPrefs.GetInt("ShredderPShield", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusShieldDamage(TowerType.Shredder, num);
		}
		num = PlayerPrefs.GetInt("EncampmentPHealth", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusHealthDamage(TowerType.Encampment, num);
		}
		num = PlayerPrefs.GetInt("EncampmentPArmor", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusArmorDamage(TowerType.Encampment, num);
		}
		num = PlayerPrefs.GetInt("EncampmentPShield", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusShieldDamage(TowerType.Encampment, num);
		}
		num = PlayerPrefs.GetInt("LookoutPHealth", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusHealthDamage(TowerType.Lookout, num);
		}
		num = PlayerPrefs.GetInt("LookoutPArmor", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusArmorDamage(TowerType.Lookout, num);
		}
		num = PlayerPrefs.GetInt("LookoutPShield", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusShieldDamage(TowerType.Lookout, num);
		}
		num = PlayerPrefs.GetInt("RadarPHealth", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusHealthDamage(TowerType.Radar, num);
		}
		num = PlayerPrefs.GetInt("RadarPArmor", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusArmorDamage(TowerType.Radar, num);
		}
		num = PlayerPrefs.GetInt("RadarPShield", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusShieldDamage(TowerType.Radar, num);
		}
		num = PlayerPrefs.GetInt("ObeliskPHealth", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusHealthDamage(TowerType.Obelisk, num);
		}
		num = PlayerPrefs.GetInt("ObeliskPArmor", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusArmorDamage(TowerType.Obelisk, num);
		}
		num = PlayerPrefs.GetInt("ObeliskPShield", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusShieldDamage(TowerType.Obelisk, num);
		}
		num = PlayerPrefs.GetInt("ParticleCannonPHealth", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusHealthDamage(TowerType.ParticleCannon, num);
		}
		num = PlayerPrefs.GetInt("ParticleCannonPArmor", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusArmorDamage(TowerType.ParticleCannon, num);
		}
		num = PlayerPrefs.GetInt("ParticleCannonPShield", 0);
		if (num > 0)
		{
			TowerManager.instance.AddBonusShieldDamage(TowerType.ParticleCannon, num);
		}
	}

	private void Update()
	{
		if (healthLoss > (float)health)
		{
			healthLoss = Mathf.Clamp(healthLoss - 4f * Time.deltaTime, health, healthLoss);
			healthLossBar.rectTransform.sizeDelta = new Vector2(healthLoss / 10f, 0.25f);
		}
	}

	public void IncreaseTowerHealth(int amt)
	{
		maxHealth += amt * 10;
		health += amt * 10;
		healthLoss = health;
		SetHealthBar();
		if (maxHealth / 10 >= 50)
		{
			AchievementManager.instance.UnlockAchievement("TowerHealth50");
		}
		else if (maxHealth / 10 >= 40)
		{
			AchievementManager.instance.UnlockAchievement("TowerHealth40");
		}
		else if (maxHealth / 10 >= 30)
		{
			AchievementManager.instance.UnlockAchievement("TowerHealth30");
		}
		if (RunSaveManager.instance != null)
		{
			RunSaveManager.instance.TryRestoreRun();
		}
	}

	public void UpdateHealthBar()
	{
		SetHealthBar();
	}

	private void SetHealthBar()
	{
		maskBar.rectTransform.localScale = new Vector3(2000 / maxHealth, 50f, 10f);
		maskBar.rectTransform.sizeDelta = new Vector2((float)maxHealth / 10f, 0.25f);
		healthLossBar.rectTransform.sizeDelta = new Vector2((float)health / 10f, 0.25f);
		UpdateHealthText();
	}

	private void UpdateHealthText()
	{
		healthText.text = "Tower: " + health / 10 + "/" + maxHealth / 10;
		healthBar.rectTransform.sizeDelta = new Vector2((float)health / 10f, 0.25f);
	}

	public void TakeDamage(int damage)
	{
		health = Mathf.Max(health - 10 * damage, 0);
		UpdateHealthText();
		if (health <= 0 && !gameOver)
		{
			gameOver = true;
			GameOver();
		}
	}

	public void RepairTower(int heal)
	{
		health = Mathf.Clamp(health + heal, 0, maxHealth);
		if (healthLoss < (float)health)
		{
			healthLoss = health;
		}
		UpdateHealthText();
	}

	public void GameOverQuit()
	{
		if (!SpawnManager.instance.combat)
		{
			SpawnManager.instance.level++;
		}
		if (Time.timeScale != 1f)
		{
			Time.timeScale = 1f;
		}
		gameOver = true;
		GameOver();
	}

	public int NaturalSum(int value)
	{
		return value * (value + 1) / 2;
	}

	private void GameOver()
	{
		RunSaveManager.DeleteSavedRun();
		gameOverMenu.SetActive(value: true);
		DamageTracker.instance.DisplayDamageTotals();
		int num = NaturalSum(SpawnManager.instance.level - 1) * gameMode;
		int num2 = 0;
		int num3 = PlayerPrefs.GetInt("Record" + gameMode, 0);
		if (SpawnManager.instance.level - 1 - num3 > 0)
		{
			num2 = (NaturalSum(SpawnManager.instance.level - 1) - NaturalSum(num3)) * 3;
			PlayerPrefs.SetInt("Record" + gameMode, SpawnManager.instance.level - 1);
			string text = "";
			if (gameMode == 1)
			{
				text = "single";
			}
			else if (gameMode == 2)
			{
				text = "double";
			}
			else if (gameMode == 3)
			{
				text = "triple";
			}
			newRecordText.text = "New " + text + " defense record!\n +" + num2 + " bonus xp";
		}
		else
		{
			newRecordText.text = "";
		}
		int num4 = PlayerPrefs.GetInt("XP", 0);
		PlayerPrefs.SetInt("XP", num4 + num2);
		levelsCompletedText.text = "Defended " + (SpawnManager.instance.level - 1) + " levels";
		xpGainText.text = "+" + num + " xp";
	}

	public void Victory()
	{
		RunSaveManager.DeleteSavedRun();
		AchievementManager.instance.CheckTowerTypesVictory();
		victoryScreen.SetActive(value: true);
		DamageTracker.instance.DisplayDamageTotals();
		int num = NaturalSum(SpawnManager.instance.level - 1) * gameMode;
		int num2 = SpawnManager.instance.lastLevel * gameMode * 10;
		int num3 = 0;
		int num4 = PlayerPrefs.GetInt("Record" + gameMode, 0);
		if (SpawnManager.instance.lastLevel - num4 > 0)
		{
			num3 = (NaturalSum(SpawnManager.instance.level - 1) - NaturalSum(num4)) * 3;
			PlayerPrefs.SetInt("Record" + gameMode, SpawnManager.instance.lastLevel);
			string text = "";
			if (gameMode == 1)
			{
				text = "single";
			}
			else if (gameMode == 2)
			{
				text = "double";
			}
			else if (gameMode == 3)
			{
				text = "triple";
			}
			vicNewRecordText.text = "New " + text + " defense record!\n +" + num3 + " bonus xp";
		}
		else
		{
			vicNewRecordText.text = "";
		}
		int num5 = PlayerPrefs.GetInt("XP", 0);
		PlayerPrefs.SetInt("XP", num5 + num3 + num2);
		vicLevelsText.text = "Defended all " + SpawnManager.instance.lastLevel + " levels";
		vicLevelXpText.text = "+" + num + " xp";
		vicXpBonusText.text = "+" + num2 + " xp";
	}

	public void LoadScene(string sceneName)
	{
		if (AchievementManager.instance != null)
		{
			AchievementManager.instance.OnSceneClose();
		}
		LevelLoader.instance.LoadLevel(sceneName);
	}
}
