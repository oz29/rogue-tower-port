using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TerraformManager : MonoBehaviour
{
	public static TerraformManager instance;

	public enum TerraformMode
	{
		Off,
		Raise,
		Lower
	}

	public TerraformMode currentMode = TerraformMode.Off;

	public const float HEIGHT_STEP = 0.3333333f;
	public const int MAX_HEIGHT_LEVEL = 4;
	public const int MIN_HEIGHT_LEVEL = 0;

	private Dictionary<Vector2Int, GameObject> spawnedPillars = new Dictionary<Vector2Int, GameObject>();
	public Dictionary<Vector2Int, int> cellElevationLevels = new Dictionary<Vector2Int, int>();

	private GameObject ghostIndicator;
	private Material terrainMaterial;
	private GameObject lookoutPrefab;
	private LayerMask grassLayerMask;

	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
			return;
		}
		instance = this;
		grassLayerMask = LayerMask.GetMask("Grass");
	}

	private void Start()
	{
		CreateGhostIndicator();
		FindTerrainMaterial();
		FindLookoutPrefab();
	}

	private void FindLookoutPrefab()
	{
		GameObject[] all = Resources.FindObjectsOfTypeAll<GameObject>();
		for (int i = 0; i < all.Length; i++)
		{
			if (all[i] != null && all[i].name == "Lookout" && all[i].GetComponentInChildren<MeshRenderer>() != null)
			{
				lookoutPrefab = all[i];
				break;
			}
		}
	}

	private void FindTerrainMaterial()
	{
		GameObject terrainObj = GameObject.Find("Terrain");
		if (terrainObj != null)
		{
			MeshRenderer mr = terrainObj.GetComponent<MeshRenderer>();
			if (mr != null && mr.sharedMaterial != null)
			{
				terrainMaterial = mr.sharedMaterial;
			}
		}
	}

	private void CreateGhostIndicator()
	{
		ghostIndicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
		ghostIndicator.name = "TerraformGhost";
		Destroy(ghostIndicator.GetComponent<Collider>());

		MeshRenderer mr = ghostIndicator.GetComponent<MeshRenderer>();
		mr.material = new Material(Shader.Find("Standard"));
		mr.material.color = new Color(0.2f, 0.9f, 0.3f, 0.45f);
		mr.material.SetFloat("_Mode", 3f);
		mr.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
		mr.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
		mr.material.SetInt("_ZWrite", 0);
		mr.material.DisableKeyword("_ALPHATEST_ON");
		mr.material.EnableKeyword("_ALPHABLEND_ON");
		mr.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
		mr.material.renderQueue = 3000;

		ghostIndicator.transform.localScale = new Vector3(0.95f, 0.333f, 0.95f);
		ghostIndicator.SetActive(false);
	}

	public void SetMode(TerraformMode mode)
	{
		currentMode = mode;
		if (ghostIndicator != null)
		{
			ghostIndicator.SetActive(mode != TerraformMode.Off);
			if (mode == TerraformMode.Raise)
			{
				ghostIndicator.GetComponent<MeshRenderer>().material.color = new Color(0.2f, 0.9f, 0.3f, 0.45f);
			}
			else if (mode == TerraformMode.Lower)
			{
				ghostIndicator.GetComponent<MeshRenderer>().material.color = new Color(0.9f, 0.3f, 0.2f, 0.45f);
			}
		}

		if (mode != TerraformMode.Off && BuildingManager.instance != null && BuildingManager.instance.buildMode)
		{
			BuildingManager.instance.ExitBuildMode();
		}
	}

	public void ToggleMode()
	{
		if (currentMode == TerraformMode.Off)
		{
			SetMode(TerraformMode.Raise);
		}
		else if (currentMode == TerraformMode.Raise)
		{
			SetMode(TerraformMode.Lower);
		}
		else
		{
			SetMode(TerraformMode.Off);
		}
	}

	private void Update()
	{
		if (currentMode == TerraformMode.Off)
		{
			if (ghostIndicator != null && ghostIndicator.activeSelf)
			{
				ghostIndicator.SetActive(false);
			}
			return;
		}

		if (PauseMenu.instance != null && PauseMenu.instance.paused)
		{
			if (ghostIndicator != null && ghostIndicator.activeSelf) ghostIndicator.SetActive(false);
			return;
		}

		if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
		{
			if (ghostIndicator != null && ghostIndicator.activeSelf) ghostIndicator.SetActive(false);
			return;
		}
		if (Input.touchCount > 0 && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
		{
			if (ghostIndicator != null && ghostIndicator.activeSelf) ghostIndicator.SetActive(false);
			return;
		}

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out RaycastHit hit, 2000f, grassLayerMask, QueryTriggerInteraction.Ignore))
		{
			int gridX = Mathf.RoundToInt(hit.point.x);
			int gridZ = Mathf.RoundToInt(hit.point.z);
			Vector2Int gridKey = new Vector2Int(gridX, gridZ);

			int currentLevel = GetCurrentCellLevel(gridKey, hit.point.y);
			float targetY = (currentLevel + (currentMode == TerraformMode.Raise ? 1 : -1)) * HEIGHT_STEP;
			if (targetY < 0.333f) targetY = 0.333f;

			if (ghostIndicator != null)
			{
				ghostIndicator.SetActive(true);
				ghostIndicator.transform.position = new Vector3(gridX, targetY, gridZ);
			}

			if (Input.GetMouseButtonDown(0))
			{
				if (currentMode == TerraformMode.Raise)
				{
					RaiseCell(gridKey, hit.point.y);
				}
				else if (currentMode == TerraformMode.Lower)
				{
					LowerCell(gridKey, hit.point.y);
				}
			}
		}
		else
		{
			if (ghostIndicator != null && ghostIndicator.activeSelf)
			{
				ghostIndicator.SetActive(false);
			}
		}
	}

	public int GetCurrentCellLevel(Vector2Int key, float fallbackY)
	{
		if (cellElevationLevels.TryGetValue(key, out int lvl))
		{
			return lvl;
		}
		return Mathf.Max(0, Mathf.RoundToInt(fallbackY * 3f) - 1);
	}

	public void RaiseCell(Vector2Int gridKey, float currentHitY)
	{
		int currentLvl = GetCurrentCellLevel(gridKey, currentHitY);
		if (currentLvl >= MAX_HEIGHT_LEVEL) return;

		int newLvl = currentLvl + 1;
		cellElevationLevels[gridKey] = newLvl;
		ApplyPillarElevation(gridKey, newLvl);

		if (ObjectPool.instance != null)
		{
			DamageNumber dn = ObjectPool.instance.SpawnObject(ObjectPool.ObjectType.DamageNumber, new Vector3(gridKey.x, (newLvl + 1) * HEIGHT_STEP + 0.5f, gridKey.y), Quaternion.identity)?.GetComponent<DamageNumber>();
			if (dn != null)
			{
				dn.SetText("+" + newLvl, "Green", 1f);
				dn.SetHoldTime(0.25f);
			}
		}

		if (SFXManager.instance != null)
		{
			SFXManager.instance.ButtonClick();
		}
		if (RunSaveManager.instance != null)
		{
			RunSaveManager.SaveCurrentRun();
		}
	}

	public void LowerCell(Vector2Int gridKey, float currentHitY)
	{
		int currentLvl = GetCurrentCellLevel(gridKey, currentHitY);
		if (currentLvl <= MIN_HEIGHT_LEVEL) return;

		int newLvl = currentLvl - 1;
		cellElevationLevels[gridKey] = newLvl;
		ApplyPillarElevation(gridKey, newLvl);

		if (ObjectPool.instance != null)
		{
			DamageNumber dn = ObjectPool.instance.SpawnObject(ObjectPool.ObjectType.DamageNumber, new Vector3(gridKey.x, (newLvl + 1) * HEIGHT_STEP + 0.5f, gridKey.y), Quaternion.identity)?.GetComponent<DamageNumber>();
			if (dn != null)
			{
				string txt = newLvl > 0 ? ("+" + newLvl) : "0";
				dn.SetText(txt, "Grey", 1f);
				dn.SetHoldTime(0.25f);
			}
		}

		if (SFXManager.instance != null)
		{
			SFXManager.instance.ButtonClick();
		}
		if (RunSaveManager.instance != null)
		{
			RunSaveManager.SaveCurrentRun();
		}
	}

	public void ApplyPillarElevation(Vector2Int gridKey, int level)
	{
		float targetTopY = (level + 1) * HEIGHT_STEP;

		if (spawnedPillars.TryGetValue(gridKey, out GameObject existingPillar) && existingPillar != null)
		{
			Destroy(existingPillar);
			spawnedPillars.Remove(gridKey);
		}

		if (level == 0)
		{
			UpdateTowerOnCell(gridKey, targetTopY);
			return;
		}

		if (lookoutPrefab == null)
		{
			FindLookoutPrefab();
		}

		GameObject pillar = null;
		if (level == 4 && lookoutPrefab != null)
		{
			pillar = Instantiate(lookoutPrefab, new Vector3(gridKey.x, 0f, gridKey.y), Quaternion.identity);
			pillar.name = "TerraformLookout_" + gridKey.x + "_" + gridKey.y;

			Lookout lk = pillar.GetComponent<Lookout>();
			if (lk != null) Destroy(lk);

			BoxCollider bc = pillar.GetComponent<BoxCollider>();
			if (bc == null) bc = pillar.AddComponent<BoxCollider>();
			bc.center = new Vector3(0f, targetTopY - 0.1f, 0f);
			bc.size = new Vector3(1f, 0.2f, 1f);

			pillar.layer = LayerMask.NameToLayer("Grass");
			foreach (Transform child in pillar.GetComponentsInChildren<Transform>())
			{
				child.gameObject.layer = LayerMask.NameToLayer("Grass");
			}
		}
		else
		{
			pillar = GameObject.CreatePrimitive(PrimitiveType.Cube);
			pillar.name = "TerraformPillar_" + gridKey.x + "_" + gridKey.y;
			pillar.layer = LayerMask.NameToLayer("Grass");

			if (terrainMaterial != null)
			{
				pillar.GetComponent<MeshRenderer>().sharedMaterial = terrainMaterial;
			}
			else
			{
				FindTerrainMaterial();
				if (terrainMaterial != null) pillar.GetComponent<MeshRenderer>().sharedMaterial = terrainMaterial;
			}

			pillar.transform.localScale = new Vector3(1f, targetTopY, 1f);
			pillar.transform.position = new Vector3(gridKey.x, targetTopY / 2f, gridKey.y);
		}

		if (pillar != null)
		{
			spawnedPillars[gridKey] = pillar;
		}

		UpdateTowerOnCell(gridKey, targetTopY);
	}

	private void UpdateTowerOnCell(Vector2Int gridKey, float newTopY)
	{
		Tower[] allTowers = FindObjectsOfType<Tower>();
		for (int i = 0; i < allTowers.Length; i++)
		{
			Tower t = allTowers[i];
			if (t == null) continue;

			int tx = Mathf.RoundToInt(t.transform.position.x);
			int tz = Mathf.RoundToInt(t.transform.position.z);
			if (tx == gridKey.x && tz == gridKey.y)
			{
				Vector3 p = t.transform.position;
				p.y = newTopY;
				t.transform.position = p;
				t.SetStats();

				if (BuildingManager.instance != null && BuildingManager.instance.levelUpFX != null)
				{
					Instantiate(BuildingManager.instance.levelUpFX, p + Vector3.up * 0.5f, Quaternion.identity);
				}
				break;
			}
		}
	}
}
