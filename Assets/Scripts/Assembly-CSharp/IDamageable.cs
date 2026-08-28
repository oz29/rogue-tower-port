public interface IDamageable
{
	void TakeDamage(TowerType whoHitMe, int baseDmg, int healthDmg, int armorDmg, int shieldDmg, float slowPercentage, float bleedPercentage, float burnPercentage, float poisonPercentage, float critChance, float stunChance);
}
