using System.Collections;
using UnityEngine;

public class Landmine : Projectile
{
	public float blastRadius = 0.5f;

	private bool armed;

	[SerializeField]
	private GameObject explosion;

	protected override void Start()
	{
		base.Start();
		SpawnManager.instance.destroyOnNewLevel.Add(base.gameObject);
	}

	public void SetEndPosition(Vector3 endPos)
	{
		StartCoroutine(ThrowMine(endPos));
	}

	protected override void FixedUpdate()
	{
	}

	private void OnTriggerEnter(Collider other)
	{
		if (armed && other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
		{
			Explode();
		}
	}

	private void Explode()
	{
		Collider[] array = Physics.OverlapSphere(base.transform.position, blastRadius, layermask, QueryTriggerInteraction.Collide);
		for (int i = 0; i < array.Length; i++)
		{
			IDamageable component = array[i].GetComponent<IDamageable>();
			if (component != null)
			{
				DealDamage(component);
			}
		}
		Object.Instantiate(explosion, base.transform.position, Quaternion.identity).transform.localScale = Vector3.one * 0.5f;
		SpawnManager.instance.destroyOnNewLevel.Remove(base.gameObject);
		Object.Destroy(base.gameObject);
	}

	private IEnumerator ThrowMine(Vector3 endPos)
	{
		Vector3 direction = endPos - base.transform.position;
		float throwTime = 1f;
		base.transform.localScale = Vector3.zero;
		for (float i = 0f; i < 1f; i += Time.deltaTime / throwTime)
		{
			base.transform.localScale = Vector3.one * i;
			base.transform.Translate(direction * Time.deltaTime / throwTime);
			yield return null;
		}
		base.transform.localScale = Vector3.one;
		base.transform.position = endPos;
		armed = true;
	}
}
