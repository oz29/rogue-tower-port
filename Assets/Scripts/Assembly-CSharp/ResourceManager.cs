using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
	public static ResourceManager instance;

	[SerializeField]
	private int gold;

	[SerializeField]
	private Text goldText;

	[SerializeField]
	private int mana;

	private float manaLoss = 1f;

	[SerializeField]
	private int maxMana;

	[SerializeField]
	private int manaGatherRate;

	[SerializeField]
	private Text manaText;

	[SerializeField]
	private Image maskBar;

	[SerializeField]
	private Image manaBar;

	[SerializeField]
	private Image manaLossBar;

	private float manaTimer;

	private int manaGen;

	public float manaMaxRegenPercent;

	[SerializeField]
	private GameObject manaHUDObject;

	[SerializeField]
	private Text manaHUDText;

	public int enemyBonusGoldDrop;

	public List<ManaBank> manaBanks = new List<ManaBank>();

	public int manaBankBonusMana;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		gold = 1000 + 100 * (PlayerPrefs.GetInt("StartGold1", 0) + PlayerPrefs.GetInt("StartGold2", 0) + PlayerPrefs.GetInt("StartGold3", 0) + PlayerPrefs.GetInt("StartGold4", 0) + PlayerPrefs.GetInt("StartGold5", 0) + PlayerPrefs.GetInt("StartGold6", 0) + PlayerPrefs.GetInt("StartGold7", 0) + PlayerPrefs.GetInt("StartGold8", 0) + PlayerPrefs.GetInt("StartGold9", 0) + PlayerPrefs.GetInt("StartGold10", 0));
		maxMana = 100 + 20 * (PlayerPrefs.GetInt("MaxMana1", 0) + PlayerPrefs.GetInt("MaxMana2", 0) + PlayerPrefs.GetInt("MaxMana3", 0) + PlayerPrefs.GetInt("MaxMana4", 0) + PlayerPrefs.GetInt("MaxMana5", 0));
		enemyBonusGoldDrop = PlayerPrefs.GetInt("GoldDrop1", 0) + PlayerPrefs.GetInt("GoldDrop2", 0) + PlayerPrefs.GetInt("GoldDrop3", 0) + PlayerPrefs.GetInt("GoldDrop4", 0) + PlayerPrefs.GetInt("GoldDrop5", 0);
		manaGen = PlayerPrefs.GetInt("ManaGen1", 0) + PlayerPrefs.GetInt("ManaGen2", 0) + PlayerPrefs.GetInt("ManaGen3", 0);
		UpdateManaGatherRate(manaGen);
		SetManaBar();
	}

	private void Update()
	{
		if (manaLoss > (float)mana)
		{
			float num = 20f;
			if (maxMana >= 500)
			{
				num = 100f;
			}
			manaLoss = Mathf.Clamp(manaLoss - num * Time.deltaTime, mana, manaLoss);
			manaLossBar.rectTransform.sizeDelta = new Vector2(manaLoss / num, 0.25f);
		}
		if (manaTimer <= 0f)
		{
			AddMana(manaGen);
			AddManaPercentMax(manaMaxRegenPercent);
			manaTimer = 1f;
		}
		else if (manaGen > 0 && SpawnManager.instance.combat)
		{
			manaTimer -= Time.deltaTime;
		}
	}

	public void Spend(int goldCost)
	{
		gold -= goldCost;
		UpdateResourceText();
	}

	public void AddMoney(int addedGold)
	{
		gold += addedGold;
		UpdateResourceText();
	}

	public bool CheckMoney(int goldCost)
	{
		if (gold >= goldCost)
		{
			return true;
		}
		return false;
	}

	public void SpendMana(int manaCost)
	{
		mana -= manaCost;
		UpdateResourceText();
	}

	public void AddMana(int addedMana)
	{
		mana = Mathf.Clamp(mana + addedMana, 0, maxMana);
		if (manaLoss < (float)mana)
		{
			manaLoss = mana;
		}
		UpdateResourceText();
	}

	public void AddManaPercentMax(float percent)
	{
		AddMana(Mathf.FloorToInt(percent * (float)maxMana));
	}

	public void AddMaxMana(int amount)
	{
		maxMana += amount;
		float num = 20f;
		if (maxMana >= 500)
		{
			num = 100f;
		}
		maskBar.rectTransform.localScale = new Vector3(num * 200f / (float)maxMana, 50f, 10f);
		maskBar.rectTransform.sizeDelta = new Vector2((float)maxMana / num, 0.25f);
		UpdateResourceText();
	}

	public bool CheckMana(int manaCost)
	{
		if (mana >= manaCost)
		{
			return true;
		}
		return false;
	}

	public void UpdateManaGatherRate(int addedGatherRate)
	{
		manaGatherRate += addedGatherRate;
		UpdateResourceText();
	}

	public void UpdateManaHUD()
	{
		string text = "";
		if (manaMaxRegenPercent > 0f)
		{
			text = text + "+" + Mathf.FloorToInt(101f * manaMaxRegenPercent) + "%/s\n";
		}
		if (MonsterManager.instance.manaDropOnDeath > 0f)
		{
			text = text + Mathf.FloorToInt(101f * MonsterManager.instance.manaDropOnDeath) + "%/kill";
		}
		if (text.Length > 1)
		{
			manaHUDObject.SetActive(value: true);
			manaHUDText.text = text;
		}
	}

	public void AddPercentManaGathering(float percent)
	{
		manaMaxRegenPercent += percent;
		UpdateResourceText();
	}

	private void UpdateResourceText()
	{
		goldText.text = "Gold: " + gold;
		manaText.text = "Mana: " + mana + "/" + maxMana + " (+" + (manaGatherRate + Mathf.FloorToInt(manaMaxRegenPercent * (float)maxMana)) + "/s)";
		float num = 20f;
		if (maxMana >= 500)
		{
			num = 100f;
		}
		manaBar.rectTransform.sizeDelta = new Vector2((float)mana / num, 0.25f);
	}

	public void SetManaBar()
	{
		float num = 20f;
		if (maxMana >= 500)
		{
			num = 100f;
		}
		maskBar.rectTransform.localScale = new Vector3(num * 200f / (float)maxMana, 50f, 10f);
		maskBar.rectTransform.sizeDelta = new Vector2((float)maxMana / num, 0.25f);
		manaLossBar.rectTransform.sizeDelta = new Vector2((float)maxMana / num, 0.25f);
		UpdateResourceText();
	}

	public void UpgradeManaBankMaxMana(int addition)
	{
		manaBankBonusMana += addition;
		foreach (ManaBank manaBank in manaBanks)
		{
			if (manaBank != null)
			{
				manaBank.UpgradeMaxMana(addition);
			}
		}
	}
}
