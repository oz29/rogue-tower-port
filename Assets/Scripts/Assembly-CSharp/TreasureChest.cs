using UnityEngine;

public class TreasureChest : MonoBehaviour, IBuildable
{
	[SerializeField]
	private GameObject uiObject;

	public int numberOfDrops = 1;

	private void Start()
	{
	}

	public void SpawnUI()
	{
		Object.Instantiate(uiObject, base.transform.position, Quaternion.identity).GetComponent<TreasureUI>().myChest = base.gameObject;
	}

	public void Demolish()
	{
	}

	public void SetStats()
	{
	}
}
