using UnityEngine;
using UnityEngine.UI;

public class MonsterManualEntry : MonoBehaviour
{
	[SerializeField]
	private int level;

	[SerializeField]
	private Enemy prefab;

	[SerializeField]
	private Text titleText;

	[SerializeField]
	private Text descriptionText;

	[SerializeField]
	private Image img;

	[SerializeField]
	private Sprite unknownSprite;

	private void Start()
	{
		int b = 0;
		b = Mathf.Max(PlayerPrefs.GetInt("Record1", 0), b);
		b = Mathf.Max(PlayerPrefs.GetInt("Record2", 0), b);
		b = Mathf.Max(PlayerPrefs.GetInt("Record3", 0), b);
		if (b + 1 < prefab.level)
		{
			titleText.text = "???";
			descriptionText.text = "???";
			img.sprite = unknownSprite;
			return;
		}
		string text = descriptionText.text;
		string text2 = "";
		text2 = text2 + "Speed: " + prefab.baseSpeed;
		text2 = text2 + "| Health: " + prefab.baseHealth;
		if (prefab.baseArmor > 0)
		{
			text2 = text2 + "| Armor: " + prefab.baseArmor;
		}
		if (prefab.baseShield > 0)
		{
			text2 = text2 + "| Shield: " + prefab.baseShield;
		}
		if (prefab.healthRegen > 0)
		{
			text2 = text2 + "| Heal: " + prefab.healthRegen + "/sec";
		}
		if (prefab.armorRegen > 0)
		{
			text2 = text2 + "| Armor repair: " + prefab.armorRegen + "/sec";
		}
		if (prefab.shieldRegen > 0)
		{
			text2 = text2 + "| Shield regen: " + prefab.shieldRegen + "/sec";
		}
		descriptionText.text = text2 + "\n" + text;
	}
}
