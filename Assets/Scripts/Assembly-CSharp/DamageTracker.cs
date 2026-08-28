using UnityEngine;
using UnityEngine.UI;

public class DamageTracker : MonoBehaviour
{
	public enum IncomeType
	{
		Monster = 0,
		House = 1,
		Haunted = 2,
		Bonus = 3
	}

	public static DamageTracker instance;

	public GameObject uiObject;

	[SerializeField]
	private Text healthDmg;

	[SerializeField]
	private Text armorDmg;

	[SerializeField]
	private Text shieldDmg;

	[SerializeField]
	private Text totalDmg;

	[SerializeField]
	private Text cost;

	[SerializeField]
	private Text ratio;

	[SerializeField]
	private Text incomeTotals;

	[SerializeField]
	private Text incomePercent;

	private Vector3[] towerDamage = new Vector3[15];

	private int[] towerCost = new int[15];

	private int[] income = new int[4];

	private void Awake()
	{
		instance = this;
	}

	public void AddDamage(TowerType type, int healthDmg, int armorDmg, int shieldDmg)
	{
		towerDamage[(int)type].x += (float)healthDmg / 1000f;
		towerDamage[(int)type].y += (float)armorDmg / 1000f;
		towerDamage[(int)type].z += (float)shieldDmg / 1000f;
	}

	public void AddCost(TowerType type, int gold)
	{
		towerCost[(int)type] += gold;
	}

	public void AddIncome(IncomeType type, int gold)
	{
		income[(int)type] += gold;
	}

	public void DisplayIncomeTotals()
	{
		float num = 0f;
		for (int i = 0; i < income.Length; i++)
		{
			num += (float)income[i];
		}
		num = Mathf.Max(num, 1f);
		string text = "Gold Generated\n" + income[0] + "g\n" + income[1] + "g\n" + income[2] + "g\n" + income[3] + "g\n";
		incomeTotals.text = text;
		string text2 = "Income Share\n" + Mathf.FloorToInt((float)(100 * income[0]) / num) + "%\n" + Mathf.FloorToInt((float)(100 * income[1]) / num) + "%\n" + Mathf.FloorToInt((float)(100 * income[2]) / num) + "%\n" + Mathf.FloorToInt((float)(100 * income[3]) / num) + "%\n";
		incomePercent.text = text2;
	}

	public void DisplayDamageTotals()
	{
		uiObject.SetActive(value: true);
		DisplayIncomeTotals();
		string text = "Health\n" + (int)towerDamage[0].x + "K\n" + (int)towerDamage[1].x + "K\n" + (int)towerDamage[2].x + "K\n" + (int)towerDamage[3].x + "K\n" + (int)towerDamage[5].x + "K\n" + (int)towerDamage[6].x + "K\n" + (int)towerDamage[12].x + "K\n" + (int)towerDamage[13].x + "K\n" + (int)towerDamage[14].x + "K\n" + (int)towerDamage[8].x + "K\n" + (int)towerDamage[4].x + "K\n" + (int)towerDamage[9].x + "K\n" + (int)towerDamage[10].x + "K\n";
		healthDmg.text = text;
		string text2 = "Armor\n" + (int)towerDamage[0].y + "K\n" + (int)towerDamage[1].y + "K\n" + (int)towerDamage[2].y + "K\n" + (int)towerDamage[3].y + "K\n" + (int)towerDamage[5].y + "K\n" + (int)towerDamage[6].y + "K\n" + (int)towerDamage[12].y + "K\n" + (int)towerDamage[13].y + "K\n" + (int)towerDamage[14].y + "K\n" + (int)towerDamage[8].y + "K\n" + (int)towerDamage[4].y + "K\n" + (int)towerDamage[9].y + "K\n" + (int)towerDamage[10].y + "K\n";
		armorDmg.text = text2;
		string text3 = "Shield\n" + (int)towerDamage[0].z + "K\n" + (int)towerDamage[1].z + "K\n" + (int)towerDamage[2].z + "K\n" + (int)towerDamage[3].z + "K\n" + (int)towerDamage[5].z + "K\n" + (int)towerDamage[6].z + "K\n" + (int)towerDamage[12].z + "K\n" + (int)towerDamage[13].z + "K\n" + (int)towerDamage[14].z + "K\n" + (int)towerDamage[8].z + "K\n" + (int)towerDamage[4].z + "K\n" + (int)towerDamage[9].z + "K\n" + (int)towerDamage[10].z + "K\n";
		shieldDmg.text = text3;
		string text4 = "Total\n" + (int)(towerDamage[0].x + towerDamage[0].y + towerDamage[0].z) + "K\n" + (int)(towerDamage[1].x + towerDamage[1].y + towerDamage[1].z) + "K\n" + (int)(towerDamage[2].x + towerDamage[2].y + towerDamage[2].z) + "K\n" + (int)(towerDamage[3].x + towerDamage[3].y + towerDamage[3].z) + "K\n" + (int)(towerDamage[5].x + towerDamage[5].y + towerDamage[5].z) + "K\n" + (int)(towerDamage[6].x + towerDamage[6].y + towerDamage[6].z) + "K\n" + (int)(towerDamage[12].x + towerDamage[12].y + towerDamage[12].z) + "K\n" + (int)(towerDamage[13].x + towerDamage[13].y + towerDamage[13].z) + "K\n" + (int)(towerDamage[14].x + towerDamage[14].y + towerDamage[14].z) + "K\n" + (int)(towerDamage[8].x + towerDamage[8].y + towerDamage[8].z) + "K\n" + (int)(towerDamage[4].x + towerDamage[4].y + towerDamage[4].z) + "K\n" + (int)(towerDamage[9].x + towerDamage[9].y + towerDamage[9].z) + "K\n" + (int)(towerDamage[10].x + towerDamage[10].y + towerDamage[10].z) + "K\n";
		totalDmg.text = text4;
		string text5 = "Cost\n" + towerCost[0] + "g\n" + towerCost[1] + "g\n" + towerCost[2] + "g\n" + towerCost[3] + "g\n" + towerCost[5] + "g\n" + towerCost[6] + "g\n" + towerCost[12] + "g\n" + towerCost[13] + "g\n" + towerCost[14] + "g\n" + towerCost[8] + "g\n" + towerCost[4] + "g\n" + towerCost[9] + "g\n";
		cost.text = text5;
		string text6 = "Dmg/g\n" + (1000f * ((towerDamage[0].x + towerDamage[0].y + towerDamage[0].z) / (float)Mathf.Max(towerCost[0], 1))).ToString("F2") + "\n" + (1000f * ((towerDamage[1].x + towerDamage[1].y + towerDamage[1].z) / (float)Mathf.Max(towerCost[1], 1))).ToString("F2") + "\n" + (1000f * ((towerDamage[2].x + towerDamage[2].y + towerDamage[2].z) / (float)Mathf.Max(towerCost[2], 1))).ToString("F2") + "\n" + (1000f * ((towerDamage[3].x + towerDamage[3].y + towerDamage[3].z) / (float)Mathf.Max(towerCost[3], 1))).ToString("F2") + "\n" + (1000f * ((towerDamage[5].x + towerDamage[5].y + towerDamage[5].z) / (float)Mathf.Max(towerCost[5], 1))).ToString("F2") + "\n" + (1000f * ((towerDamage[6].x + towerDamage[6].y + towerDamage[6].z) / (float)Mathf.Max(towerCost[6], 1))).ToString("F2") + "\n" + (1000f * ((towerDamage[12].x + towerDamage[12].y + towerDamage[12].z) / (float)Mathf.Max(towerCost[12], 1))).ToString("F2") + "\n" + (1000f * ((towerDamage[13].x + towerDamage[13].y + towerDamage[13].z) / (float)Mathf.Max(towerCost[13], 1))).ToString("F2") + "\n" + (1000f * ((towerDamage[14].x + towerDamage[14].y + towerDamage[14].z) / (float)Mathf.Max(towerCost[14], 1))).ToString("F2") + "\n" + (1000f * ((towerDamage[8].x + towerDamage[8].y + towerDamage[8].z) / (float)Mathf.Max(towerCost[8], 1))).ToString("F2") + "\n" + (1000f * ((towerDamage[4].x + towerDamage[4].y + towerDamage[4].z) / (float)Mathf.Max(towerCost[4], 1))).ToString("F2") + "\n" + (1000f * ((towerDamage[9].x + towerDamage[9].y + towerDamage[9].z) / (float)Mathf.Max(towerCost[9], 1))).ToString("F2") + "\n";
		ratio.text = text6;
	}
}
