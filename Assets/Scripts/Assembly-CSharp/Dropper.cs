using System.Collections.Generic;
using UnityEngine;

public class Dropper : Tower
{
	[SerializeField]
	private int manaPerSecond;

	[SerializeField]
	private float dropHeight = 3f;

	[SerializeField]
	private LayerMask dropSetMask;

	[SerializeField]
	protected Vector2[] dropPoints;

	[SerializeField]
	private bool requireTargetToDrop;

	private int currentLevel;

	public float dropperRPMdisplay;

	private float timeOfNextManaCheck;

	private bool canFire = true;

	protected override void Start()
	{
		base.Start();
		SetDropPoints();
	}

	public override void SetStats()
	{
		base.SetStats();
		rps = 60f / rpm * (10f / (10f + (float)dropPoints.Length));
		dropperRPMdisplay = rpm * (10f + (float)dropPoints.Length) / 10f;
	}

	private void SetDropPoints()
	{
		List<Vector2> list = new List<Vector2>();
		for (int i = -(int)base.range; (float)i <= base.range; i++)
		{
			for (int j = -(int)base.range; (float)j <= base.range; j++)
			{
				if (Physics.Raycast(new Vector3(base.transform.position.x + (float)i, dropHeight, base.transform.position.z + (float)j), Vector3.down, out var hitInfo, 1.5f * dropHeight, dropSetMask, QueryTriggerInteraction.Ignore) && hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Path"))
				{
					list.Add(new Vector2(i, j));
				}
			}
		}
		dropPoints = list.ToArray();
		rps = 60f / rpm * (10f / (10f + (float)dropPoints.Length));
		dropperRPMdisplay = rpm * (10f + (float)dropPoints.Length) / 10f;
	}

	protected override void Update()
	{
		if (SpawnManager.instance.level != currentLevel)
		{
			SetDropPoints();
			currentLevel = SpawnManager.instance.level;
		}
		if (currentTarget != null)
		{
			if (turret != null)
			{
				AimTurret();
			}
			GainXP();
		}
		if (manaPerSecond > 0 && Time.time >= timeOfNextManaCheck && SpawnManager.instance.combat)
		{
			timeOfNextManaCheck = Time.time + 1f;
			if (ResourceManager.instance.CheckMana(manaPerSecond))
			{
				ResourceManager.instance.SpendMana(manaPerSecond);
				canFire = true;
			}
			else
			{
				canFire = false;
			}
		}
		timeSinceLastShot += Time.deltaTime;
		if (canFire && TargetingCheck() && timeSinceLastShot > rps && SpawnManager.instance.combat)
		{
			Fire();
			timeSinceLastShot = 0f;
		}
	}

	private bool TargetingCheck()
	{
		if (requireTargetToDrop)
		{
			if (currentTarget != null)
			{
				return true;
			}
			return false;
		}
		return true;
	}

	protected override void Fire()
	{
		if (consumesMana)
		{
			int manaCost = (int)((float)base.damage * manaConsumptionRate);
			if (!ResourceManager.instance.CheckMana(manaCost))
			{
				return;
			}
			ResourceManager.instance.SpendMana(manaCost);
		}
		if (dropPoints.Length != 0)
		{
			Vector2 vector = dropPoints[Random.Range(0, dropPoints.Length)];
			Object.Instantiate(projectile, new Vector3(vector.x, dropHeight, vector.y) + base.transform.position, Quaternion.identity).GetComponent<Projectile>().SetStats(towerType, null, projectileSpeed, base.damage, base.healthDamage, base.armorDamage, base.shieldDamage, base.slowPercent, base.bleedPercent, base.burnPercent, base.poisonPercent, base.critChance, base.stunChance);
		}
	}
}
