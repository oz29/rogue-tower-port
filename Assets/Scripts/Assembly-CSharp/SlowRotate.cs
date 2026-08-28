using UnityEngine;

public class SlowRotate : MonoBehaviour
{
	[SerializeField]
	private Vector3 rotation;

	private void Update()
	{
		base.transform.localEulerAngles += rotation * Time.deltaTime;
	}
}
