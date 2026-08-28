using System.Collections;
using UnityEngine;

public class ProjectileFX : MonoBehaviour
{
	[SerializeField]
	private GameObject bleedingPS;

	[SerializeField]
	private GameObject burrningPS;

	[SerializeField]
	private GameObject poisonPS;

	public void SetFX(float bleeding, float burning, float poison, float slow, bool arcane)
	{
		if (OptionsMenu.instance.extraProjectileEffects)
		{
			if (bleeding > 0f)
			{
				bleedingPS.SetActive(value: true);
			}
			if (burning > 0f)
			{
				burrningPS.SetActive(value: true);
			}
			if (poison > 0f)
			{
				poisonPS.SetActive(value: true);
			}
		}
	}

	public void OnDetach()
	{
		StartCoroutine(Die());
	}

	private IEnumerator Die()
	{
		bleedingPS.GetComponent<ParticleSystem>().Stop(withChildren: true);
		burrningPS.GetComponent<ParticleSystem>().Stop(withChildren: true);
		poisonPS.GetComponent<ParticleSystem>().Stop(withChildren: true);
		yield return new WaitForSeconds(1.1f);
		Object.Destroy(base.gameObject);
	}
}
