using UnityEngine;

public class BuildingManager : MonoBehaviour
{
	public static BuildingManager instance;

	[SerializeField]
	private GameObject buildingGhost;

	private GameObject currentGhost;

	[SerializeField]
	private GameObject placementFXObject;

	[SerializeField]
	public GameObject levelUpFX;

	[SerializeField]
	private LayerMask buildableMask;

	private bool buildSpotAvailable;

	private GameObject thingToBuild;

	private int buildingCost;

	private int priceIncrease;

	private TowerType tType;

	public bool buildMode { get; private set; }

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		buildMode = false;
	}

	private void FixedUpdate()
	{
		if (buildMode)
		{
			buildSpotAvailable = SamplePoint();
		}
	}

	public bool continuousBuildMode;

	private void Update()
	{
		if (!buildMode)
		{
			return;
		}
		if (Input.GetMouseButtonDown(0) && BuildingCheck())
		{
			ResourceManager.instance.Spend(buildingCost);
			DamageTracker.instance.AddCost(tType, buildingCost);
			Build();
			if (!Input.GetKey(KeyCode.LeftShift) && !continuousBuildMode)
			{
				ExitBuildMode();
			}
			else
			{
				buildingCost += priceIncrease;
			}
		}
		if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
		{
			ExitBuildMode();
		}
	}

	public void EnterBuildMode(GameObject objectToBuild, int cost, int _priceIncrease, TowerType type)
	{
		buildMode = true;
		thingToBuild = objectToBuild;
		buildingCost = cost;
		priceIncrease = _priceIncrease;
		tType = type;
	}

	public void ExitBuildMode()
	{
		HideGhost();
		buildMode = false;
	}

	private void Build()
	{
		GameObject gameObject = Object.Instantiate(thingToBuild, currentGhost.transform.position, Quaternion.identity);
		gameObject.GetComponent<IBuildable>()?.SetStats();
		Object.Instantiate(placementFXObject, gameObject.transform.position + Vector3.up * 0.333f, Quaternion.identity);
		buildSpotAvailable = SamplePoint();
		RunSaveManager.SaveCurrentRun();
	}

	private bool BuildingCheck()
	{
		if (!PauseMenu.instance.paused && buildSpotAvailable && ResourceManager.instance.CheckMoney(buildingCost))
		{
			return true;
		}
		if (!ResourceManager.instance.CheckMoney(buildingCost) && !PauseMenu.instance.paused && buildSpotAvailable)
		{
			DamageNumber component = ObjectPool.instance.SpawnObject(ObjectPool.ObjectType.DamageNumber, currentGhost.transform.position + Vector3.up, Quaternion.identity).GetComponent<DamageNumber>();
			component.SetText("Not enough gold", "Grey", 1f);
			component.SetHoldTime(0.25f);
		}
		return false;
	}

	private bool SamplePoint()
	{
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hitInfo, 2000f, buildableMask, QueryTriggerInteraction.Ignore))
		{
			if (hitInfo.collider.gameObject.layer != LayerMask.NameToLayer("Grass"))
			{
				HideGhost();
				return false;
			}
			Vector3 vector = new Vector3(Mathf.Round(hitInfo.point.x), Mathf.Round(3f * hitInfo.point.y) / 3f, Mathf.Round(hitInfo.point.z));
			if (Vector3.SqrMagnitude(hitInfo.point - vector) < 0.25f)
			{
				string text = "";
				if (vector.y > 0.34f)
				{
					text = "+" + (Mathf.RoundToInt(vector.y * 3f) - 1);
				}
				DisplayGhost(vector, text);
				return true;
			}
			HideGhost();
			return false;
		}
		HideGhost();
		return false;
	}

	private void DisplayGhost(Vector3 pos, string text)
	{
		if (currentGhost == null)
		{
			currentGhost = Object.Instantiate(buildingGhost, pos, Quaternion.identity);
		}
		else
		{
			currentGhost.SetActive(value: true);
			currentGhost.transform.position = pos;
		}
		currentGhost.GetComponent<BuildingGhost>().SetText(text);
	}

	private void HideGhost()
	{
		if (currentGhost != null)
		{
			currentGhost.SetActive(value: false);
		}
	}
}
