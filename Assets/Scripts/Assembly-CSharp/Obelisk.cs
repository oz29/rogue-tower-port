using UnityEngine;

public class Obelisk : Tower
{
	[SerializeField]
	private GameObject beam;

	[SerializeField]
	private AudioSource audioS;

	private bool soundPlaying;

	private IDamageable lastThingIHit;

	private float timeOnTarget;

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
		else
		{
			beam.SetActive(value: false);
			PlaySound(onOff: false);
		}
		timeSinceLastShot += Time.deltaTime;
		if (currentTarget != null && timeSinceLastShot > rps)
		{
			Fire();
			timeSinceLastShot = 0f;
		}
	}

	protected override void Fire()
	{
		if (consumesMana)
		{
			int manaCost = (int)((float)base.damage * manaConsumptionRate);
			if (!ResourceManager.instance.CheckMana(manaCost))
			{
				beam.SetActive(value: false);
				PlaySound(onOff: false);
				return;
			}
			ResourceManager.instance.SpendMana(manaCost);
		}
		DealDamage();
	}

	protected override void AimTurret()
	{
		Vector3 forward = currentTarget.transform.position - turret.transform.position + Vector3.up * 0.5f;
		Quaternion rotation = Quaternion.LookRotation(forward, Vector3.up);
		turret.transform.rotation = rotation;
		beam.transform.localScale = new Vector3(1f, 1f, forward.magnitude);
	}

	private void DealDamage()
	{
		IDamageable component = currentTarget.GetComponent<IDamageable>();
		if (component != null)
		{
			if (component == lastThingIHit)
			{
				timeOnTarget += rps;
			}
			else
			{
				timeOnTarget = 0f;
				lastThingIHit = component;
			}
			int num = Mathf.Clamp(Mathf.FloorToInt(timeOnTarget * TowerManager.instance.obeliskTimeOnTargetMultiplier), 0, base.damage);
			component.TakeDamage(towerType, base.damage + num, base.healthDamage, base.armorDamage, base.shieldDamage, base.slowPercent, base.bleedPercent, base.burnPercent, base.poisonPercent, base.critChance, base.stunChance);
			beam.SetActive(value: true);
			PlaySound(onOff: true);
		}
	}

	private void PlaySound(bool onOff)
	{
		if (onOff && !soundPlaying)
		{
			audioS.volume = OptionsMenu.instance.masterVolume * OptionsMenu.instance.sfxVolume;
			audioS.Play();
			soundPlaying = true;
		}
		else if (!onOff && soundPlaying)
		{
			audioS.Stop();
			soundPlaying = false;
		}
	}
}
