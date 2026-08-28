using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
	[SerializeField]
	private string unlockString;

	[SerializeField]
	public int xpCost;

	public int cardCountRequirement;

	[SerializeField]
	private bool countsAsCardUnlock = true;

	[SerializeField]
	private bool countAsDevelopment;

	[SerializeField]
	private Sprite unlockedSprite;

	[SerializeField]
	private bool checkAchievements;

	[SerializeField]
	private GameObject priceTag;

	private GameObject currentPriceTag;

	private Image img;

	private Button btn;

	[SerializeField]
	private UpgradeButton previous;

	[SerializeField]
	private UpgradeButton[] next;

	public bool unlocked;

	private void Start()
	{
		img = GetComponent<Image>();
		btn = GetComponent<Button>();
		CheckUnlock();
		StartCoroutine(LateStart());
	}

	public void Unlock()
	{
		if (UpgradeManager.instance.xp >= xpCost)
		{
			SFXManager.instance.ButtonClick();
			UpgradeManager.instance.AddXP(-xpCost);
			PlayerPrefs.SetInt(unlockString, 1);
			if (checkAchievements)
			{
				AchievementManager.instance.CheckTowerUnlocks();
			}
			UpgradeManager.instance.CountCard(countsAsCardUnlock);
			UpgradeManager.instance.CountDevelopment(countAsDevelopment);
			btn.enabled = false;
			img.sprite = unlockedSprite;
			unlocked = true;
			UpdateTitleText();
			UpgradeButton[] array = next;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].CheckEnabled();
			}
		}
	}

	public void ResetUnlock()
	{
		PlayerPrefs.SetInt(unlockString, 0);
		unlocked = false;
	}

	private void CheckUnlock()
	{
		if (PlayerPrefs.GetInt(unlockString, 0) == 1)
		{
			btn.enabled = false;
			img.sprite = unlockedSprite;
			unlocked = true;
		}
	}

	public void CheckEnabled()
	{
		if (previous == null)
		{
			if (cardCountRequirement <= UpgradeManager.instance.unlockedCardCount)
			{
				btn.interactable = true;
			}
			else
			{
				btn.interactable = false;
			}
			UpdateTitleText();
		}
		else
		{
			if (previous.unlocked && cardCountRequirement <= UpgradeManager.instance.unlockedCardCount)
			{
				btn.interactable = true;
			}
			else
			{
				btn.interactable = false;
			}
			UpdateTitleText();
		}
	}

	private void UpdateTitleText()
	{
		if (!unlocked)
		{
			if (currentPriceTag == null)
			{
				currentPriceTag = Object.Instantiate(priceTag, base.transform);
				currentPriceTag.transform.localPosition = new Vector3(-63.7f, 0f, 0f);
				int num = cardCountRequirement - UpgradeManager.instance.unlockedCardCount;
				if (num > 0)
				{
					currentPriceTag.GetComponent<RectTransform>().sizeDelta = new Vector2(75f, 50f);
					currentPriceTag.GetComponentInChildren<Text>().text = " Unlock " + num + "\n more cards";
				}
				else
				{
					currentPriceTag.GetComponent<RectTransform>().sizeDelta = new Vector2(50f, 25f);
					currentPriceTag.GetComponentInChildren<Text>().text = xpCost + " xp";
				}
			}
			else
			{
				int num2 = cardCountRequirement - UpgradeManager.instance.unlockedCardCount;
				if (num2 > 0)
				{
					currentPriceTag.GetComponent<RectTransform>().sizeDelta = new Vector2(75f, 50f);
					currentPriceTag.GetComponentInChildren<Text>().text = " Unlock " + num2 + "\n more cards";
				}
				else
				{
					currentPriceTag.GetComponent<RectTransform>().sizeDelta = new Vector2(50f, 25f);
					currentPriceTag.GetComponentInChildren<Text>().text = xpCost + " xp";
				}
			}
		}
		else if (currentPriceTag != null)
		{
			currentPriceTag.SetActive(value: false);
		}
	}

	private IEnumerator LateStart()
	{
		yield return new WaitForSeconds(0.1f);
		CheckEnabled();
	}
}
