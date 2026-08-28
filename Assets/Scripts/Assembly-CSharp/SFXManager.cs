using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
	public float volume = 0.5f;

	[SerializeField]
	private AudioClip ballistaClip;

	[SerializeField]
	private AudioClip mortarClip;

	[SerializeField]
	private AudioClip teslaClip;

	[SerializeField]
	private AudioClip[] explosions;

	[SerializeField]
	private AudioClip biPlaneGunClip;

	[SerializeField]
	private AudioClip particleCannonClip;

	[SerializeField]
	private AudioClip shredderClip;

	[SerializeField]
	private AudioClip[] ballistaHits;

	[SerializeField]
	private AudioClip frostHitClip;

	[SerializeField]
	private AudioClip shredderHitClip;

	[SerializeField]
	private AudioClip[] bulletHits;

	[SerializeField]
	private AudioClip particleHitClip;

	[SerializeField]
	private AudioClip[] coinLongClips;

	[SerializeField]
	private AudioClip[] coinShortClips;

	[SerializeField]
	private AudioClip buttonClick;

	[SerializeField]
	private AudioClip[] critSmall;

	[SerializeField]
	private AudioClip[] critBig;

	[SerializeField]
	private AudioClip cards;

	[SerializeField]
	private GameObject sourceObject;

	public List<AudioPoolSource> sources = new List<AudioPoolSource>();

	public static SFXManager instance;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		volume = OptionsMenu.instance.masterVolume * OptionsMenu.instance.sfxVolume;
	}

	public void ButtonClick()
	{
		PlaySound(Sound.ButtonClick, MusicManager.instance.transform.position, MusicManager.instance.transform);
	}

	public void PlaySound(Sound s, Vector3 pos)
	{
		PlaySound(s, pos, null);
	}

	public void PlaySound(Sound s, Vector3 pos, Transform parent)
	{
		if (!(volume <= 0f))
		{
			AudioClip clip = GetClip(s);
			AudioPoolSource audioPoolSource;
			if (sources.Count < 1)
			{
				audioPoolSource = Object.Instantiate(sourceObject).GetComponent<AudioPoolSource>();
			}
			else
			{
				audioPoolSource = sources[0];
				sources.Remove(audioPoolSource);
			}
			audioPoolSource.transform.position = pos;
			audioPoolSource.PlayClip(clip, volume, 0.08333f);
			if (parent != null)
			{
				audioPoolSource.transform.parent = parent;
			}
		}
	}

	private AudioClip GetClip(Sound s)
	{
		switch (s)
		{
		case Sound.Ballista:
			return ballistaClip;
		case Sound.Mortar:
			return mortarClip;
		case Sound.TeslaZap:
			return teslaClip;
		case Sound.Explosion:
			return explosions[Random.Range(0, explosions.Length)];
		case Sound.BiPlaneGun:
			return biPlaneGunClip;
		case Sound.ParticleCannon:
			return particleCannonClip;
		case Sound.Shredder:
			return shredderClip;
		case Sound.BallistaHit:
			return ballistaHits[Random.Range(0, ballistaHits.Length)];
		case Sound.FrostHit:
			return frostHitClip;
		case Sound.ShredderHit:
			return shredderHitClip;
		case Sound.BulletHit:
			return bulletHits[Random.Range(0, bulletHits.Length)];
		case Sound.ParticleHit:
			return particleHitClip;
		case Sound.CoinLong:
			return coinLongClips[Random.Range(0, coinLongClips.Length)];
		case Sound.CoinShort:
			return coinShortClips[Random.Range(0, coinShortClips.Length)];
		case Sound.ButtonClick:
			return buttonClick;
		case Sound.CritSmall:
			return critSmall[Random.Range(0, critSmall.Length)];
		case Sound.CritBig:
			return critBig[Random.Range(0, critBig.Length)];
		case Sound.Cards:
			return cards;
		default:
			Debug.LogError("No Audio Clip Found Type " + s);
			return null;
		}
	}
}
