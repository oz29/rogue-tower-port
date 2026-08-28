using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
	public static OptionsMenu instance;

	public bool showConditionText;

	public bool showDamageNumbers = true;

	public bool extraProjectileEffects = true;

	public float masterVolume;

	public float musicVolume;

	public float sfxVolume;

	[SerializeField]
	private Transform[] scalableUIs;

	public float uiScale;

	[SerializeField]
	private Toggle showDamageToggle;

	[SerializeField]
	private Toggle showConditionToggle;

	[SerializeField]
	private Toggle projectileFXToggle;

	[SerializeField]
	private Slider masterVolumeSlider;

	[SerializeField]
	private Slider musicVolumeSlider;

	[SerializeField]
	private Slider sfxVolumeSlider;

	[SerializeField]
	private Slider uiScaleSlider;

	private float timer = 1f;

	private void Awake()
	{
		instance = this;
		Application.targetFrameRate = 60;
		QualitySettings.vSyncCount = 1;
	}

	private void Update()
	{
		if (timer >= 0f)
		{
			timer -= Time.deltaTime;
		}
	}

	private void Start()
	{
		if (PlayerPrefs.GetInt("ShowConditionText", 0) == 1)
		{
			showConditionText = true;
		}
		else
		{
			showConditionText = false;
		}
		if (showConditionToggle != null)
		{
			showConditionToggle.isOn = showConditionText;
		}
		if (PlayerPrefs.GetInt("ShowDamageNumbers", 1) == 1)
		{
			showDamageNumbers = true;
		}
		else
		{
			showDamageNumbers = false;
		}
		if (showDamageToggle != null)
		{
			showDamageToggle.isOn = showDamageNumbers;
		}
		if (PlayerPrefs.GetInt("ExtraProjectileFX", 1) == 1)
		{
			extraProjectileEffects = true;
		}
		else
		{
			extraProjectileEffects = false;
		}
		if (projectileFXToggle != null)
		{
			projectileFXToggle.isOn = extraProjectileEffects;
		}
		masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
		musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
		sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
		masterVolumeSlider.value = masterVolume;
		musicVolumeSlider.value = musicVolume;
		uiScale = PlayerPrefs.GetFloat("UIScale", 1f);
		if (uiScaleSlider != null)
		{
			uiScaleSlider.value = uiScale;
		}
		float num = Mathf.Max(uiScale + 0.5f, (uiScale + 0.5f) * 2f - 1f);
		if (scalableUIs != null)
		{
			for (int i = 0; i < scalableUIs.Length; i++)
			{
				if (scalableUIs[i] != null)
				{
					scalableUIs[i].localScale = num * Vector3.one;
				}
			}
		}
	}

	public void ToggleDamageNumbers()
	{
		showDamageNumbers = showDamageToggle.isOn;
		PlayerPrefs.SetInt("ShowDamageNumbers", showDamageNumbers ? 1 : 0);
		if (timer < 0f)
		{
			SFXManager.instance.ButtonClick();
		}
	}

	public void ToggleConditionTexts()
	{
		showConditionText = showConditionToggle.isOn;
		PlayerPrefs.SetInt("ShowConditionText", showConditionText ? 1 : 0);
		if (timer < 0f)
		{
			SFXManager.instance.ButtonClick();
		}
	}

	public void ToggleProjectileFX()
	{
		extraProjectileEffects = projectileFXToggle.isOn;
		PlayerPrefs.SetInt("ExtraProjectileFX", extraProjectileEffects ? 1 : 0);
		if (timer < 0f)
		{
			SFXManager.instance.ButtonClick();
		}
	}

	public void ChangeMasterVolume()
	{
		masterVolume = masterVolumeSlider.value;
		PlayerPrefs.SetFloat("MasterVolume", masterVolume);
		if (MusicManager.instance != null)
		{
			MusicManager.instance.UpdateVolume(masterVolume * musicVolume);
		}
		if (SFXManager.instance != null)
		{
			SFXManager.instance.volume = masterVolume * sfxVolume;
		}
	}

	public void ChangeMusicVolume()
	{
		musicVolume = musicVolumeSlider.value;
		PlayerPrefs.SetFloat("MusicVolume", musicVolume);
		if (MusicManager.instance != null)
		{
			MusicManager.instance.UpdateVolume(masterVolume * musicVolume);
		}
	}

	public void ChangeSFXVolume()
	{
		sfxVolume = sfxVolumeSlider.value;
		PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
		if (SFXManager.instance != null)
		{
			SFXManager.instance.volume = masterVolume * sfxVolume;
		}
	}

	public void ChangeUIScale()
	{
		uiScale = uiScaleSlider.value;
		PlayerPrefs.SetFloat("UIScale", uiScale);
		float num = Mathf.Max(uiScale + 0.5f, (uiScale + 0.5f) * 2f - 1f);
		Transform[] array = scalableUIs;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].localScale = num * Vector3.one;
		}
	}
}
