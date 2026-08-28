using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
	public enum ObjectType
	{
		DamageNumber = 0
	}

	public static ObjectPool instance;

	[SerializeField]
	private GameObject damageNumberPrefab;

	[SerializeField]
	private List<GameObject> pooledDamageNumbers = new List<GameObject>();

	private void Awake()
	{
		instance = this;
	}

	public GameObject SpawnObject(ObjectType type, Vector3 position, Quaternion rotation)
	{
		if (type == ObjectType.DamageNumber)
		{
			return SpawnDamageNumber(position, rotation);
		}
		Debug.LogError("Failed to spawn object from pool");
		return null;
	}

	private GameObject SpawnDamageNumber(Vector3 position, Quaternion rotation)
	{
		while (pooledDamageNumbers.Count > 0 && pooledDamageNumbers[0] == null)
		{
			pooledDamageNumbers.RemoveAt(0);
		}
		if (pooledDamageNumbers.Count > 0)
		{
			GameObject gameObject = pooledDamageNumbers[0];
			pooledDamageNumbers.Remove(gameObject);
			gameObject.SetActive(value: true);
			gameObject.transform.position = position;
			gameObject.transform.rotation = rotation;
			gameObject.GetComponent<DamageNumber>().Start();
			return gameObject;
		}
		return Object.Instantiate(damageNumberPrefab, position, rotation);
	}

	public void PoolDamageNumber(GameObject damageNumber)
	{
		pooledDamageNumbers.Add(damageNumber);
		damageNumber.SetActive(value: false);
	}
}
