using UnityEngine;

public class Explosion : MonoBehaviour
{
	[SerializeField]
	private float duration = 2f;

	[SerializeField]
	private Sound sound;

	private void Start()
	{
		if (sound != Sound.None)
		{
			SFXManager.instance.PlaySound(sound, base.transform.position);
		}
	}

	private void FixedUpdate()
	{
		duration -= Time.fixedDeltaTime;
		if (duration <= 0f)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
