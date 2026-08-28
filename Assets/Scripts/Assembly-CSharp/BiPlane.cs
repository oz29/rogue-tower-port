using UnityEngine;

public class BiPlane : MonoBehaviour
{
	[SerializeField]
	private TowerType towerType;

	[SerializeField]
	private float speed;

	[SerializeField]
	private float turnTime = 3f;

	[SerializeField]
	private float minDistance = 3f;

	[SerializeField]
	private float maxDistance = 12f;

	[SerializeField]
	private float attackDistance = 6f;

	public GameObject target;

	private int directionMultiplier = 1;

	private Vector3 previousPos;

	private bool flyingTowards = true;

	private float desiredAltitude = 5f;

	private float desiredTilt;

	public int damage;

	public int healthDamage;

	public int armorDamage;

	public int shieldDamage;

	public float rps;

	public float slowPercent;

	public float bleedPercent;

	public float burnPercent;

	public float poisonPercent;

	public float critChance;

	public float stunChance;

	private int ammo;

	[SerializeField]
	private int maxAmmo = 20;

	private float timeOfLastShot;

	[SerializeField]
	private LayerMask bulletHitMask;

	[SerializeField]
	private GameObject projectile;

	[SerializeField]
	private float projectileSpeed;

	[SerializeField]
	private Transform muzzle;

	[SerializeField]
	private Transform artTransform;

	private void Start()
	{
		ammo = maxAmmo;
	}

	private void Update()
	{
		FlightPath();
	}

	private void FixedUpdate()
	{
	}

	private void FlightPath()
	{
		Vector3 vector;
		if (target != null)
		{
			vector = target.transform.position;
		}
		else
		{
			vector = Vector3.zero;
			vector.y = 5f;
		}
		if (Vector3.SqrMagnitude(vector - base.transform.position) <= minDistance * minDistance)
		{
			flyingTowards = false;
			Reload();
		}
		else if (Vector3.SqrMagnitude(vector - base.transform.position) >= maxDistance * maxDistance)
		{
			flyingTowards = true;
		}
		float num = ((!flyingTowards) ? 0f : Vector2.SignedAngle(new Vector2(base.transform.forward.x, base.transform.forward.z), new Vector2(vector.x - base.transform.position.x, vector.z - base.transform.position.z)));
		float num2;
		if (target != null && flyingTowards && Vector3.SqrMagnitude(vector - base.transform.position) <= attackDistance * attackDistance)
		{
			desiredAltitude += (Mathf.Min(vector.y, 1f) - desiredAltitude) * Time.deltaTime * 1.5f;
			num2 = base.transform.position.y - desiredAltitude;
			if (Mathf.Abs(num) < 22.5f)
			{
				Fire();
			}
		}
		else
		{
			desiredAltitude += (5f - desiredAltitude) * Time.deltaTime * 3f;
			num2 = base.transform.position.y - desiredAltitude;
		}
		num = Mathf.Clamp(num * 6f, -90f, 90f);
		num2 = Mathf.Clamp(num2 * 6f, -45f, 45f);
		base.transform.Rotate(new Vector3(0f, (0f - num) * Time.deltaTime / turnTime, 0f));
		base.transform.eulerAngles = new Vector3(num2, base.transform.eulerAngles.y, 0f);
		base.transform.Translate(Vector3.forward * Time.deltaTime * speed);
		desiredTilt += (num - desiredTilt) * Time.deltaTime;
		artTransform.eulerAngles = new Vector3(base.transform.eulerAngles.x, base.transform.eulerAngles.y, desiredTilt);
	}

	private void Reload()
	{
		ammo = maxAmmo;
	}

	private void Fire()
	{
		if (ammo > 0 && timeOfLastShot + rps <= Time.time)
		{
			ammo--;
			timeOfLastShot = Time.time;
			LaunchProjectile();
		}
	}

	private void LaunchProjectile()
	{
		Vector3 position = target.transform.position;
		position += new Vector3(Random.Range(-0.33f, 0.33f) + Random.Range(-0.33f, 0.33f), Random.Range(-0.33f, 0.33f) + Random.Range(-0.33f, 0.33f), Random.Range(-0.33f, 0.33f) + Random.Range(-0.33f, 0.33f));
		muzzle.LookAt(position);
		Object.Instantiate(projectile, muzzle.position, muzzle.rotation).GetComponent<Projectile>().SetStats(towerType, target, projectileSpeed, damage, healthDamage, armorDamage, shieldDamage, slowPercent, bleedPercent, burnPercent, poisonPercent, critChance, stunChance);
	}
}
