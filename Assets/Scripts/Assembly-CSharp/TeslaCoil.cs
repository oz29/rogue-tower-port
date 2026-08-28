using System;
using System.Collections;
using UnityEngine;

public class TeslaCoil : Tower
{
	[SerializeField]
	private GameObject lightningTrail;

	protected override void AimTurret()
	{
	}

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
		Vector3 position = base.transform.position;
		position.y = 0f;
		Collider[] array = Physics.OverlapSphere(position, base.range, enemyLayerMask, QueryTriggerInteraction.Collide);
		for (int i = 0; i < array.Length; i++)
		{
			array[i].GetComponent<IDamageable>()?.TakeDamage(towerType, base.damage, base.healthDamage, base.armorDamage, base.shieldDamage, base.slowPercent, base.bleedPercent, base.burnPercent, base.poisonPercent, base.critChance, base.stunChance);
		}
		SFXManager.instance.PlaySound(Sound.TeslaZap, base.transform.position);
		StartCoroutine(DrawLightning());
	}

	private IEnumerator DrawLightning()
	{
		for (float i = 0f; i <= (float)Math.PI * 2f; i += 0.5f)
		{
			Vector3 vector = new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f));
			lightningTrail.transform.position = vector + new Vector3(base.transform.position.x + Mathf.Sin(i) * base.range, 0.75f, base.transform.position.z + Mathf.Cos(i) * base.range);
			yield return new WaitForEndOfFrame();
		}
	}
}
