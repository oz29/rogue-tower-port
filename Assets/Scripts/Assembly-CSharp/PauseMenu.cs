using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	[SerializeField]
	private KeyCode pauseKey1;

	[SerializeField]
	private KeyCode pauseKey2;

	[SerializeField]
	private KeyCode hideUIKey;

	[SerializeField]
	private KeyCode damageTrackerKey;

	[SerializeField]
	private GameObject pauseMenu;

	[SerializeField]
	private GameObject areYouSureMenu;

	[SerializeField]
	private GameObject optionsMenu;

	[SerializeField]
	private GameObject[] hideableUI;

	[SerializeField]
	private GameObject[] hideableWithTracker;

	private bool uiHidden;

	private bool trackerHidden = true;

	public bool paused;

	public static PauseMenu instance;

	private void Awake()
	{
		instance = this;
	}

	private void Update()
	{
		if (Input.GetKeyDown(pauseKey1) || Input.GetKeyDown(pauseKey2))
		{
			if (!paused)
			{
				UnHideUI();
				Pause();
			}
			else
			{
				UnPause();
			}
		}
		if (Input.GetKeyDown(hideUIKey))
		{
			if (uiHidden)
			{
				UnHideUI();
			}
			else
			{
				hideUI();
			}
		}
		if (Input.GetKeyDown(damageTrackerKey))
		{
			if (trackerHidden)
			{
				UnHideTracker();
			}
			else
			{
				HideTracker();
			}
		}
	}

	public void Pause()
	{
		paused = true;
		Time.timeScale = 0f;
		pauseMenu.SetActive(value: true);
	}

	public void UnPause()
	{
		pauseMenu.SetActive(value: false);
		areYouSureMenu.SetActive(value: false);
		optionsMenu.SetActive(value: false);
		Time.timeScale = 1f;
		paused = false;
	}

	public void hideUI()
	{
		uiHidden = true;
		GameObject[] array = hideableUI;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: false);
		}
	}

	public void UnHideUI()
	{
		uiHidden = false;
		GameObject[] array = hideableUI;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: true);
		}
	}

	public void HideTracker()
	{
		trackerHidden = true;
		DamageTracker.instance.uiObject.SetActive(value: false);
		GameObject[] array = hideableWithTracker;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: false);
		}
	}

	public void UnHideTracker()
	{
		trackerHidden = false;
		DamageTracker.instance.DisplayDamageTotals();
		GameObject[] array = hideableWithTracker;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: true);
		}
	}
}
