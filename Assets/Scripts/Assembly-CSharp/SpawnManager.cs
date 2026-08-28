using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
	public static SpawnManager instance;

	public Waypoint[] initialSpawns;

	private HashSet<Waypoint> spawnPoints = new HashSet<Waypoint>();

	public HashSet<IncomeGenerator> incomeGenerators = new HashSet<IncomeGenerator>();

	public HashSet<University> universities = new HashSet<University>();

	public HashSet<Mine> mines = new HashSet<Mine>();

	public HashSet<House> houses = new HashSet<House>();

	public HashSet<GameObject> destroyOnNewLevel = new HashSet<GameObject>();

	public HashSet<GameObject> tileSpawnUis = new HashSet<GameObject>();

	public HashSet<Enemy> currentEnemies = new HashSet<Enemy>();

	public bool combat;

	private float levelTime;

	[SerializeField]
	private Text levelText;

	[SerializeField]
	private Text scoreText;

	[SerializeField]
	private int cardDrawFrequency = 5;

	[SerializeField]
	private float baseRepairChance;

	public int level;

	public int lastLevel = 30;

	[SerializeField]
	private GameObject level1Enemy;

	[SerializeField]
	private GameObject level3Enemy;

	[SerializeField]
	private GameObject level5Enemy;

	[SerializeField]
	private GameObject level7Enemy;

	[SerializeField]
	private GameObject level9Enemy;

	[SerializeField]
	private GameObject level12Enemy;

	[SerializeField]
	private GameObject level16Enemy;

	[SerializeField]
	private GameObject level18Enemy;

	[SerializeField]
	private GameObject level20Enemy;

	[SerializeField]
	private GameObject level21Enemy;

	[SerializeField]
	private GameObject level23Enemy;

	[SerializeField]
	private GameObject level26Enemy;

	[SerializeField]
	private GameObject level28Enemy;

	[SerializeField]
	private GameObject level30Enemy;

	[SerializeField]
	private GameObject level32Enemy;

	[SerializeField]
	private GameObject level36Enemy;

	[SerializeField]
	private GameObject level38Enemy;

	[SerializeField]
	private GameObject level40Enemy;

	[SerializeField]
	private GameObject level15Boss;

	[SerializeField]
	private GameObject level25Boss;

	[SerializeField]
	private GameObject level35Boss;

	[SerializeField]
	private GameObject level45Boss;

	[SerializeField]
	private LightManager lightManager;

	[SerializeField]
	private int secondStageLightLevel;

	[SerializeField]
	private int thirdStageLightLevel;

	[SerializeField]
	private int fourthStageLightLevel;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		Waypoint[] array = initialSpawns;
		foreach (Waypoint item in array)
		{
			spawnPoints.Add(item);
		}
		levelText.text = "Level: " + level;
		scoreText.text = "Score: " + 0;
		cardDrawFrequency = 3 - PlayerPrefs.GetInt("CardFreq1", 0) - PlayerPrefs.GetInt("CardFreq2", 0);
		baseRepairChance = 34f * (float)(PlayerPrefs.GetInt("TowerRepair1", 0) + PlayerPrefs.GetInt("TowerRepair2", 0) + PlayerPrefs.GetInt("TowerRepair3", 0));
	}

	private void Update()
	{
		if (combat && currentEnemies.Count == 0 && !GameManager.instance.gameOver && levelTime > 2f)
		{
			EndWave();
		}
		else
		{
			levelTime += Time.deltaTime;
		}
	}

	private void EndWave()
	{
		combat = false;
		AchievementManager.instance.BeatLevel(level);
		int num = PlayerPrefs.GetInt("XP", 0);
		PlayerPrefs.SetInt("XP", num + level * GameManager.instance.gameMode);
		scoreText.text = "Score: " + GameManager.instance.NaturalSum(level) * GameManager.instance.gameMode;
		RunSaveManager.SaveCurrentRun();
		if (level >= lastLevel)
		{
			GameManager.instance.Victory();
			return;
		}
		ShowSpawnUIs(state: true);
		if (level % cardDrawFrequency == 0)
		{
			ShowSpawnUIs(state: false);
			CardManager.instance.DrawCards();
		}
		if (level == 15 || level == 25 || level == 35)
		{
			MusicManager.instance.FadeOut(6f);
		}
		else
		{
			MusicManager.instance.SetIntensity(action: false);
		}
		GameObject[] array = new GameObject[destroyOnNewLevel.Count];
		destroyOnNewLevel.CopyTo(array);
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			destroyOnNewLevel.Remove(gameObject);
			Object.Destroy(gameObject);
		}
		if (tileSpawnUis.Count == 0)
		{
			Debug.Log("I GUESS ILL JUST DIE THEN");
			AchievementManager.instance.UnlockAchievement("NoMorePaths");
			StartNextWave();
		}
	}

	public void StartNextWave()
	{
		levelTime = 0f;
		UpdateSpawnPoints();
		float num = 1f - (float)(GameManager.instance.health / GameManager.instance.maxHealth);
		if (Random.Range(1f, 100f) <= baseRepairChance * num)
		{
			GameManager.instance.RepairTower(10);
		}
		level++;
		levelText.text = "Level: " + level;
		if (level == secondStageLightLevel)
		{
			lightManager.ChangeColor(2);
		}
		else if (level == thirdStageLightLevel)
		{
			lightManager.ChangeColor(3);
		}
		else if (level == fourthStageLightLevel)
		{
			lightManager.ChangeColor(4);
		}
		if (level == 15 || level == 25 || level == 35 || level == 45)
		{
			SpawnBoss(level);
		}
		if (level == 16)
		{
			MusicManager.instance.PlaySong(1, action: true);
		}
		else if (level == 26)
		{
			MusicManager.instance.PlaySong(2, action: true);
		}
		else if (level == 36)
		{
			MusicManager.instance.PlaySong(3, action: true);
		}
		else if (level > 1)
		{
			MusicManager.instance.SetIntensity(action: true);
		}
		StartCoroutine(Spawn(level * level));
		combat = true;
		ShowSpawnUIs(state: false);
		foreach (House house in houses)
		{
			if (house != null)
			{
				house.CheckTowers();
			}
		}
		foreach (IncomeGenerator incomeGenerator in incomeGenerators)
		{
			if (incomeGenerator != null)
			{
				incomeGenerator.GenerateIncome();
			}
		}
		foreach (University university in universities)
		{
			if (university != null)
			{
				university.Research();
			}
		}
		foreach (Mine mine in mines)
		{
			if (mine != null)
			{
				mine.Repair();
			}
		}
	}

	public void ShowSpawnUIs(bool state)
	{
		foreach (GameObject tileSpawnUi in tileSpawnUis)
		{
			tileSpawnUi.SetActive(state);
		}
	}

	private void SpawnBoss(int lvl)
	{
		Waypoint spawnLocation = null;
		float num = -1f;
		foreach (Waypoint spawnPoint in spawnPoints)
		{
			if (spawnPoint.distanceFromEnd > num)
			{
				num = spawnPoint.distanceFromEnd;
				spawnLocation = spawnPoint;
			}
		}
		switch (lvl)
		{
		case 15:
			SpawnEnemy(level15Boss, spawnLocation);
			break;
		case 25:
			SpawnEnemy(level25Boss, spawnLocation);
			break;
		case 35:
			SpawnEnemy(level35Boss, spawnLocation);
			break;
		case 45:
			SpawnEnemy(level45Boss, spawnLocation);
			break;
		}
	}

	private IEnumerator Spawn(int num)
	{
		int safetyCount = level * level + 100;
		int count = num;
		float t = 0.5f / (float)spawnPoints.Count;
		while (count > 0)
		{
			foreach (Waypoint spawnPoint in spawnPoints)
			{
				if (count <= 0)
				{
					break;
				}
				count -= SpawnSelection(spawnPoint);
				yield return new WaitForSeconds(t);
			}
			safetyCount--;
			if (safetyCount <= 0)
			{
				Debug.LogError("Spawn loop might be looping infinitely");
				break;
			}
		}
	}

	private int SpawnSelection(Waypoint spawnPoint)
	{
		if (level >= 40 && Random.Range(1f, 100f) < 10f)
		{
			SpawnEnemy(level40Enemy, spawnPoint);
			return 10;
		}
		if (level >= 38 && Random.Range(1f, 100f) < 10f)
		{
			SpawnEnemy(level38Enemy, spawnPoint);
			return 10;
		}
		if (level >= 36 && Random.Range(1f, 100f) < 12f)
		{
			SpawnEnemy(level36Enemy, spawnPoint);
			return 9;
		}
		if (level >= 32 && Random.Range(1f, 100f) < 10f)
		{
			SpawnEnemy(level32Enemy, spawnPoint);
			return 10;
		}
		if (level >= 30 && Random.Range(1f, 100f) < 10f)
		{
			SpawnEnemy(level30Enemy, spawnPoint);
			return 10;
		}
		if (level >= 28 && Random.Range(1f, 100f) < 11f)
		{
			SpawnEnemy(level28Enemy, spawnPoint);
			return 10;
		}
		if (level >= 26 && Random.Range(1f, 100f) < 12f)
		{
			SpawnEnemy(level26Enemy, spawnPoint);
			return 7;
		}
		if (level >= 23 && Random.Range(1f, 100f) < 9f)
		{
			SpawnEnemy(level23Enemy, spawnPoint);
			return 12;
		}
		if (level >= 21 && Random.Range(1f, 100f) < 10f)
		{
			SpawnEnemy(level21Enemy, spawnPoint);
			return 10;
		}
		if (level >= 20 && Random.Range(1f, 100f) < 10f)
		{
			SpawnEnemy(level20Enemy, spawnPoint);
			return 10;
		}
		if (level >= 18 && Random.Range(1f, 100f) < 12f)
		{
			SpawnEnemy(level18Enemy, spawnPoint);
			return 9;
		}
		if (level >= 16 && Random.Range(1f, 100f) < 13f)
		{
			SpawnEnemy(level16Enemy, spawnPoint);
			return 8;
		}
		if (level >= 12 && Random.Range(1f, 100f) < 9f)
		{
			SpawnEnemy(level12Enemy, spawnPoint);
			return 12;
		}
		if (level >= 9 && Random.Range(1f, 100f) < 12f)
		{
			SpawnEnemy(level9Enemy, spawnPoint);
			return 9;
		}
		if (level >= 7 && Random.Range(1f, 100f) < 14f)
		{
			SpawnEnemy(level7Enemy, spawnPoint);
			return 7;
		}
		if (level >= 5 && Random.Range(1f, 100f) < 20f)
		{
			SpawnEnemy(level5Enemy, spawnPoint);
			return 5;
		}
		if (level >= 3 && Random.Range(1f, 100f) < 34f)
		{
			SpawnEnemy(level3Enemy, spawnPoint);
			return 3;
		}
		SpawnEnemy(level1Enemy, spawnPoint);
		return 1;
	}

	private void SpawnEnemy(GameObject unit, Waypoint spawnLocation)
	{
		Enemy component = Object.Instantiate(unit, spawnLocation.transform.position, Quaternion.identity).GetComponent<Enemy>();
		component.SetStats();
		component.SetFirstSpawnPoint(spawnLocation);
		currentEnemies.Add(component);
	}

	private void UpdateSpawnPoints()
	{
		List<Waypoint> list = new List<Waypoint>();
		List<Waypoint> list2 = new List<Waypoint>();
		foreach (Waypoint spawnPoint in spawnPoints)
		{
			if (!CheckSpawnPoint(spawnPoint))
			{
				continue;
			}
			list.Add(spawnPoint);
			foreach (Waypoint newSpawnPoint in GetNewSpawnPoints(spawnPoint, 1))
			{
				list2.Add(newSpawnPoint);
			}
		}
		foreach (Waypoint item in list)
		{
			spawnPoints.Remove(item);
		}
		foreach (Waypoint item2 in list2)
		{
			spawnPoints.Add(item2);
		}
	}

	private bool CheckSpawnPoint(Waypoint point)
	{
		if (point.GetPreviousWaypoints().Length != 0)
		{
			return true;
		}
		return false;
	}

	private List<Waypoint> GetNewSpawnPoints(Waypoint start, int count)
	{
		if (count > 100)
		{
			Debug.LogError("Possible infinite loop while finding new spawn points (count over 100)");
			return null;
		}
		Waypoint[] previousWaypoints = start.GetPreviousWaypoints();
		List<Waypoint> list = new List<Waypoint>();
		if (previousWaypoints.Length == 0)
		{
			list.Add(start);
			return list;
		}
		count++;
		Waypoint[] array = previousWaypoints;
		foreach (Waypoint start2 in array)
		{
			foreach (Waypoint newSpawnPoint in GetNewSpawnPoints(start2, count))
			{
				list.Add(newSpawnPoint);
			}
		}
		return list;
	}
}
