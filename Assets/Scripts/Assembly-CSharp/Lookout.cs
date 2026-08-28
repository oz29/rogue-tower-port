using UnityEngine;

public class Lookout : Tower
{
	[SerializeField]
	private GameObject markIcon;

	private GameObject currentMark;

	protected override void Update()
	{
		if (currentTarget != null)
		{
			markIcon.SetActive(value: true);
			UpdateMark();
			GainXP();
		}
		else
		{
			markIcon.SetActive(value: false);
		}
	}

	private void UpdateMark()
	{
		if (currentTarget != currentMark)
		{
			if (currentMark != null)
			{
				currentMark.GetComponent<Enemy>().mark = null;
			}
			currentMark = currentTarget;
			currentMark.GetComponent<Enemy>().mark = this;
		}
		markIcon.transform.position = currentMark.transform.position;
	}

	protected override GameObject SelectEnemy(Collider[] possibleTargets)
	{
		GameObject result = null;
		float num = -1f;
		for (int i = 0; i < possibleTargets.Length; i++)
		{
			float num2 = 1f;
			Enemy component = possibleTargets[i].GetComponent<Enemy>();
			if (!(component.mark != this) || !(component.mark != null))
			{
				if (CheckPriority(Priority.Progress))
				{
					num2 /= Mathf.Max(0.001f, possibleTargets[i].GetComponent<Pathfinder>().distanceFromEnd);
				}
				if (CheckPriority(Priority.NearDeath))
				{
					num2 /= Mathf.Max(0.001f, component.CurrentHealth());
				}
				if (CheckPriority(Priority.MostHealth))
				{
					num2 *= (float)Mathf.Max(1, component.health);
				}
				if (CheckPriority(Priority.MostArmor))
				{
					num2 *= (float)Mathf.Max(1, component.armor);
				}
				if (CheckPriority(Priority.MostShield))
				{
					num2 *= (float)Mathf.Max(1, component.shield);
				}
				if (CheckPriority(Priority.LeastHealth))
				{
					num2 /= (float)Mathf.Max(1, component.health);
				}
				if (CheckPriority(Priority.LeastArmor))
				{
					num2 /= (float)Mathf.Max(1, component.armor);
				}
				if (CheckPriority(Priority.LeastShield))
				{
					num2 /= (float)Mathf.Max(1, component.shield);
				}
				if (CheckPriority(Priority.Fastest))
				{
					num2 *= Mathf.Max(0.001f, possibleTargets[i].GetComponent<Pathfinder>().speed);
				}
				if (CheckPriority(Priority.Slowest))
				{
					num2 /= Mathf.Max(0.001f, possibleTargets[i].GetComponent<Pathfinder>().speed);
				}
				if (num2 > num)
				{
					result = component.gameObject;
					num = num2;
				}
			}
		}
		return result;
	}
}
