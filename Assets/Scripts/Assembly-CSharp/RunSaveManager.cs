using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

[Serializable]
public class RunSaveData
{
	public int gameMode;
	public int level;
	public int health;
	public int maxHealth;
	public int gold;
	public int mana;
	public int maxMana;
	public int manaGatherRate;
	public int manaBankBonusMana;

	public float dotTickX;
	public float dotTickY;
	public float dotTickZ;

	public int bonusDamageOnBleed;
	public int bonusDamageOnBurn;
	public int bonusDamageOnPoison;
	public int bonusDamageOnStun;
	public float poisonSlowPercent;
	public float burnSpeedDamagePercentBonus;
	public float bleedingCritChance;
	public float bleedPop;
	public float burnPop;
	public float poisonPop;

	public int extraTowerDamage;
	public int extraGoldDrop;
	public float manaDropOnDeath;
	public float speedBonus;
	public float slowCapModifier;
	public float hasteCapModifier;

	public List<SavedTileData> tiles = new List<SavedTileData>();
	public List<SavedTowerData> towers = new List<SavedTowerData>();
	public List<string> pickedCards = new List<string>();
}

[Serializable]
public class SavedTileData
{
	public int posX;
	public int posY;
	public int eulerAngle;
	public string prefabName;
}

[Serializable]
public class SavedTowerData
{
	public int towerType;
	public float posX;
	public float posY;
	public float posZ;
	public float rotY;
	public int level;
	public int damageUpgrade;
	public int healthDamageUpgrade;
	public int armorDamageUpgrade;
	public int shieldDamageUpgrade;
	public float healthXP;
	public float armorXP;
	public float shieldXP;
	public int priority0;
	public int priority1;
	public int priority2;
}

public class RunSaveManager : MonoBehaviour
{
	public static RunSaveManager instance;
	public static bool loadSavedRunOnStart = false;

	public List<string> currentRunPickedCards = new List<string>();
	public List<SavedTileData> currentRunTiles = new List<SavedTileData>();

	private static string SaveFilePath => Path.Combine(Application.persistentDataPath, "roguetower_midgame_run.json");

	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
			return;
		}
		instance = this;
	}

	public static bool HasSavedRun()
	{
		try
		{
			return File.Exists(SaveFilePath);
		}
		catch
		{
			return false;
		}
	}

	public static RunSaveData GetSavedRunHeader()
	{
		try
		{
			if (!HasSavedRun()) return null;
			string json = File.ReadAllText(SaveFilePath);
			return JsonConvert.DeserializeObject<RunSaveData>(json);
		}
		catch
		{
			return null;
		}
	}

	public static void DeleteSavedRun()
	{
		try
		{
			if (File.Exists(SaveFilePath))
			{
				File.Delete(SaveFilePath);
				Debug.Log("[RunSaveManager] Saved run deleted.");
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("[RunSaveManager] Error deleting save: " + ex.Message);
		}
	}

	public void RecordPickedCard(string cardTitle)
	{
		if (!string.IsNullOrEmpty(cardTitle))
		{
			currentRunPickedCards.Add(cardTitle);
		}
	}

	public void RecordSpawnedTile(int posX, int posY, int eulerAngle, string prefabName)
	{
		currentRunTiles.Add(new SavedTileData
		{
			posX = posX,
			posY = posY,
			eulerAngle = eulerAngle,
			prefabName = prefabName
		});
	}

	public static void SaveCurrentRun()
	{
		if (GameManager.instance == null || GameManager.instance.gameOver)
		{
			return;
		}

		try
		{
			RunSaveData data = new RunSaveData();

			data.gameMode = GameManager.instance.gameMode;
			data.level = SpawnManager.instance != null ? SpawnManager.instance.level : 1;
			data.health = GameManager.instance.health;
			data.maxHealth = GameManager.instance.maxHealth;

			if (ResourceManager.instance != null)
			{
				data.gold = GetPrivateField<int>(ResourceManager.instance, "gold");
				data.mana = GetPrivateField<int>(ResourceManager.instance, "mana");
				data.maxMana = GetPrivateField<int>(ResourceManager.instance, "maxMana");
				data.manaGatherRate = GetPrivateField<int>(ResourceManager.instance, "manaGatherRate");
				data.manaBankBonusMana = ResourceManager.instance.manaBankBonusMana;
			}

			data.dotTickX = GameManager.instance.dotTick.x;
			data.dotTickY = GameManager.instance.dotTick.y;
			data.dotTickZ = GameManager.instance.dotTick.z;

			if (MonsterManager.instance != null)
			{
				data.bonusDamageOnBleed = MonsterManager.instance.bonusDamageOnBleed;
				data.bonusDamageOnBurn = MonsterManager.instance.bonusDamageOnBurn;
				data.bonusDamageOnPoison = MonsterManager.instance.bonusDamageOnPoison;
				data.bonusDamageOnStun = MonsterManager.instance.bonusDamageOnStun;
				data.poisonSlowPercent = MonsterManager.instance.poisonSlowPercent;
				data.burnSpeedDamagePercentBonus = MonsterManager.instance.burnSpeedDamagePercentBonus;
				data.bleedingCritChance = MonsterManager.instance.bleedingCritChance;
				data.bleedPop = MonsterManager.instance.bleedPop;
				data.burnPop = MonsterManager.instance.burnPop;
				data.poisonPop = MonsterManager.instance.poisonPop;
				data.extraTowerDamage = MonsterManager.instance.extraTowerDamage;
				data.extraGoldDrop = MonsterManager.instance.extraGoldDrop;
				data.manaDropOnDeath = MonsterManager.instance.manaDropOnDeath;
				data.speedBonus = MonsterManager.instance.speedBonus;
				data.slowCapModifier = MonsterManager.instance.slowCapModifier;
				data.hasteCapModifier = MonsterManager.instance.hasteCapModifier;
			}

			if (instance != null)
			{
				data.tiles = new List<SavedTileData>(instance.currentRunTiles);
				data.pickedCards = new List<string>(instance.currentRunPickedCards);
			}

			// Collect all towers
			Tower[] allTowers = FindObjectsOfType<Tower>();
			for (int i = 0; i < allTowers.Length; i++)
			{
				Tower t = allTowers[i];
				if (t == null) continue;

				SavedTowerData st = new SavedTowerData
				{
					towerType = (int)t.towerType,
					posX = t.transform.position.x,
					posY = t.transform.position.y,
					posZ = t.transform.position.z,
					rotY = t.transform.eulerAngles.y,
					level = t.level,
					damageUpgrade = GetPrivateField<int>(t, "damageUpgrade"),
					healthDamageUpgrade = GetPrivateField<int>(t, "healthDamageUpgrade"),
					armorDamageUpgrade = GetPrivateField<int>(t, "armorDamageUpgrade"),
					shieldDamageUpgrade = GetPrivateField<int>(t, "shieldDamageUpgrade"),
					healthXP = t.healthXP,
					armorXP = t.armorXP,
					shieldXP = t.shieldXP,
					priority0 = (int)t.priorities[0],
					priority1 = (int)t.priorities[1],
					priority2 = (int)t.priorities[2]
				};
				data.towers.Add(st);
			}

			string json = JsonConvert.SerializeObject(data, Formatting.Indented);
			File.WriteAllText(SaveFilePath, json);
			Debug.Log("[RunSaveManager] Run saved successfully at level " + data.level);
		}
		catch (Exception ex)
		{
			Debug.LogError("[RunSaveManager] Error saving run: " + ex.Message + "\n" + ex.StackTrace);
		}
	}

	public void TryRestoreRun()
	{
		if (!loadSavedRunOnStart || !HasSavedRun())
		{
			return;
		}

		loadSavedRunOnStart = false;
		RunSaveData data = GetSavedRunHeader();
		if (data == null) return;

		StartCoroutine(RestoreRunRoutine(data));
	}

	private System.Collections.IEnumerator RestoreRunRoutine(RunSaveData data)
	{
		yield return new WaitForSeconds(0.2f);

		try
		{
			// 1. Restore resources & health
			GameManager.instance.health = data.health;
			GameManager.instance.maxHealth = data.maxHealth;
			GameManager.instance.dotTick = new Vector3(data.dotTickX, data.dotTickY, data.dotTickZ);
			GameManager.instance.UpdateHealthBar();

			if (ResourceManager.instance != null)
			{
				SetPrivateField(ResourceManager.instance, "gold", data.gold);
				SetPrivateField(ResourceManager.instance, "mana", data.mana);
				SetPrivateField(ResourceManager.instance, "maxMana", data.maxMana);
				SetPrivateField(ResourceManager.instance, "manaGatherRate", data.manaGatherRate);
				ResourceManager.instance.manaBankBonusMana = data.manaBankBonusMana;
				ResourceManager.instance.SetManaBar();
				ResourceManager.instance.UpdateManaHUD();
			}

			if (MonsterManager.instance != null)
			{
				MonsterManager.instance.bonusDamageOnBleed = data.bonusDamageOnBleed;
				MonsterManager.instance.bonusDamageOnBurn = data.bonusDamageOnBurn;
				MonsterManager.instance.bonusDamageOnPoison = data.bonusDamageOnPoison;
				MonsterManager.instance.bonusDamageOnStun = data.bonusDamageOnStun;
				MonsterManager.instance.poisonSlowPercent = data.poisonSlowPercent;
				MonsterManager.instance.burnSpeedDamagePercentBonus = data.burnSpeedDamagePercentBonus;
				MonsterManager.instance.bleedingCritChance = data.bleedingCritChance;
				MonsterManager.instance.bleedPop = data.bleedPop;
				MonsterManager.instance.burnPop = data.burnPop;
				MonsterManager.instance.poisonPop = data.poisonPop;
				MonsterManager.instance.extraTowerDamage = data.extraTowerDamage;
				MonsterManager.instance.extraGoldDrop = data.extraGoldDrop;
				MonsterManager.instance.manaDropOnDeath = data.manaDropOnDeath;
				MonsterManager.instance.speedBonus = data.speedBonus;
				MonsterManager.instance.slowCapModifier = data.slowCapModifier;
				MonsterManager.instance.hasteCapModifier = data.hasteCapModifier;
			}

			// 2. Re-apply picked cards
			if (data.pickedCards != null && CardManager.instance != null)
			{
				UpgradeCard[] allCards = Resources.FindObjectsOfTypeAll<UpgradeCard>();
				foreach (string cardTitle in data.pickedCards)
				{
					for (int i = 0; i < allCards.Length; i++)
					{
						if (allCards[i].title == cardTitle)
						{
							allCards[i].Upgrade();
							currentRunPickedCards.Add(cardTitle);
							break;
						}
					}
				}
			}

			// 3. Rebuild placed tiles
			if (data.tiles != null && TileManager.instance != null)
			{
				foreach (SavedTileData tile in data.tiles)
				{
					TileManager.instance.SpawnNewTile(tile.posX, tile.posY, tile.eulerAngle);
				}
			}

			// 4. Restore placed towers
			if (data.towers != null)
			{
				BuildButtonUI[] buttons = FindObjectsOfType<BuildButtonUI>();
				Dictionary<int, GameObject> prefabMap = new Dictionary<int, GameObject>();
				for (int i = 0; i < buttons.Length; i++)
				{
					TowerType tType = GetPrivateField<TowerType>(buttons[i], "myTowerType");
					GameObject tObj = GetPrivateField<GameObject>(buttons[i], "tower");
					if (tObj != null && !prefabMap.ContainsKey((int)tType))
					{
						prefabMap.Add((int)tType, tObj);
					}
				}

				foreach (SavedTowerData st in data.towers)
				{
					if (prefabMap.TryGetValue(st.towerType, out GameObject prefab))
					{
						Vector3 pos = new Vector3(st.posX, st.posY, st.posZ);
						Quaternion rot = Quaternion.Euler(0f, st.rotY, 0f);
						GameObject towerObj = Instantiate(prefab, pos, rot);
						Tower t = towerObj.GetComponent<Tower>();
						if (t != null)
						{
							SetPrivateProperty(t, "level", st.level);
							SetPrivateField(t, "damageUpgrade", st.damageUpgrade);
							SetPrivateField(t, "healthDamageUpgrade", st.healthDamageUpgrade);
							SetPrivateField(t, "armorDamageUpgrade", st.armorDamageUpgrade);
							SetPrivateField(t, "shieldDamageUpgrade", st.shieldDamageUpgrade);
							SetPrivateProperty(t, "healthXP", st.healthXP);
							SetPrivateProperty(t, "armorXP", st.armorXP);
							SetPrivateProperty(t, "shieldXP", st.shieldXP);
							t.priorities[0] = (Tower.Priority)st.priority0;
							t.priorities[1] = (Tower.Priority)st.priority1;
							t.priorities[2] = (Tower.Priority)st.priority2;
							t.SetStats();
						}
					}
				}
			}

			// 5. Restore level/wave
			if (SpawnManager.instance != null)
			{
				SpawnManager.instance.level = data.level;
				Text lvlText = GetPrivateField<Text>(SpawnManager.instance, "levelText");
				if (lvlText != null)
				{
					lvlText.text = "Level: " + data.level;
				}
				SpawnManager.instance.ShowSpawnUIs(true);
			}

			Debug.Log("[RunSaveManager] Run restored successfully at level " + data.level);
		}
		catch (Exception ex)
		{
			Debug.LogError("[RunSaveManager] Error restoring run: " + ex.Message + "\n" + ex.StackTrace);
		}
	}

	private static T GetPrivateField<T>(object target, string fieldName)
	{
		var field = target.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
		if (field != null) return (T)field.GetValue(target);
		return default(T);
	}

	private static void SetPrivateField(object target, string fieldName, object value)
	{
		var field = target.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
		if (field != null) field.SetValue(target, value);
	}

	private static void SetPrivateProperty(object target, string propertyName, object value)
	{
		var prop = target.GetType().GetProperty(propertyName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
		if (prop != null && prop.CanWrite) prop.SetValue(target, value, null);
	}
}
