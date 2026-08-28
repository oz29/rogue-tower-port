using UnityEngine;

public class SnowFlake : Projectile
{
	protected override void Start()
	{
		base.Start();
		base.transform.Translate(new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f)));
	}

	protected override void MoveProjectile()
	{
		base.transform.Translate(Vector3.down * speed * Time.fixedDeltaTime);
	}

	protected override void CheckForHits()
	{
		if (Physics.SphereCast(base.transform.position, 0.125f, Vector3.down, out var hitInfo, speed * Time.fixedDeltaTime, layermask, QueryTriggerInteraction.Collide))
		{
			OnHit(hitInfo);
		}
	}
}
