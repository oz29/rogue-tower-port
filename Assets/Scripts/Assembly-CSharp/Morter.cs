using UnityEngine;

public class Morter : Tower
{
	protected override void AimTurret()
	{
	}

	protected override void Fire()
	{
		if (consumesMana)
		{
			int manaCost = (int)((float)base.damage * finalManaConsumption);
			if (!ResourceManager.instance.CheckMana(manaCost))
			{
				return;
			}
			ResourceManager.instance.SpendMana(manaCost);
		}
		float num = projectileSpeed * Mathf.Clamp(Vector3.SqrMagnitude(currentTarget.transform.position - base.transform.position) / (2f * baseRange * baseRange), 1f, float.MaxValue);
		Vector3 vector = currentTarget.GetComponent<Enemy>().GetFuturePosition(num) - turret.transform.position;
		GameObject gameObject = Object.Instantiate(rotation: Quaternion.LookRotation(new Vector3(vector.x, 0f, vector.z), Vector3.up), original: projectile, position: muzzle.position);
		gameObject.GetComponent<Projectile>().SetStats(towerType, currentTarget, num, base.damage, base.healthDamage, base.armorDamage, base.shieldDamage, base.slowPercent, base.bleedPercent, base.burnPercent, base.poisonPercent, base.critChance, base.stunChance);
		gameObject.GetComponent<MorterShell>().SetMorterPhysics(currentTarget.GetComponent<Enemy>().GetFuturePosition(num));
		gameObject.GetComponent<MorterShell>().blastRadius = base.blastRadius;
		if (extraProjectileFX != null)
		{
			GameObject gameObject2 = Object.Instantiate(extraProjectileFX, gameObject.transform.position, gameObject.transform.rotation);
			gameObject2.transform.SetParent(gameObject.transform);
			gameObject2.GetComponent<ProjectileFX>().SetFX(base.bleedPercent, base.burnPercent, base.poisonPercent, base.slowPercent, consumesMana);
			Projectile component = gameObject.GetComponent<Projectile>();
			if (component.detachOnDestruction == null)
			{
				component.detachOnDestruction = gameObject2;
				component.extraFX = gameObject2.GetComponent<ProjectileFX>();
			}
		}
	}
}
