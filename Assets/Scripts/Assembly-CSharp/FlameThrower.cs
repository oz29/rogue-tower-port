using UnityEngine;

public class FlameThrower : Tower
{
	[SerializeField]
	private ParticleSystem flames;

	public bool flaming;

	private bool hasMana = true;

	[SerializeField]
	private AudioSource audioS;

	private bool soundPlaying;

	protected override void Update()
	{
		if (hasMana && currentTarget != null && !flaming)
		{
			flames.Play();
			PlaySound(onOff: true);
			flaming = true;
		}
		else if (!hasMana || (currentTarget == null && flaming))
		{
			flames.Stop();
			PlaySound(onOff: false);
			flaming = false;
		}
		base.Update();
	}

	protected override void Fire()
	{
		if (consumesMana)
		{
			int manaCost = (int)((float)base.damage * manaConsumptionRate);
			if (!ResourceManager.instance.CheckMana(manaCost))
			{
				hasMana = false;
				return;
			}
			ResourceManager.instance.SpendMana(manaCost);
			hasMana = true;
		}
		RaycastHit[] array = Physics.SphereCastAll(muzzle.position, base.range / 6f, muzzle.transform.forward, base.range * 1.5f, enemyLayerMask, QueryTriggerInteraction.Collide);
		foreach (RaycastHit raycastHit in array)
		{
			raycastHit.collider.GetComponent<IDamageable>()?.TakeDamage(towerType, base.damage, base.healthDamage, base.armorDamage, base.shieldDamage, base.slowPercent, base.bleedPercent, base.burnPercent, base.poisonPercent, base.critChance, base.stunChance);
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
