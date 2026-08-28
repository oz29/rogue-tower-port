using UnityEngine;

public class Encampment : Dropper
{
	protected override void Fire()
	{
		if (consumesMana)
		{
			int manaCost = (int)((float)base.damage * manaConsumptionRate);
			if (!ResourceManager.instance.CheckMana(manaCost))
			{
				return;
			}
			ResourceManager.instance.SpendMana(manaCost);
		}
		if (dropPoints.Length != 0)
		{
			Vector2 vector = dropPoints[Random.Range(0, dropPoints.Length)];
			Vector3 endPosition = new Vector3(vector.x + Random.Range(-0.125f, 0.125f) + base.transform.position.x, 0f, vector.y + Random.Range(-0.125f, 0.125f) + base.transform.position.z);
			GameObject obj = Object.Instantiate(projectile, base.transform.position, Quaternion.identity);
			obj.GetComponent<Projectile>().SetStats(towerType, null, projectileSpeed, base.damage, base.healthDamage, base.armorDamage, base.shieldDamage, base.slowPercent, base.bleedPercent, base.burnPercent, base.poisonPercent, base.critChance, base.stunChance);
			obj.GetComponent<Landmine>().blastRadius = base.blastRadius;
			obj.GetComponent<Landmine>().SetEndPosition(endPosition);
		}
	}
}
