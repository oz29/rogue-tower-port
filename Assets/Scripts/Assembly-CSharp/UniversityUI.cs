using UnityEngine;
using UnityEngine.UI;

public class UniversityUI : MonoBehaviour
{
	private University myUniversity;

	[SerializeField]
	private Text healthCostText;

	[SerializeField]
	private Text armorCostText;

	[SerializeField]
	private Text shieldCostText;

	[SerializeField]
	private Text healthDiscoveresText;

	[SerializeField]
	private Text armorDiscoveriesText;

	[SerializeField]
	private Text shieldDiscoveriesText;

	[SerializeField]
	private Text healthBonusText;

	[SerializeField]
	private Text armorBonusText;

	[SerializeField]
	private Text shieldBonusText;

	[SerializeField]
	private Text healthPercentageText;

	[SerializeField]
	private Text armorPercentageText;

	[SerializeField]
	private Text shieldPercentageText;

	[SerializeField]
	private Image healthPercentageImg;

	[SerializeField]
	private Image armorPercentageImg;

	[SerializeField]
	private Image shieldPercentageImg;

	[SerializeField]
	private Text demolishText;

	private void Start()
	{
		UIManager.instance.SetNewUI(base.gameObject);
		if (demolishText != null)
		{
			demolishText.text = "Demolish (" + myUniversity.goldBackOnDemolish + "g)";
		}
	}

	public void SetStats(University myUni)
	{
		myUniversity = myUni;
		int num = (myUniversity.healthPercent + myUniversity.armorPercent + myUniversity.shieldPercent + 1) * 20;
		healthCostText.text = num.ToString();
		armorCostText.text = num.ToString();
		shieldCostText.text = num.ToString();
		healthDiscoveresText.text = "Health Studies: " + myUniversity.healthGained;
		armorDiscoveriesText.text = "Armor Studies: " + myUniversity.armorGained;
		shieldDiscoveriesText.text = "Magic Studies: " + myUniversity.shieldGained;
		healthBonusText.text = "Global Health Damage: +" + myUniversity.healthGained;
		armorBonusText.text = "Global Armor Damage: +" + myUniversity.armorGained;
		shieldBonusText.text = "Global Shield Damage: +" + myUniversity.shieldGained;
		healthPercentageText.text = "Health Studies: " + (myUniversity.healthPercent + GameManager.instance.universityBonus) + "%";
		armorPercentageText.text = "Armor Studies: " + (myUniversity.armorPercent + GameManager.instance.universityBonus) + "%";
		shieldPercentageText.text = "Magic Studies: " + (myUniversity.shieldPercent + GameManager.instance.universityBonus) + "%";
		healthPercentageImg.rectTransform.sizeDelta = new Vector2((float)(myUniversity.healthPercent + GameManager.instance.universityBonus) / 10f, 0.25f);
		armorPercentageImg.rectTransform.sizeDelta = new Vector2((float)(myUniversity.armorPercent + GameManager.instance.universityBonus) / 10f, 0.25f);
		shieldPercentageImg.rectTransform.sizeDelta = new Vector2((float)(myUniversity.shieldPercent + GameManager.instance.universityBonus) / 10f, 0.25f);
		if (demolishText != null)
		{
			demolishText.text = "Demolish (" + myUniversity.goldBackOnDemolish + "g)";
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
		{
			FundHealthResearch();
		}
		if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
		{
			FundArmorResearch();
		}
		if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
		{
			FundShieldResearch();
		}
	}

	public void FundHealthResearch()
	{
		SFXManager.instance.ButtonClick();
		myUniversity.FundHealthStudies();
		SetStats(myUniversity);
	}

	public void FundArmorResearch()
	{
		SFXManager.instance.ButtonClick();
		myUniversity.FundArmorStudies();
		SetStats(myUniversity);
	}

	public void FundShieldResearch()
	{
		SFXManager.instance.ButtonClick();
		myUniversity.FundShieldStudies();
		SetStats(myUniversity);
	}

	public void Demolish()
	{
		SFXManager.instance.ButtonClick();
		myUniversity.Demolish();
		Object.Destroy(myUniversity.gameObject);
		ResourceManager.instance.AddMoney(myUniversity.goldBackOnDemolish);
		UIManager.instance.CloseUI(base.gameObject);
	}
}
