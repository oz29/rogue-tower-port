using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
	public static CardManager instance;

	private List<int> queuedDraws = new List<int>();

	[SerializeField]
	private int baseCardDraw = 3;

	[SerializeField]
	private GameObject ui;

	[SerializeField]
	private List<UpgradeCard> availableCards = new List<UpgradeCard>();

	[SerializeField]
	private List<UpgradeCard> startCardsCheck = new List<UpgradeCard>();

	private UpgradeCard[] cards = new UpgradeCard[6];

	[SerializeField]
	private Text[] titles = new Text[6];

	[SerializeField]
	private Image[] images = new Image[6];

	[SerializeField]
	private Text[] descriptions = new Text[6];

	[SerializeField]
	private GameObject[] cardHolders = new GameObject[6];

	public bool drawingCards { get; private set; }

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		baseCardDraw = 3 + PlayerPrefs.GetInt("CardDraw1", 0) + PlayerPrefs.GetInt("CardDraw2", 0) + PlayerPrefs.GetInt("CardDraw3", 0);
		foreach (UpgradeCard item in startCardsCheck)
		{
			if (item.unlocked)
			{
				availableCards.Add(item);
			}
		}
		ArrangeCards(baseCardDraw);
	}

	public void DrawCards()
	{
		DrawCards(baseCardDraw);
	}

	public void DrawCards(int numb)
	{
		if (drawingCards)
		{
			Debug.Log("Queueing Draw call of " + numb);
			queuedDraws.Add(numb);
			return;
		}
		int num = numb;
		if (availableCards.Count < num)
		{
			num = availableCards.Count;
			if (num <= 0)
			{
				Debug.LogError("No cards to draw. (Only " + availableCards.Count + " available)");
				SpawnManager.instance.ShowSpawnUIs(state: true);
				return;
			}
		}
		ArrangeCards(num);
		SFXManager.instance.PlaySound(Sound.Cards, MusicManager.instance.transform.position, MusicManager.instance.transform);
		SpawnManager.instance.ShowSpawnUIs(state: false);
		ui.SetActive(value: true);
		drawingCards = true;
		int[] array = new int[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = -1;
		}
		for (int j = 0; j < num; j++)
		{
			int num2 = Random.Range(0, availableCards.Count);
			int num3 = 0;
			while (CheckArray(array, num2))
			{
				num2 = Random.Range(0, availableCards.Count);
				num3++;
				if (num3 > 100)
				{
					Debug.LogError("Possible infinite loop in card selection");
					break;
				}
			}
			array[j] = num2;
		}
		for (int k = 0; k < num; k++)
		{
			cards[k] = availableCards[array[k]];
		}
		DisplayCards(num);
	}

	private void ArrangeCards(int count)
	{
		for (int i = 0; i < count; i++)
		{
			cardHolders[i].SetActive(value: true);
		}
		for (int j = count; j < 6; j++)
		{
			cardHolders[j].SetActive(value: false);
		}
		for (int k = 0; k < count; k++)
		{
			cardHolders[k].transform.localPosition = new Vector3(200 * k - 100 * count + 100, 0f, 0f);
		}
	}

	private void DisplayCards(int count)
	{
		for (int i = 0; i < count; i++)
		{
			titles[i].text = cards[i].title;
			images[i].sprite = cards[i].image;
			descriptions[i].text = cards[i].description;
		}
	}

	public void ActivateCard(int index)
	{
		if (RunSaveManager.instance != null && cards[index] != null)
		{
			RunSaveManager.instance.RecordPickedCard(cards[index].title);
		}
		cards[index].Upgrade();
		foreach (UpgradeCard unlock in cards[index].unlocks)
		{
			if (unlock.unlocked)
			{
				availableCards.Add(unlock);
			}
		}
		availableCards.Remove(cards[index]);
		drawingCards = false;
		if (queuedDraws.Count > 0)
		{
			int numb = queuedDraws[0];
			queuedDraws.RemoveAt(0);
			DrawCards(numb);
			return;
		}
		if (!SpawnManager.instance.combat)
		{
			SpawnManager.instance.ShowSpawnUIs(state: true);
		}
		ui.SetActive(value: false);
	}

	private bool CheckArray(int[] array, int val)
	{
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == val)
			{
				return true;
			}
		}
		return false;
	}
}
