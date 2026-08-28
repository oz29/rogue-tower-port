using UnityEngine;

public class MorterShell : Projectile
{
	[SerializeField]
	private GameObject artObject;

	[SerializeField]
	private LayerMask layersAffectedByBlast;

	public float blastRadius = 1f;

	private Vector3 destination;

	private float timeOfFlight;

	[SerializeField]
	private float vSpeed;

	[SerializeField]
	private float hSpeed;

	[SerializeField]
	private float gravity = 5f;

	[SerializeField]
	private GameObject explosion;

	private float lookAhead;

	private Vector3 previousPos;

	public void SetMorterPhysics(Vector3 pos)
	{
		destination = pos;
		timeOfFlight = speed;
		vSpeed = gravity * timeOfFlight / 2f;
		hSpeed = Vector3.Magnitude(base.transform.position - destination) / timeOfFlight;
		lookAhead = vSpeed + hSpeed;
	}

	protected override void MoveProjectile()
	{
		previousPos = base.transform.position;
		base.transform.Translate(Vector3.forward * hSpeed * Time.fixedDeltaTime);
		base.transform.Translate(Vector3.up * vSpeed * Time.fixedDeltaTime);
		vSpeed -= gravity * Time.fixedDeltaTime;
		artObject.transform.rotation = Quaternion.LookRotation(base.transform.position - previousPos, Vector3.up);
	}

	protected override void CheckForHits()
	{
		if (!(vSpeed > 0f) && Physics.Raycast(artObject.transform.position, artObject.transform.forward, out var hitInfo, lookAhead * Time.fixedDeltaTime, layermask, QueryTriggerInteraction.Collide))
		{
			OnHit(hitInfo);
		}
	}

	protected override void OnHit(RaycastHit hit)
	{
		Collider[] array = Physics.OverlapSphere(base.transform.position, blastRadius, layersAffectedByBlast, QueryTriggerInteraction.Collide);
		for (int i = 0; i < array.Length; i++)
		{
			IDamageable component = array[i].GetComponent<IDamageable>();
			if (component != null)
			{
				DealDamage(component);
			}
		}
		if (detachOnDestruction != null)
		{
			detachOnDestruction.transform.parent = null;
		}
		if (extraFX != null)
		{
			extraFX.OnDetach();
		}
		Object.Instantiate(explosion, base.transform.position, Quaternion.identity);
		Object.Destroy(base.gameObject);
	}
}
