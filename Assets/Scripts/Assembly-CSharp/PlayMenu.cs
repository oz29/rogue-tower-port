using UnityEngine;
using UnityEngine.UI;

public class PlayMenu : MonoBehaviour
{
	[SerializeField]
	private Text record1Text;

	[SerializeField]
	private Text record2Text;

	[SerializeField]
	private Text record3Text;

	private void Start()
	{
		record1Text.text = "Current Record\nLevel " + PlayerPrefs.GetInt("Record1", 0);
		record2Text.text = "Current Record\nLevel " + PlayerPrefs.GetInt("Record2", 0);
		record3Text.text = "Current Record\nLevel " + PlayerPrefs.GetInt("Record3", 0);

		CheckAndCreateContinueButton();
	}

	private void CheckAndCreateContinueButton()
	{
		if (RunSaveManager.HasSavedRun())
		{
			RunSaveData data = RunSaveManager.GetSavedRunHeader();
			if (data != null)
			{
				Button singleBtn = record1Text.GetComponentInParent<Button>();
				if (singleBtn == null) singleBtn = GetComponentInChildren<Button>();
				if (singleBtn != null)
				{
					GameObject cBtnObj = Instantiate(singleBtn.gameObject, singleBtn.transform.parent);
					cBtnObj.name = "ContinueRunButton";
					cBtnObj.transform.SetAsFirstSibling();
					RectTransform rt = cBtnObj.GetComponent<RectTransform>();
					if (rt != null)
					{
						rt.anchoredPosition = new Vector2(0f, 160f);
						rt.sizeDelta = new Vector2(380f, 90f);
					}
					Button btn = cBtnObj.GetComponent<Button>();
					btn.onClick.RemoveAllListeners();
					btn.onClick.AddListener(ContinueSavedGame);

					Image img = cBtnObj.GetComponent<Image>();
					if (img != null) img.color = new Color(0.2f, 0.75f, 0.35f, 1f);

					Text txt = cBtnObj.GetComponentInChildren<Text>();
					if (txt != null)
					{
						string modeName = data.gameMode == 1 ? "Single" : (data.gameMode == 2 ? "Double" : "Triple");
						txt.text = $"▶ Continuar Partida\nNivel {data.level} ({modeName})";
						txt.fontSize = 24;
						txt.color = Color.white;
					}
				}
			}
		}
	}

	public void ContinueSavedGame()
	{
		RunSaveManager.loadSavedRunOnStart = true;
		RunSaveData data = RunSaveManager.GetSavedRunHeader();
		if (data != null)
		{
			PlayerPrefs.SetInt("GameMode", Mathf.Clamp(data.gameMode, 1, 3));
		}
		LevelLoader.instance.LoadLevel("GameScene");
	}

	public void StartGame(int mode)
	{
		RunSaveManager.loadSavedRunOnStart = false;
		RunSaveManager.DeleteSavedRun();
		PlayerPrefs.SetInt("GameMode", Mathf.Clamp(mode, 1, 3));
		LevelLoader.instance.LoadLevel("GameScene");
	}
}
