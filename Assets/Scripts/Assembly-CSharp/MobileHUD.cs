using UnityEngine;
using UnityEngine.UI;

public class MobileHUD : MonoBehaviour
{
	public static MobileHUD instance;

	private GameObject hudCanvasObj;
	private GameObject cancelBtnObj;
	private GameObject menuBtnObj;
	private GameObject continuousBtnObj;
	private Text continuousBtnText;
	private Image continuousBtnImg;

	private void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
			return;
		}
		instance = this;
		CreateHUD();
	}

	private void Start()
	{
		FixSquishedIcons();
	}

	private void CreateHUD()
	{
		ClearPCControlsUI();
		hudCanvasObj = new GameObject("MobileHUD_Canvas");
		DontDestroyOnLoad(hudCanvasObj);

		Canvas canvas = hudCanvasObj.AddComponent<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		canvas.sortingOrder = 999;

		CanvasScaler scaler = hudCanvasObj.AddComponent<CanvasScaler>();
		scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
		scaler.referenceResolution = new Vector2(1920, 1080);
		scaler.matchWidthOrHeight = 0.5f;

		hudCanvasObj.AddComponent<GraphicRaycaster>();

		Font font = Resources.GetBuiltinResource<Font>("Arial.ttf");

		// 1. Menu / Pause Button (Top Right)
		menuBtnObj = CreateButton(
			hudCanvasObj.transform,
			"MobileMenuButton",
			"\u23F8 Menú",
			new Vector2(1f, 1f),
			new Vector2(1f, 1f),
			new Vector2(-105f, -60f),
			new Vector2(175f, 75f),
			new Color(0.12f, 0.12f, 0.16f, 0.88f),
			font,
			26,
			() =>
			{
				if (PauseMenu.instance != null)
				{
					if (!PauseMenu.instance.paused)
					{
						PauseMenu.instance.UnHideUI();
						PauseMenu.instance.Pause();
					}
					else
					{
						PauseMenu.instance.UnPause();
					}
				}
			}
		);

		// 2. Cancel / ESC Button (Middle Right, upper)
		cancelBtnObj = CreateButton(
			hudCanvasObj.transform,
			"MobileCancelButton",
			"\u2716 Cancelar",
			new Vector2(1f, 0.5f),
			new Vector2(1f, 0.5f),
			new Vector2(-120f, 50f),
			new Vector2(210f, 85f),
			new Color(0.75f, 0.15f, 0.15f, 0.95f),
			font,
			26,
			() =>
			{
				if (BuildingManager.instance != null && BuildingManager.instance.buildMode)
				{
					BuildingManager.instance.ExitBuildMode();
				}
				else if (UIManager.instance != null && UIManager.instance.HasActiveUI)
				{
					UIManager.instance.CloseCurrentUI();
				}
			}
		);

		// 3. Multi-Build (Shift) Toggle Button (Middle Right, lower)
		continuousBtnObj = CreateButton(
			hudCanvasObj.transform,
			"MobileContinuousButton",
			"\u267B Multi (OFF)",
			new Vector2(1f, 0.5f),
			new Vector2(1f, 0.5f),
			new Vector2(-120f, -50f),
			new Vector2(210f, 85f),
			new Color(0.2f, 0.22f, 0.28f, 0.9f),
			font,
			22,
			() =>
			{
				if (BuildingManager.instance != null)
				{
					BuildingManager.instance.continuousBuildMode = !BuildingManager.instance.continuousBuildMode;
					UpdateContinuousButtonState();
				}
			}
		);

		continuousBtnText = continuousBtnObj.GetComponentInChildren<Text>();
		continuousBtnImg = continuousBtnObj.GetComponent<Image>();

		cancelBtnObj.SetActive(false);
		continuousBtnObj.SetActive(false);
	}

	private void UpdateContinuousButtonState()
	{
		if (BuildingManager.instance != null && continuousBtnText != null && continuousBtnImg != null)
		{
			if (BuildingManager.instance.continuousBuildMode)
			{
				continuousBtnText.text = "\u2714 Multi (ON)";
				continuousBtnImg.color = new Color(0.12f, 0.65f, 0.25f, 0.95f);
			}
			else
			{
				continuousBtnText.text = "\u267B Multi (OFF)";
				continuousBtnImg.color = new Color(0.2f, 0.22f, 0.28f, 0.9f);
			}
		}
	}

	private GameObject CreateButton(Transform parent, string name, string text, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPos, Vector2 size, Color bgColor, Font font, int fontSize, UnityEngine.Events.UnityAction onClick)
	{
		GameObject btnObj = new GameObject(name);
		btnObj.transform.SetParent(parent, false);

		RectTransform rect = btnObj.AddComponent<RectTransform>();
		rect.anchorMin = anchorMin;
		rect.anchorMax = anchorMax;
		rect.pivot = new Vector2(0.5f, 0.5f);
		rect.anchoredPosition = anchoredPos;
		rect.sizeDelta = size;

		Image img = btnObj.AddComponent<Image>();
		img.color = bgColor;

		Button btn = btnObj.AddComponent<Button>();
		ColorBlock cb = btn.colors;
		cb.normalColor = bgColor;
		cb.highlightedColor = bgColor * 1.2f;
		cb.pressedColor = bgColor * 0.8f;
		btn.colors = cb;
		btn.onClick.AddListener(onClick);

		GameObject textObj = new GameObject("Text");
		textObj.transform.SetParent(btnObj.transform, false);
		RectTransform textRect = textObj.AddComponent<RectTransform>();
		textRect.anchorMin = Vector2.zero;
		textRect.anchorMax = Vector2.one;
		textRect.sizeDelta = Vector2.zero;

		Text txt = textObj.AddComponent<Text>();
		txt.text = text;
		txt.alignment = TextAnchor.MiddleCenter;
		txt.color = Color.white;
		txt.fontSize = fontSize;
		txt.fontStyle = FontStyle.Bold;
		if (font != null)
		{
			txt.font = font;
		}

		return btnObj;
	}

	private void Update()
	{
		bool inBuildMode = (BuildingManager.instance != null && BuildingManager.instance.buildMode);
		bool hasUI = (UIManager.instance != null && UIManager.instance.HasActiveUI);

		if (cancelBtnObj != null)
		{
			bool shouldShowCancel = inBuildMode || hasUI;
			if (cancelBtnObj.activeSelf != shouldShowCancel)
			{
				cancelBtnObj.SetActive(shouldShowCancel);
			}
		}

		if (continuousBtnObj != null)
		{
			if (continuousBtnObj.activeSelf != inBuildMode)
			{
				continuousBtnObj.SetActive(inBuildMode);
				if (!inBuildMode && BuildingManager.instance != null)
				{
					BuildingManager.instance.continuousBuildMode = false;
					UpdateContinuousButtonState();
				}
			}
		}

		if (menuBtnObj != null)
		{
			bool inGame = GameManager.instance != null && !GameManager.instance.gameOver;
			bool isPaused = PauseMenu.instance != null && PauseMenu.instance.paused;
			if (menuBtnObj.activeSelf != (inGame && !isPaused))
			{
				menuBtnObj.SetActive(inGame && !isPaused);
			}
		}
	}

	private void ClearPCControlsUI()
	{
		Text[] allTexts = Object.FindObjectsOfType<Text>();
		for (int i = 0; i < allTexts.Length; i++)
		{
			Text txt = allTexts[i];
			if (txt != null && !string.IsNullOrEmpty(txt.text))
			{
				if (txt.text.Contains("Pause game") || txt.text.Contains("Hold shift") || txt.text.Contains("Recenter camera") || txt.text.Contains("Hide UI"))
				{
					txt.text = "";
					txt.gameObject.SetActive(false);
				}
			}
		}
	}

	public void FixSquishedIcons()
	{
		Image[] allImages = Object.FindObjectsOfType<Image>();
		for (int i = 0; i < allImages.Length; i++)
		{
			Image img = allImages[i];
			if (img != null && img.sprite != null && img.type == Image.Type.Simple)
			{
				img.preserveAspect = true;
			}
		}
	}
}
