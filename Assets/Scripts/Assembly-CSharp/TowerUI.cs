using System;
using UnityEngine;
using UnityEngine.UI;

public class TowerUI : MonoBehaviour
{
	private Tower myTower;

	private int level;

	private int baseDamage;

	private int healthDamage;

	private int armorDamage;

	private int shieldDamage;

	private float healthXP;

	private float armorXP;

	private float shieldXP;

	private bool usesMana;

	private float range;

	private float manaUseMultiplier;

	private int rpm;

	[SerializeField]
	private Image healthXPImg;

	[SerializeField]
	private Image armorXPImg;

	[SerializeField]
	private Image shieldXPImg;

	[SerializeField]
	private Text levelText;

	[SerializeField]
	private Text baseDamageText;

	[SerializeField]
	private Text healthDamageText;

	[SerializeField]
	private Text armorDamageText;

	[SerializeField]
	private Text shieldDamageText;

	[SerializeField]
	private Text healthXPText;

	[SerializeField]
	private Text armorXPText;

	[SerializeField]
	private Text shieldXPText;

	[SerializeField]
	private Text[] priorityTexts;

	[SerializeField]
	private Text slowText;

	[SerializeField]
	private Text bleedText;

	[SerializeField]
	private Text burnText;

	[SerializeField]
	private Text poisonText;

	[SerializeField]
	private Text critText;

	[SerializeField]
	private Text rangeText;

	[SerializeField]
	private Text rpmText;

	[SerializeField]
	private Text manaUseText;

	[SerializeField]
	private Text demolishText;

	private float timer = 0.5f;

	[SerializeField]
	private LineRenderer line;

	private void Start()
	{
		UIManager.instance.SetNewUI(base.gameObject);
	}

	private void Update()
	{
		if (timer < 0f)
		{
			SetStats(myTower);
			timer = 0.5f;
		}
		timer -= Time.deltaTime;
		if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
		{
			BuyHealthLevel();
		}
		if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
		{
			BuyArmorLevel();
		}
		if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
		{
			BuyShieldLevel();
		}
	}

	public void SetStats(Tower _myTower)
	{
		myTower = _myTower;
		level = myTower.level;
		baseDamage = myTower.damage;
		healthDamage = myTower.healthDamage;
		armorDamage = myTower.armorDamage;
		shieldDamage = myTower.shieldDamage;
		healthXP = myTower.healthXP;
		armorXP = myTower.armorXP;
		shieldXP = myTower.shieldXP;
		slowText.text = (int)(myTower.slowPercent * 100f) + "%";
		bleedText.text = (int)(myTower.bleedPercent * 100f) + "%";
		burnText.text = (int)(myTower.burnPercent * 100f) + "%";
		poisonText.text = (int)(myTower.poisonPercent * 100f) + "%";
		critText.text = CritText();
		if (myTower.range != range)
		{
			DrawCircle();
		}
		range = myTower.range;
		rpm = (int)myTower.rpm;
		usesMana = myTower.consumesMana;
		manaUseMultiplier = myTower.finalManaConsumption;
		UpdateText();
		for (int i = 0; i < priorityTexts.Length; i++)
		{
			priorityTexts[i].text = myTower.priorities[i].ToString();
		}
	}

	private void DrawCircle()
	{
		float num = myTower.range;
		if (myTower.squareUI)
		{
			num += 0.5f;
			line.SetVertexCount(5);
			line.useWorldSpace = true;
			Vector3 position = base.transform.position;
			position.y = 0.4f;
			line.SetPosition(0, new Vector3(num, 0f, num) + position);
			line.SetPosition(1, new Vector3(num, 0f, 0f - num) + position);
			line.SetPosition(2, new Vector3(0f - num, 0f, 0f - num) + position);
			line.SetPosition(3, new Vector3(0f - num, 0f, num) + position);
			line.SetPosition(4, new Vector3(num, 0f, num) + position);
			return;
		}
		line.SetVertexCount(61);
		line.useWorldSpace = true;
		Vector3 vector = new Vector3(0f, 0f, 0f);
		Vector3 position2 = base.transform.position;
		position2.y = 0.4f;
		float num2 = 0f;
		for (int i = 0; i < 61; i++)
		{
			vector.x = Mathf.Cos((float)Math.PI / 180f * num2) * num;
			vector.z = Mathf.Sin((float)Math.PI / 180f * num2) * num;
			line.SetPosition(i, vector + position2);
			num2 += 6f;
		}
	}

	private string CritText()
	{
		string text = "x2! ";
		text = text + (int)Mathf.Clamp(myTower.critChance * 100f, 0f, 50f) + "%";
		if (myTower.critChance > 0.5f)
		{
			text = text + "\nx3!! " + (int)Mathf.Clamp(myTower.critChance * 100f - 50f, 0f, 50f) + "%";
		}
		if (myTower.critChance > 1f)
		{
			text = text + "\nx4!! " + (int)Mathf.Clamp(myTower.critChance * 100f - 100f, 0f, 50f) + "%";
		}
		return text;
	}

	private void UpdateText()
	{
		levelText.text = "Level: " + level;
		baseDamageText.text = "Base Damage: " + baseDamage;
		healthDamageText.text = "Health Multiplier: " + healthDamage + " (" + baseDamage * healthDamage + ")";
		armorDamageText.text = "Armor Multiplier: " + armorDamage + " (" + baseDamage * armorDamage + ")";
		shieldDamageText.text = "Shield Multiplier: " + shieldDamage + " (" + baseDamage * shieldDamage + ")";
		healthXPText.text = ((10 * level - (int)healthXP) * myTower.upgradeCostMultiplier).ToString();
		armorXPText.text = ((10 * level - (int)armorXP) * myTower.upgradeCostMultiplier).ToString();
		shieldXPText.text = ((10 * level - (int)shieldXP) * myTower.upgradeCostMultiplier).ToString();
		healthXPImg.rectTransform.sizeDelta = new Vector2(healthXP / (float)level, 0.25f);
		armorXPImg.rectTransform.sizeDelta = new Vector2(armorXP / (float)level, 0.25f);
		shieldXPImg.rectTransform.sizeDelta = new Vector2(shieldXP / (float)level, 0.25f);
		rangeText.text = "Range: " + range;
		if (myTower.GetComponent<Dropper>() != null)
		{
			rpmText.text = "Fire Rate: " + (int)myTower.GetComponent<Dropper>().dropperRPMdisplay + " RPM";
		}
		else
		{
			rpmText.text = "Fire Rate: " + rpm + " RPM";
		}
		if (usesMana)
		{
			manaUseText.text = "Mana Use: " + (int)((float)baseDamage * manaUseMultiplier) + "/shot";
		}
		else if (!usesMana && manaUseMultiplier > 0f)
		{
			manaUseText.text = "Mana Use: " + manaUseMultiplier + "/sec";
		}
		else
		{
			manaUseText.text = "";
		}
		demolishText.text = "Demolish (" + TowerManager.instance.GetSellPrice(myTower.towerType) + "g)";
	}

	public void BuyHealthLevel()
	{
		SFXManager.instance.ButtonClick();
		myTower.BuyHealthLevel();
		SetStats(myTower);
	}

	public void BuyArmorLevel()
	{
		SFXManager.instance.ButtonClick();
		myTower.BuyArmorLevel();
		SetStats(myTower);
	}

	public void BuyShieldLevel()
	{
		SFXManager.instance.ButtonClick();
		myTower.BuyShieldLevel();
		SetStats(myTower);
	}

	public void TogglePriorityUp(int index)
	{
		SFXManager.instance.ButtonClick();
		myTower.TogglePriority(index, 1);
		priorityTexts[index].text = myTower.priorities[index].ToString();
	}

	public void TogglePriorityDown(int index)
	{
		SFXManager.instance.ButtonClick();
		myTower.TogglePriority(index, -1);
		priorityTexts[index].text = myTower.priorities[index].ToString();
	}

	public void DemolishTower()
	{
		SFXManager.instance.ButtonClick();
		myTower.Demolish();
		CloseUI();
	}

	public void CloseUI()
	{
		UIManager.instance.CloseUI(base.gameObject);
	}
}
