using UnityEngine;

public class BattleCry : MonoBehaviour
{
	public enum BattleCryTrigger
	{
		Death = 0,
		Spawn = 1,
		NearEnd = 2,
		ArmorBreak = 3,
		ShieldBreak = 4
	}

	[SerializeField]
	private LayerMask enemyLayerMask;

	[SerializeField]
	private string battleCryText = "Battle Cry!";

	[SerializeField]
	private float cryRadius;

	[SerializeField]
	private BattleCryTrigger myTrigger;

	[SerializeField]
	private float fortifyTime;

	[SerializeField]
	private float hastePercentage;

	[SerializeField]
	private GameObject[] enemiesToSpawn;

	private bool triggered;

	public void CheckBattleCry(BattleCryTrigger source)
	{
		if (source == myTrigger && !triggered)
		{
			Cry();
		}
	}

	private void Cry()
	{
		if (enemiesToSpawn.Length != 0)
		{
			Waypoint currentWaypoint = GetComponent<Pathfinder>().currentWaypoint;
			GameObject[] array = enemiesToSpawn;
			for (int i = 0; i < array.Length; i++)
			{
				Enemy component = Object.Instantiate(array[i], base.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f)), Quaternion.identity).GetComponent<Enemy>();
				component.SetStats();
				component.SetFirstSpawnPoint(currentWaypoint);
				SpawnManager.instance.currentEnemies.Add(component);
			}
		}
		DamageNumber component2 = ObjectPool.instance.SpawnObject(ObjectPool.ObjectType.DamageNumber, base.transform.position, Quaternion.identity).GetComponent<DamageNumber>();
		component2.SetText(battleCryText, "Grey", 1f);
		component2.SetHoldTime(2f);
		if (myTrigger != BattleCryTrigger.Death)
		{
			component2.transform.parent = base.transform;
		}
		Collider[] array2 = Physics.OverlapSphere(base.transform.position, cryRadius, enemyLayerMask, QueryTriggerInteraction.Collide);
		for (int i = 0; i < array2.Length; i++)
		{
			Enemy component3 = array2[i].GetComponent<Enemy>();
			if (component3 != null)
			{
				if (fortifyTime > 0f)
				{
					component3.Fortify(fortifyTime);
				}
				if (hastePercentage > 0f)
				{
					component3.AddHaste(hastePercentage);
				}
			}
		}
		triggered = true;
	}
}
