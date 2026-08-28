using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
	public static MusicManager instance;

	[SerializeField]
	private AudioSource[] hard;

	[SerializeField]
	private AudioSource[] soft;

	[SerializeField]
	private int currentSorceIndex;

	[SerializeField]
	private int currentSong;

	private float currentSongLoopDuration;

	[SerializeField]
	private AudioClip[] hardTracks;

	[SerializeField]
	private AudioClip[] softTracks;

	[SerializeField]
	private int[] BPMin;

	[SerializeField]
	private int[] BPMeasure;

	[SerializeField]
	private int[] measures;

	private int intensity = 1;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		PlaySong(0, action: false);
	}

	private void Update()
	{
		if (hard[currentSorceIndex].time >= currentSongLoopDuration)
		{
			int num = (currentSorceIndex + 1) % 2;
			hard[num].Stop();
			soft[num].Stop();
			hard[num].Play();
			soft[num].Play();
			currentSorceIndex = num;
		}
		int num2 = (currentSorceIndex + 1) % 2;
		if (hard[num2].time >= hard[num2].clip.length - 0.1f)
		{
			hard[num2].Stop();
			soft[num2].Stop();
		}
	}

	public void SetIntensity(bool action)
	{
		int num = (action ? 1 : 0);
		if (num != intensity)
		{
			intensity = num;
			StartCoroutine(CrossFade(intensity));
		}
	}

	public void UpdateVolume(float newVolume)
	{
		hard[0].volume = newVolume * (float)intensity;
		hard[1].volume = newVolume * (float)intensity;
		soft[0].volume = newVolume * (float)(1 - intensity);
		soft[1].volume = newVolume * (float)(1 - intensity);
	}

	public void PlaySong(int songNumber, bool action)
	{
		StopAllCoroutines();
		hard[0].Stop();
		hard[1].Stop();
		soft[0].Stop();
		soft[1].Stop();
		currentSong = songNumber;
		currentSongLoopDuration = (float)(60 * measures[currentSong] * BPMeasure[currentSong]) / (float)BPMin[currentSong];
		hard[0].clip = hardTracks[currentSong];
		hard[1].clip = hardTracks[currentSong];
		soft[0].clip = softTracks[currentSong];
		soft[1].clip = softTracks[currentSong];
		intensity = (action ? 1 : 0);
		float num = OptionsMenu.instance.masterVolume * OptionsMenu.instance.musicVolume;
		hard[0].volume = (float)intensity * num;
		hard[1].volume = (float)intensity * num;
		soft[0].volume = (float)(1 - intensity) * num;
		soft[1].volume = (float)(1 - intensity) * num;
		currentSorceIndex = 0;
		hard[0].Play();
		soft[0].Play();
	}

	private IEnumerator CrossFade(int newIntensity)
	{
		float startIntensity = ((newIntensity != 1) ? 1 : 0);
		float vMod = OptionsMenu.instance.masterVolume * OptionsMenu.instance.musicVolume;
		float fadeTime = 2f;
		int saftyCount = 0;
		for (float f = startIntensity; f != (startIntensity + 1f) % 2f; f = Mathf.Clamp(f + Time.unscaledDeltaTime * ((float)newIntensity - startIntensity) / fadeTime, 0f, 1f))
		{
			hard[0].volume = f * vMod;
			hard[1].volume = f * vMod;
			soft[0].volume = (1f - f) * vMod;
			soft[1].volume = (1f - f) * vMod;
			yield return null;
			saftyCount++;
			if ((float)saftyCount > 1000f * fadeTime)
			{
				Debug.LogError("Possible infinite loop");
				break;
			}
		}
		hard[0].volume = (float)newIntensity * vMod;
		hard[1].volume = (float)newIntensity * vMod;
		soft[0].volume = (float)(1 - newIntensity) * vMod;
		soft[1].volume = (float)(1 - newIntensity) * vMod;
	}

	public void FadeOut(float t)
	{
		StartCoroutine(FadeOutCo(t));
	}

	private IEnumerator FadeOutCo(float fadeTime)
	{
		float vMod = OptionsMenu.instance.masterVolume * OptionsMenu.instance.musicVolume;
		int saftyCount = 0;
		for (float f = 1f; f > 0f; f -= Time.unscaledDeltaTime / fadeTime)
		{
			hard[0].volume = (float)intensity * vMod * f;
			hard[1].volume = (float)intensity * vMod * f;
			soft[0].volume = (float)(1 - intensity) * vMod * f;
			soft[1].volume = (float)(1 - intensity) * vMod * f;
			yield return null;
			saftyCount++;
			if ((float)saftyCount > 1000f * fadeTime)
			{
				Debug.LogError("Possible infinite loop");
				break;
			}
		}
		hard[0].volume = 0f;
		hard[1].volume = 0f;
		soft[0].volume = 0f;
		soft[1].volume = 0f;
	}
}
