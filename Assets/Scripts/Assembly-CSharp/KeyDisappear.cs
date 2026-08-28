using UnityEngine;

public class KeyDisappear : MonoBehaviour
{
	[SerializeField]
	private GameObject wKey;

	[SerializeField]
	private GameObject aKey;

	[SerializeField]
	private GameObject sKey;

	[SerializeField]
	private GameObject dKey;

	private bool w;

	private bool a;

	private bool s;

	private bool d;

	[SerializeField]
	private bool destroyOnCompletion = true;

	private void Awake()
	{
		base.gameObject.SetActive(false);
		Object.Destroy(base.gameObject);
	}
}
