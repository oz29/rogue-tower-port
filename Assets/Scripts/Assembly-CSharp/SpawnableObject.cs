using UnityEngine;

public class SpawnableObject : MonoBehaviour, IBuildable
{
	[SerializeField]
	private GameObject artStuff;

	[SerializeField]
	private bool randomRotation = true;

	[SerializeField]
	private float spawnChance;

	[SerializeField]
	private Vector2 spawnLevels;

	[SerializeField]
	protected GameObject UIObject;

	protected bool spawned;

	protected virtual void Start()
	{
		if ((float)SpawnManager.instance.level >= spawnLevels.x && (float)SpawnManager.instance.level <= spawnLevels.y && Random.Range(1f, 100f) < 100f * spawnChance)
		{
			if (randomRotation)
			{
				artStuff.transform.eulerAngles = new Vector3(0f, Random.Range(0, 4) * 90, 0f);
			}
			artStuff.SetActive(value: true);
			spawned = true;
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void SetStats()
	{
	}

	public virtual void SpawnUI()
	{
		if (UIObject != null)
		{
			Object.Instantiate(UIObject, base.transform.position, Quaternion.identity);
		}
	}

	public void Demolish()
	{
	}
}
