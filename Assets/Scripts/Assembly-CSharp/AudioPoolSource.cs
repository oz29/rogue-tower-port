using UnityEngine;

public class AudioPoolSource : MonoBehaviour
{
	[SerializeField]
	private AudioSource audioS;

	private float timer = -1f;

	private bool active;

	public void PlayClip(AudioClip clip, float volume, float pitchVariance)
	{
		audioS.Stop();
		audioS.clip = clip;
		audioS.pitch = 1f + Random.Range(0f - pitchVariance, pitchVariance);
		audioS.volume = volume;
		timer = clip.length + 0.1f;
		active = true;
		audioS.Play();
	}

	private void Update()
	{
		if (!active)
		{
			return;
		}
		timer -= Time.deltaTime;
		if (timer <= 0f)
		{
			active = false;
			if (base.transform.parent != null)
			{
				base.transform.parent = null;
			}
			SFXManager.instance.sources.Add(this);
		}
	}
}
