using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumber : MonoBehaviour
{
	[SerializeField]
	private Transform holder;

	[SerializeField]
	private Text text;

	[SerializeField]
	private int baseTextSize;

	[SerializeField]
	private float zoom = 0.25f;

	private float hold = 0.5f;

	[SerializeField]
	private float spin = 0.5f;

	[SerializeField]
	private Color healthColor;

	[SerializeField]
	private Color armorColor;

	[SerializeField]
	private Color shieldColor;

	[SerializeField]
	private Color greyColor;

	[SerializeField]
	private Color greenColor;

	public void Start()
	{
		StartCoroutine(Bloop());
	}

	public void SetNumber(int num, string color, float fontScale)
	{
		text.fontSize = (int)((float)baseTextSize * fontScale);
		text.text = num.ToString();
		switch (color)
		{
		case "Blue":
			text.color = shieldColor;
			break;
		case "Yellow":
			text.color = armorColor;
			break;
		case "Red":
			text.color = healthColor;
			break;
		case "Grey":
			text.color = greyColor;
			break;
		case "Green":
			text.color = greenColor;
			break;
		}
	}

	public void SetText(string _text, string color, float fontScale)
	{
		text.fontSize = (int)((float)baseTextSize * fontScale);
		text.text = _text;
		switch (color)
		{
		case "Blue":
			text.color = shieldColor;
			break;
		case "Yellow":
			text.color = armorColor;
			break;
		case "Red":
			text.color = healthColor;
			break;
		case "Grey":
			text.color = greyColor;
			break;
		case "Green":
			text.color = greenColor;
			break;
		}
	}

	public void SetHoldTime(float time)
	{
		hold = time;
	}

	private IEnumerator Bloop()
	{
		Vector3 scale = Vector3.zero;
		holder.localScale = scale;
		holder.localPosition = new Vector3(-1.5f, 2.12132f, 1.5f);
		text.rectTransform.localRotation = Quaternion.identity;
		float t = zoom;
		while (t > 0f)
		{
			scale += Vector3.one * (1f / zoom) * Time.deltaTime;
			holder.localScale = scale;
			t -= Time.deltaTime;
			yield return null;
		}
		scale = Vector3.one;
		holder.localScale = scale;
		t = hold;
		Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), 0f);
		while (t > 0f)
		{
			holder.localPosition += direction * Time.deltaTime;
			t -= Time.deltaTime;
			yield return null;
		}
		t = spin;
		int r = 1;
		if (Random.Range(1f, 100f) <= 50f)
		{
			r = -1;
		}
		while (t > 0f)
		{
			holder.localPosition += direction * Time.deltaTime;
			scale -= Vector3.one * (1f / spin) * Time.deltaTime;
			holder.localScale = scale;
			text.rectTransform.eulerAngles += r * 180 * Vector3.forward * Time.deltaTime / spin;
			t -= Time.deltaTime;
			yield return null;
		}
		ObjectPool.instance.PoolDamageNumber(base.gameObject);
	}
}
