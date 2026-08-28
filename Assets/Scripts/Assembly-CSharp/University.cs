using UnityEngine;

public class University : MonoBehaviour, IBuildable
{
	[SerializeField]
	private GameObject UIObject;

	[SerializeField]
	private GameObject mainUIObject;

	public int goldBackOnDemolish;

	[SerializeField]
	private LayerMask layermask;

	private bool active;

	public int healthPercent { get; private set; }

	public int armorPercent { get; private set; }

	public int shieldPercent { get; private set; }

	public int healthGained { get; private set; }

	public int armorGained { get; private set; }

	public int shieldGained { get; private set; }

	private void Start()
	{
		SpawnManager.instance.universities.Add(this);
		DetectShrines();
	}

	public void SetStats()
	{
	}

	public void Research()
	{
		if (active)
		{
			if (Random.Range(0f, 100f) <= (float)(healthPercent + GameManager.instance.universityBonus))
			{
				healthGained++;
				TowerManager.instance.AddBonusHealthDamage(TowerType.Global, 1);
				UniBonusUI.instance.UniBonus(1, 0, 0);
				DamageNumber component = ObjectPool.instance.SpawnObject(ObjectPool.ObjectType.DamageNumber, base.transform.position, Quaternion.identity).GetComponent<DamageNumber>();
				component.SetText("New Discoveries!", "Grey", 1f);
				component.SetHoldTime(2f);
				AchievementManager.instance.NewDiscoveriesAchievement();
			}
			if (Random.Range(0f, 100f) <= (float)(armorPercent + GameManager.instance.universityBonus))
			{
				armorGained++;
				UniBonusUI.instance.UniBonus(0, 1, 0);
				TowerManager.instance.AddBonusArmorDamage(TowerType.Global, 1);
				DamageNumber component2 = ObjectPool.instance.SpawnObject(ObjectPool.ObjectType.DamageNumber, base.transform.position, Quaternion.identity).GetComponent<DamageNumber>();
				component2.SetText("New Discoveries!", "Grey", 1f);
				component2.SetHoldTime(2f);
				AchievementManager.instance.NewDiscoveriesAchievement();
			}
			if (Random.Range(0f, 100f) <= (float)(shieldPercent + GameManager.instance.universityBonus))
			{
				shieldGained++;
				UniBonusUI.instance.UniBonus(0, 0, 1);
				TowerManager.instance.AddBonusShieldDamage(TowerType.Global, 1);
				DamageNumber component3 = ObjectPool.instance.SpawnObject(ObjectPool.ObjectType.DamageNumber, base.transform.position, Quaternion.identity).GetComponent<DamageNumber>();
				component3.SetText("New Discoveries!", "Grey", 1f);
				component3.SetHoldTime(2f);
				AchievementManager.instance.NewDiscoveriesAchievement();
			}
		}
	}

	public void FundHealthStudies()
	{
		int num = (healthPercent + armorPercent + shieldPercent + 1) * 20;
		if (ResourceManager.instance.CheckMoney(num))
		{
			healthPercent++;
			ResourceManager.instance.Spend(num);
			AchievementManager.instance.FundResearchAchievement(num);
		}
	}

	public void FundArmorStudies()
	{
		int num = (healthPercent + armorPercent + shieldPercent + 1) * 20;
		if (ResourceManager.instance.CheckMoney(num))
		{
			armorPercent++;
			ResourceManager.instance.Spend(num);
			AchievementManager.instance.FundResearchAchievement(num);
		}
	}

	public void FundShieldStudies()
	{
		int num = (healthPercent + armorPercent + shieldPercent + 1) * 20;
		if (ResourceManager.instance.CheckMoney(num))
		{
			shieldPercent++;
			ResourceManager.instance.Spend(num);
			AchievementManager.instance.FundResearchAchievement(num);
		}
	}

	public void SpawnUI()
	{
		if (!active)
		{
			Object.Instantiate(UIObject, base.transform.position, Quaternion.identity).GetComponent<SimpleUI>().SetDemolishable(base.gameObject, goldBackOnDemolish);
		}
		else
		{
			Object.Instantiate(mainUIObject, base.transform.position, Quaternion.identity).GetComponent<UniversityUI>().SetStats(this);
		}
	}

	public void Demolish()
	{
		SpawnManager.instance.universities.Remove(this);
	}

	private void DetectShrines()
	{
		if ((!Physics.Raycast(base.transform.position + new Vector3(1f, 1f, 0f), -base.transform.up, out var hitInfo, 1f, layermask, QueryTriggerInteraction.Ignore) || !Rotate(hitInfo)) && (!Physics.Raycast(base.transform.position + new Vector3(-1f, 1f, 0f), -base.transform.up, out hitInfo, 1f, layermask, QueryTriggerInteraction.Ignore) || !Rotate(hitInfo)) && (!Physics.Raycast(base.transform.position + new Vector3(0f, 1f, 1f), -base.transform.up, out hitInfo, 1f, layermask, QueryTriggerInteraction.Ignore) || !Rotate(hitInfo)) && Physics.Raycast(base.transform.position + new Vector3(0f, 1f, -1f), -base.transform.up, out hitInfo, 1f, layermask, QueryTriggerInteraction.Ignore))
		{
			Rotate(hitInfo);
		}
	}

	private bool Rotate(RaycastHit hit)
	{
		if (hit.collider.GetComponent<Shrine>() == null)
		{
			return false;
		}
		if ((double)(hit.collider.transform.position.y - base.transform.position.y) > 0.001)
		{
			return false;
		}
		base.transform.LookAt(hit.collider.transform.position, Vector3.up);
		active = true;
		SetStats();
		return true;
	}
}
