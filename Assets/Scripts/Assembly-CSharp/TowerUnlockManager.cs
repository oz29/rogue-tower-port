using System.Collections.Generic;
using UnityEngine;

public class TowerUnlockManager : MonoBehaviour
{
	public static TowerUnlockManager instance;

	[SerializeField]
	private List<GameObject> unlockedTowers = new List<GameObject>();

	[SerializeField]
	private List<GameObject> unlockedBuildings = new List<GameObject>();

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		DisplayButtons();
	}

	public void UnlockTower(GameObject towerUIObject, bool isTower)
	{
		towerUIObject.SetActive(value: true);
		if (isTower)
		{
			unlockedTowers.Add(towerUIObject);
		}
		else
		{
			unlockedBuildings.Add(towerUIObject);
		}
		DisplayButtons();
	}

	private void DisplayButtons()
	{
		List<GameObject> allItems = new List<GameObject>();
		allItems.AddRange(unlockedTowers);
		allItems.AddRange(unlockedBuildings);

		int count = allItems.Count;
		int num = 0;
		foreach (GameObject item in allItems)
		{
			item.transform.localPosition = new Vector3(num * 100 - count * 50 + 50, 0f, 0f);
			num++;
		}
	}
}
