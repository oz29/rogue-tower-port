using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	[SerializeField]
	private Image maskBar;

	[SerializeField]
	private Image healthBar;

	[SerializeField]
	private Image armorBar;

	[SerializeField]
	private Image shieldBar;

	[SerializeField]
	private Image dmgBar;

	[SerializeField]
	private Image slowImageLeft;

	[SerializeField]
	private Image slowImageRight;

	[SerializeField]
	private GameObject BleedImage;

	[SerializeField]
	private GameObject BurnImage;

	[SerializeField]
	private GameObject PoisonImage;

	[SerializeField]
	private Text bleedText;

	[SerializeField]
	private Text burnText;

	[SerializeField]
	private Text poisonText;

	[SerializeField]
	private Image fortImageLeft;

	[SerializeField]
	private Image fortImageRight;

	[SerializeField]
	private Image hasteImageLeft;

	[SerializeField]
	private Image hasteImageRight;

	private float maxHp;

	private float health;

	private float armor;

	private float shield;

	private float dmg;

	private float barScaler = 100f;

	private void Update()
	{
		if (dmg > health + armor + shield)
		{
			dmg = Mathf.Clamp(dmg - 4f * Time.deltaTime, health + armor + shield, dmg);
			dmgBar.rectTransform.sizeDelta = new Vector2(dmg, 0.25f);
		}
	}

	public void SetHealth(int max, int heal, int armr, int shld, int scaleDegree)
	{
		barScaler = 10f * Mathf.Pow(10f, scaleDegree);
		maxHp = (float)max / barScaler;
		dmg = maxHp;
		health = (float)heal / barScaler;
		armor = (float)armr / barScaler;
		shield = (float)shld / barScaler;
		maskBar.rectTransform.localScale = new Vector3(3f / (maxHp + 3f), 1f, 1f);
		maskBar.rectTransform.sizeDelta = new Vector2(maxHp, 0.25f);
		dmgBar.rectTransform.sizeDelta = new Vector2(dmg, 0.25f);
		UpdateHealth(heal, armr, shld, 0f, isBleeding: false, isBurning: false, isPoisoned: false, 0, 0, 0);
	}

	public void UpdateHealth(int heal, int armr, int shld, float currentSlow, bool isBleeding, bool isBurning, bool isPoisoned, int currentBleed, int currentBurn, int currentPoison)
	{
		health = (float)heal / barScaler;
		armor = (float)armr / barScaler;
		shield = (float)shld / barScaler;
		healthBar.rectTransform.sizeDelta = new Vector2(health, 0.25f);
		armorBar.rectTransform.sizeDelta = new Vector2(armor, 0.25f);
		shieldBar.rectTransform.sizeDelta = new Vector2(shield, 0.25f);
		armorBar.rectTransform.localPosition = new Vector3(health - maskBar.rectTransform.sizeDelta.x / 2f, 0f, 0f);
		shieldBar.rectTransform.localPosition = new Vector3(health + armor - maskBar.rectTransform.sizeDelta.x / 2f, 0f, 0f);
		Image image = slowImageLeft;
		float fillAmount = (slowImageRight.fillAmount = currentSlow);
		image.fillAmount = fillAmount;
		BleedImage.SetActive(isBleeding);
		bleedText.gameObject.SetActive(isBleeding);
		bleedText.text = currentBleed.ToString();
		BurnImage.SetActive(isBurning);
		burnText.gameObject.SetActive(isBurning);
		burnText.text = currentBurn.ToString();
		PoisonImage.SetActive(isPoisoned);
		poisonText.gameObject.SetActive(isPoisoned);
		poisonText.text = currentPoison.ToString();
	}

	public void UpdateSlow(float currentSlow)
	{
		Image image = slowImageLeft;
		float fillAmount = (slowImageRight.fillAmount = currentSlow);
		image.fillAmount = fillAmount;
	}

	public void UpdateBleed(bool status, int amt)
	{
		BleedImage.SetActive(status);
		bleedText.gameObject.SetActive(status);
		bleedText.text = amt.ToString();
	}

	public void UpdateBurn(bool status, int amt)
	{
		BurnImage.SetActive(status);
		burnText.gameObject.SetActive(status);
		burnText.text = amt.ToString();
	}

	public void UpdatePoison(bool status, int amt)
	{
		PoisonImage.SetActive(status);
		poisonText.gameObject.SetActive(status);
		poisonText.text = amt.ToString();
	}

	public void UpdateFortified(float fortTime)
	{
		Image image = fortImageLeft;
		float fillAmount = (fortImageRight.fillAmount = fortTime * 0.083333f);
		image.fillAmount = fillAmount;
	}

	public void UpdateHaste(float hastePercentage)
	{
		Image image = hasteImageLeft;
		float fillAmount = (hasteImageRight.fillAmount = hastePercentage);
		image.fillAmount = fillAmount;
	}
}
