using System.Collections.Generic;
using UnityEngine;

public class Sawblade : Projectile
{
	private Pathfinder targetPathfinder;

	private Waypoint nextWaypoint;

	private bool pathMode;

	private HashSet<Collider> targetsHit = new HashSet<Collider>();

	public override void SetStats(TowerType whoShotMe, GameObject _target, float spd, int dmg, int healthDmg, int armorDmg, int shieldDmg, float slow, float bleed, float burn, float poison, float crit, float stun)
	{
		base.SetStats(whoShotMe, _target, spd, dmg, healthDmg, armorDmg, shieldDmg, slow, bleed, burn, poison, crit, stun);
		targetPathfinder = target.GetComponent<Pathfinder>();
		nextWaypoint = targetPathfinder.currentWaypoint;
	}

	protected override void AlterCourse()
	{
		if (!pathMode && targetPathfinder != null)
		{
			nextWaypoint = targetPathfinder.currentWaypoint;
		}
		if (!pathMode && (target == null || Vector3.SqrMagnitude(target.transform.position - base.transform.position) < 0.125f))
		{
			nextWaypoint = GetPreviousWaypoint();
			pathMode = true;
		}
		Vector3 position;
		if (!pathMode)
		{
			position = target.transform.position;
		}
		else
		{
			if (Vector3.SqrMagnitude(nextWaypoint.transform.position - base.transform.position) < 0.125f)
			{
				nextWaypoint = GetPreviousWaypoint();
			}
			position = nextWaypoint.transform.position;
		}
		Quaternion rotation = Quaternion.LookRotation(position - base.transform.position, Vector3.up);
		base.transform.rotation = rotation;
	}

	protected override void CheckForHits()
	{
		Collider[] array = Physics.OverlapBox(base.transform.position, Vector3.one * 0.25f, Quaternion.identity, layermask, QueryTriggerInteraction.Collide);
		foreach (Collider collider in array)
		{
			if (!pathMode && collider.gameObject == target)
			{
				nextWaypoint = GetPreviousWaypoint();
				pathMode = true;
			}
			if (targetsHit.Contains(collider))
			{
				continue;
			}
			IDamageable component = collider.GetComponent<IDamageable>();
			if (component != null)
			{
				DealDamage(component);
				damage--;
				if (damage <= 0)
				{
					Object.Destroy(base.gameObject);
				}
			}
			targetsHit.Add(collider);
		}
	}

	private Waypoint GetPreviousWaypoint()
	{
		Waypoint[] previousWaypoints = nextWaypoint.GetPreviousWaypoints();
		if (previousWaypoints.Length == 0)
		{
			Object.Destroy(base.gameObject);
			return nextWaypoint;
		}
		return previousWaypoints[Random.Range(0, previousWaypoints.Length)];
	}
}
