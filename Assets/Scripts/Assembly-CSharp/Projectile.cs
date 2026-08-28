using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField]
	protected Sound launchSound;

	[SerializeField]
	protected Sound hitSound;

	protected TowerType shotBy;

	[SerializeField]
	protected LayerMask layermask;

	[SerializeField]
	protected float speed;

	[SerializeField]
	protected bool homing;

	[SerializeField]
	protected float maximumLifeTime = 10f;

	[SerializeField]
	public GameObject detachOnDestruction;

	[SerializeField]
	public ProjectileFX extraFX;

	protected GameObject target;

	protected int damage;

	protected int healthDamage;

	protected int armorDamage;

	protected int shieldDamage;

	protected float slowPercent;

	protected float bleedPercent;

	protected float burnPercent;

	protected float poisonPercent;

	protected float critChance;

	protected float stunChance;

	protected virtual void Start()
	{
		if (launchSound != Sound.None)
		{
			SFXManager.instance.PlaySound(launchSound, base.transform.position);
		}
	}

	protected virtual void FixedUpdate()
	{
		maximumLifeTime -= Time.fixedDeltaTime;
		if (maximumLifeTime < 0f)
		{
			Object.Destroy(base.gameObject);
		}
		CheckForHits();
		MoveProjectile();
		if (homing)
		{
			AlterCourse();
		}
	}

	public virtual void SetStats(TowerType whoShotMe, GameObject _target, float spd, int dmg, int healthDmg, int armorDmg, int shieldDmg, float slow, float bleed, float burn, float poison, float crit, float stun)
	{
		shotBy = whoShotMe;
		target = _target;
		speed = spd;
		damage = dmg;
		healthDamage = healthDmg;
		armorDamage = armorDmg;
		shieldDamage = shieldDmg;
		slowPercent = slow;
		bleedPercent = bleed;
		burnPercent = burn;
		poisonPercent = poison;
		critChance = crit;
		stunChance = stun;
	}

	protected virtual void MoveProjectile()
	{
		base.transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);
	}

	protected virtual void AlterCourse()
	{
		if (!(target == null))
		{
			Quaternion rotation = Quaternion.LookRotation(0.25f * Vector3.up + target.transform.position - base.transform.position, Vector3.up);
			base.transform.rotation = rotation;
		}
	}

	protected virtual void CheckForHits()
	{
		if (Physics.Raycast(base.transform.position, base.transform.forward, out var hitInfo, speed * Time.fixedDeltaTime, layermask, QueryTriggerInteraction.Collide))
		{
			OnHit(hitInfo);
		}
	}

	protected virtual void OnHit(RaycastHit hit)
	{
		IDamageable component = hit.transform.GetComponent<IDamageable>();
		if (component != null)
		{
			DealDamage(component);
		}
		if (detachOnDestruction != null)
		{
			detachOnDestruction.transform.parent = null;
		}
		if (extraFX != null)
		{
			extraFX.OnDetach();
		}
		Object.Destroy(base.gameObject);
	}

	protected virtual void DealDamage(IDamageable target)
	{
		target.TakeDamage(shotBy, damage, healthDamage, armorDamage, shieldDamage, slowPercent, bleedPercent, burnPercent, poisonPercent, critChance, stunChance);
		if (hitSound != Sound.None)
		{
			SFXManager.instance.PlaySound(hitSound, base.transform.position);
		}
	}
}
