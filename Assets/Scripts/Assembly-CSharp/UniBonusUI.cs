using UnityEngine;
using UnityEngine.UI;

public class UniBonusUI : MonoBehaviour
{
	public static UniBonusUI instance;

	[SerializeField]
	private GameObject uniUI;

	[SerializeField]
	private Text uniUIText;

	private int healthTotal;

	private int armorTotal;

	private int shieldTotal;

	private void Awake()
	{
		instance = this;
	}

	public void UniBonus(int health, int armor, int shield)
	{
		healthTotal += health;
		armorTotal += armor;
		shieldTotal += shield;
		uniUI.SetActive(value: true);
		if (uniUIText != null)
		{
			uniUIText.text = "";
			uniUIText.gameObject.SetActive(false);
		}
	}
}
