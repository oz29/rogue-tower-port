using UnityEngine;

public class RadarTower : Tower
{
	[SerializeField]
	private GameObject biPlanePrefab;

	[SerializeField]
	private BiPlane myPlane;

	protected override void Start()
	{
		if (myPlane == null)
		{
			myPlane = Object.Instantiate(biPlanePrefab, base.transform.position + new Vector3(-50f, 5f, 0f), Quaternion.identity).GetComponent<BiPlane>();
		}
		base.Start();
	}

	public override void SetStats()
	{
		base.SetStats();
		if (myPlane == null)
		{
			myPlane = Object.Instantiate(biPlanePrefab, base.transform.position + new Vector3(-50f, 5f, 0f), Quaternion.identity).GetComponent<BiPlane>();
		}
		myPlane.damage = base.damage;
		myPlane.healthDamage = base.healthDamage;
		myPlane.armorDamage = base.armorDamage;
		myPlane.shieldDamage = base.shieldDamage;
		myPlane.rps = rps;
		myPlane.slowPercent = base.slowPercent;
		myPlane.bleedPercent = base.bleedPercent;
		myPlane.burnPercent = base.burnPercent;
		myPlane.poisonPercent = base.poisonPercent;
		myPlane.critChance = base.critChance;
		myPlane.stunChance = base.stunChance;
	}

	protected override void Update()
	{
		if (currentTarget != null)
		{
			if (turret != null)
			{
				AimTurret();
			}
			GainXP();
		}
		timeSinceLastShot += Time.deltaTime;
		if (currentTarget != null && timeSinceLastShot > 3f)
		{
			SendTargetInfo(currentTarget);
			timeSinceLastShot = 0f;
		}
	}

	private void SendTargetInfo(GameObject target)
	{
		if (myPlane.target == null)
		{
			myPlane.target = target;
		}
	}

	public override void Demolish()
	{
		Object.Destroy(myPlane.gameObject);
		base.Demolish();
	}
}
