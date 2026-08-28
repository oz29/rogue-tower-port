using UnityEngine;

public class ParticleBeam : Projectile
{
	[SerializeField]
	private GameObject beamTrail;

	protected override void MoveProjectile()
	{
		base.MoveProjectile();
		beamTrail.transform.position = base.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), Random.Range(-1f, 1f));
	}

	protected new virtual void OnHit(RaycastHit hit)
	{
		beamTrail.transform.position = hit.point;
		base.OnHit(hit);
	}
}
