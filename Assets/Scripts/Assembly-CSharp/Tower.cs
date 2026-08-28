using UnityEngine;

public class Tower : MonoBehaviour, IBuildable
{
	public enum Priority
	{
		Progress = 0,
		NearDeath = 1,
		MostHealth = 2,
		MostArmor = 3,
		MostShield = 4,
		LeastHealth = 5,
		LeastArmor = 6,
		LeastShield = 7,
		Fastest = 8,
		Slowest = 9,
		Marked = 10
	}

	public TowerType towerType;

	public bool squareUI;

	[SerializeField]
	protected GameObject towerUI;

	public Priority[] priorities = new Priority[3];

	[SerializeField]
	protected GameObject turret;

	[SerializeField]
	protected bool towerVerticalyAims = true;

	[SerializeField]
	protected Transform muzzle;

	[SerializeField]
	protected GameObject projectile;

	[SerializeField]
	protected float projectileSpeed = 10f;

	[SerializeField]
	protected GameObject extraProjectileFX;

	[SerializeField]
	protected int baseDamage;

	[SerializeField]
	protected int baseHealthDamage;

	[SerializeField]
	protected int baseArmorDamage;

	[SerializeField]
	protected int baseShieldDamage;

	public float rpm;

	protected float rps;

	[SerializeField]
	protected float baseRange;

	[SerializeField]
	protected float baseSlowPercentage;

	[SerializeField]
	protected float baseBleedPercentage;

	[SerializeField]
	protected float baseBurnPercentage;

	[SerializeField]
	protected float basePoisonPercentage;

	[SerializeField]
	protected LayerMask enemyLayerMask;

	[SerializeField]
	protected LayerMask buildingLayerMask;

	public bool consumesMana;

	public float manaConsumptionRate;

	public float finalManaConsumption;

	public int upgradeCostMultiplier = 1;

	private int damageUpgrade;

	private int healthDamageUpgrade;

	private int armorDamageUpgrade;

	private int shieldDamageUpgrade;

	private int heightBonus;

	protected float timeSinceLastShot;

	private float lastTargetUpdate = 1f;

	protected GameObject currentTarget;

	[SerializeField]
	protected float baseBlastRadius;

	public int level { get; private set; }

	public float healthXP { get; private set; }

	public float armorXP { get; private set; }

	public float shieldXP { get; private set; }

	public int damage { get; private set; }

	public int healthDamage { get; private set; }

	public int armorDamage { get; private set; }

	public int shieldDamage { get; private set; }

	public float range { get; private set; }

	public float slowPercent { get; private set; }

	public float bleedPercent { get; private set; }

	public float burnPercent { get; private set; }

	public float poisonPercent { get; private set; }

	public float critChance { get; private set; }

	public float stunChance { get; private set; }

	public float blastRadius { get; private set; }

	protected virtual void Start()
	{
		level = 1;
		SetStats();
		TowerManager.instance.AddNewTower(this, towerType);
		DetectHouses();
		priorities[0] = (priorities[1] = (priorities[2] = Priority.Progress));
	}

	public virtual void SetStats()
	{
		heightBonus = (int)Mathf.Round(base.transform.position.y * 3f - 1f);
		damage = Mathf.Max(baseDamage + heightBonus + damageUpgrade + TowerManager.instance.GetBonusBaseDamage(towerType), 0);
		healthDamage = baseHealthDamage + healthDamageUpgrade + TowerManager.instance.GetBonusHealthDamage(towerType);
		armorDamage = baseArmorDamage + armorDamageUpgrade + TowerManager.instance.GetBonusArmorDamage(towerType);
		shieldDamage = baseShieldDamage + shieldDamageUpgrade + TowerManager.instance.GetBonusShieldDamage(towerType);
		rps = 60f / rpm;
		range = baseRange + (float)heightBonus / 2f + TowerManager.instance.GetBonusRange(towerType);
		slowPercent = baseSlowPercentage + TowerManager.instance.GetBonusSlow(towerType);
		bleedPercent = baseBleedPercentage + TowerManager.instance.GetBonusBleed(towerType);
		burnPercent = baseBurnPercentage + TowerManager.instance.GetBonusBurn(towerType);
		poisonPercent = basePoisonPercentage + TowerManager.instance.GetBonusPoison(towerType);
		blastRadius = baseBlastRadius + TowerManager.instance.GetBonusBlast(towerType);
		finalManaConsumption = manaConsumptionRate + TowerManager.instance.GetManaConsumptionBonus(towerType);
		if (TowerManager.instance.GetManaConsumptionBonus(towerType) > 0f)
		{
			consumesMana = true;
		}
		critChance = TowerManager.instance.GetCritChance(towerType) + TowerManager.instance.GetCritChanceLevelMultiplier(towerType) * (float)level / 100f;
		stunChance = TowerManager.instance.GetStunChance(towerType);
	}

	private void FixedUpdate()
	{
		if (lastTargetUpdate <= 0f)
		{
			DetectEnemies();
		}
		else
		{
			lastTargetUpdate -= Time.fixedDeltaTime;
		}
	}

	protected virtual void Update()
	{
		if (currentTarget != null)
		{
			if (turret != null)
			{
				AimTurret();
			}
			GainXP();
		}
		timeSinceLastShot += Time.deltaTime;
		if (currentTarget != null && timeSinceLastShot > rps)
		{
			Fire();
			timeSinceLastShot = 0f;
		}
	}

	private void DetectHouses()
	{
		if (Physics.Raycast(base.transform.position + new Vector3(1f, 1f, 0f), -base.transform.up, out var hitInfo, 1f, buildingLayerMask, QueryTriggerInteraction.Ignore))
		{
			CheckForHouse(hitInfo);
		}
		if (Physics.Raycast(base.transform.position + new Vector3(-1f, 1f, 0f), -base.transform.up, out hitInfo, 1f, buildingLayerMask, QueryTriggerInteraction.Ignore))
		{
			CheckForHouse(hitInfo);
		}
		if (Physics.Raycast(base.transform.position + new Vector3(0f, 1f, 1f), -base.transform.up, out hitInfo, 1f, buildingLayerMask, QueryTriggerInteraction.Ignore))
		{
			CheckForHouse(hitInfo);
		}
		if (Physics.Raycast(base.transform.position + new Vector3(0f, 1f, -1f), -base.transform.up, out hitInfo, 1f, buildingLayerMask, QueryTriggerInteraction.Ignore))
		{
			CheckForHouse(hitInfo);
		}
	}

	private void CheckForHouse(RaycastHit hit)
	{
		if (hit.collider.GetComponent<House>() != null && (double)Mathf.Abs(hit.collider.transform.position.y - base.transform.position.y) <= 0.001)
		{
			hit.collider.GetComponent<House>().AddDefender(this);
		}
	}

	public void TogglePriority(int index, int direction)
	{
		priorities[index] = (Priority)(((int)(priorities[index] + direction) % 11 + 11) % 11);
	}

	protected virtual void Fire()
	{
		if (consumesMana)
		{
			int manaCost = (int)((float)damage * finalManaConsumption);
			if (!ResourceManager.instance.CheckMana(manaCost))
			{
				return;
			}
			ResourceManager.instance.SpendMana(manaCost);
		}
		GameObject gameObject = Object.Instantiate(projectile, muzzle.position, muzzle.rotation);
		gameObject.GetComponent<Projectile>().SetStats(towerType, currentTarget, projectileSpeed, damage, healthDamage, armorDamage, shieldDamage, slowPercent, bleedPercent, burnPercent, poisonPercent, critChance, stunChance);
		if (extraProjectileFX != null)
		{
			GameObject gameObject2 = Object.Instantiate(extraProjectileFX, gameObject.transform.position, gameObject.transform.rotation);
			gameObject2.transform.SetParent(gameObject.transform);
			gameObject2.GetComponent<ProjectileFX>().SetFX(bleedPercent, burnPercent, poisonPercent, slowPercent, consumesMana);
			Projectile component = gameObject.GetComponent<Projectile>();
			if (component.detachOnDestruction == null)
			{
				component.detachOnDestruction = gameObject2;
				component.extraFX = gameObject2.GetComponent<ProjectileFX>();
			}
		}
	}

	protected virtual void AimTurret()
	{
		Vector3 forward = currentTarget.transform.position - turret.transform.position;
		if (!towerVerticalyAims)
		{
			forward.y = 0f;
		}
		Quaternion rotation = Quaternion.LookRotation(forward, Vector3.up);
		turret.transform.rotation = rotation;
	}

	protected void GainXP()
	{
		Enemy component = currentTarget.GetComponent<Enemy>();
		if (component != null)
		{
			if (component.shield > 0)
			{
				shieldXP += Time.deltaTime / (2f * range) + Time.deltaTime / 2f;
			}
			else if (component.armor > 0)
			{
				armorXP += Time.deltaTime / (2f * range) + Time.deltaTime / 2f;
			}
			else
			{
				healthXP += Time.deltaTime / (2f * range) + Time.deltaTime / 2f;
			}
			CheckXPAndLevelUp();
		}
	}

	private void CheckXPAndLevelUp()
	{
		if (healthXP >= (float)(10 * level))
		{
			healthXP -= 10 * level;
			level++;
			damageUpgrade++;
			healthDamageUpgrade++;
			SetStats();
			LevelUpText();
		}
		if (armorXP >= (float)(10 * level))
		{
			armorXP -= 10 * level;
			level++;
			damageUpgrade++;
			armorDamageUpgrade++;
			SetStats();
			LevelUpText();
		}
		if (shieldXP >= (float)(10 * level))
		{
			shieldXP -= 10 * level;
			level++;
			damageUpgrade++;
			shieldDamageUpgrade++;
			SetStats();
			LevelUpText();
		}
	}

	private void LevelUpText()
	{
		Object.Instantiate(BuildingManager.instance.levelUpFX, base.transform.position, Quaternion.identity);
		DamageNumber component = ObjectPool.instance.SpawnObject(ObjectPool.ObjectType.DamageNumber, base.transform.position, Quaternion.identity).GetComponent<DamageNumber>();
		component.SetText("LEVEL UP!", "Grey", 1f);
		component.SetHoldTime(1f);
		AchievementManager.instance.TowerLevel(level);
	}

	public void BuyHealthLevel()
	{
		int num = (10 * level - (int)healthXP) * upgradeCostMultiplier;
		if (ResourceManager.instance.CheckMoney(num))
		{
			ResourceManager.instance.Spend(num);
			DamageTracker.instance.AddCost(towerType, num);
			healthXP -= 10 * level - num / upgradeCostMultiplier;
			level++;
			damageUpgrade++;
			healthDamageUpgrade++;
			SetStats();
			LevelUpText();
		}
	}

	public void BuyArmorLevel()
	{
		int num = (10 * level - (int)armorXP) * upgradeCostMultiplier;
		if (ResourceManager.instance.CheckMoney(num))
		{
			ResourceManager.instance.Spend(num);
			DamageTracker.instance.AddCost(towerType, num);
			armorXP -= 10 * level - num / upgradeCostMultiplier;
			level++;
			damageUpgrade++;
			armorDamageUpgrade++;
			SetStats();
			LevelUpText();
		}
	}

	public void BuyShieldLevel()
	{
		int num = (10 * level - (int)shieldXP) * upgradeCostMultiplier;
		if (ResourceManager.instance.CheckMoney(num))
		{
			ResourceManager.instance.Spend(num);
			DamageTracker.instance.AddCost(towerType, num);
			shieldXP -= 10 * level - num / upgradeCostMultiplier;
			level++;
			damageUpgrade++;
			shieldDamageUpgrade++;
			SetStats();
			LevelUpText();
		}
	}

	private void DetectEnemies()
	{
		Collider[] array = Physics.OverlapSphere(base.transform.position, range, enemyLayerMask, QueryTriggerInteraction.Collide);
		if (array.Length != 0)
		{
			currentTarget = SelectEnemy(array);
		}
		else
		{
			currentTarget = null;
		}
		lastTargetUpdate = Random.Range(0.25f, 1f);
	}

	protected bool CheckPriority(Priority check)
	{
		for (int i = 0; i < priorities.Length; i++)
		{
			if (check == priorities[i])
			{
				return true;
			}
		}
		return false;
	}

	protected int PriorityScale(Priority check)
	{
		for (int i = 0; i < priorities.Length; i++)
		{
			if (check == priorities[i])
			{
				return Mathf.Clamp(3 - i, 1, 3);
			}
		}
		return 1;
	}

	protected virtual GameObject SelectEnemy(Collider[] possibleTargets)
	{
		GameObject result = null;
		float num = -1f;
		for (int i = 0; i < possibleTargets.Length; i++)
		{
			float num2 = 1f;
			Enemy component = possibleTargets[i].GetComponent<Enemy>();
			if (CheckPriority(Priority.Progress))
			{
				float f = Mathf.Max(0.001f, possibleTargets[i].GetComponent<Pathfinder>().distanceFromEnd);
				num2 /= Mathf.Pow(f, PriorityScale(Priority.Progress));
			}
			if (CheckPriority(Priority.NearDeath))
			{
				float f2 = Mathf.Max(0.001f, component.CurrentHealth());
				num2 /= Mathf.Pow(f2, PriorityScale(Priority.NearDeath));
			}
			if (CheckPriority(Priority.MostHealth))
			{
				float f3 = (float)Mathf.Max(1, component.health) / 1000f;
				num2 *= Mathf.Pow(f3, PriorityScale(Priority.MostHealth));
			}
			if (CheckPriority(Priority.MostArmor))
			{
				float f4 = (float)Mathf.Max(1, component.armor) / 1000f;
				num2 *= Mathf.Pow(f4, PriorityScale(Priority.MostArmor));
			}
			if (CheckPriority(Priority.MostShield))
			{
				float f5 = (float)Mathf.Max(1, component.shield) / 1000f;
				num2 *= Mathf.Pow(f5, PriorityScale(Priority.MostShield));
			}
			if (CheckPriority(Priority.LeastHealth))
			{
				float f6 = Mathf.Max(1, component.health);
				num2 /= Mathf.Pow(f6, PriorityScale(Priority.LeastHealth));
			}
			if (CheckPriority(Priority.LeastArmor))
			{
				float f7 = Mathf.Max(1, component.armor);
				num2 /= Mathf.Pow(f7, PriorityScale(Priority.LeastArmor));
			}
			if (CheckPriority(Priority.LeastShield))
			{
				float f8 = Mathf.Max(1, component.shield);
				num2 /= Mathf.Pow(f8, PriorityScale(Priority.LeastShield));
			}
			if (CheckPriority(Priority.Fastest))
			{
				float f9 = Mathf.Max(0.001f, possibleTargets[i].GetComponent<Pathfinder>().speed);
				num2 *= Mathf.Pow(f9, PriorityScale(Priority.Fastest));
			}
			if (CheckPriority(Priority.Slowest))
			{
				float f10 = Mathf.Max(0.001f, possibleTargets[i].GetComponent<Pathfinder>().speed);
				num2 /= Mathf.Pow(f10, PriorityScale(Priority.Slowest));
			}
			if (CheckPriority(Priority.Marked))
			{
				float f11 = 1f;
				if (component.mark != null)
				{
					f11 = (float)(component.mark.damage * (component.mark.healthDamage + component.mark.armorDamage + component.mark.shieldDamage)) * (1f + component.mark.critChance);
				}
				num2 *= Mathf.Pow(f11, PriorityScale(Priority.Marked));
			}
			if (num2 > num)
			{
				result = component.gameObject;
				num = num2;
			}
		}
		return result;
	}

	public void SpawnUI()
	{
		Object.Instantiate(towerUI, base.transform.position, Quaternion.identity).GetComponent<TowerUI>().SetStats(this);
	}

	public virtual void Demolish()
	{
		TowerManager.instance.RemoveTower(this, towerType);
		Object.Destroy(base.gameObject);
	}
}
