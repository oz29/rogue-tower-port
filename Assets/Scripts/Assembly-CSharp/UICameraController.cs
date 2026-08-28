using UnityEngine;

public class UICameraController : MonoBehaviour
{
	[SerializeField]
	private float cameraSpeed = 10f;

	[SerializeField]
	private float cameraZoom = 1f;

	private Vector3 oldPos;

	private void Start()
	{
	}

	private void Update()
	{
		UpdateMovement();
		UpdateZoom();
	}

	public void ResetPosition()
	{
		base.transform.localPosition = Vector3.zero;
		cameraZoom = 1f;
		base.transform.localScale = Vector3.one * cameraZoom;
	}

	private Vector3 ViewPointToPixels(Vector3 view)
	{
		return new Vector3(view.x * (float)Camera.main.scaledPixelWidth, view.y * (float)Camera.main.scaledPixelHeight, 0f);
	}

	private void UpdateMovement()
	{
		if (Input.GetMouseButtonDown(1))
		{
			oldPos = ViewPointToPixels(Camera.main.ScreenToViewportPoint(Input.mousePosition));
		}
		if (Input.GetMouseButton(1))
		{
			Vector3 vector = ViewPointToPixels(Camera.main.ScreenToViewportPoint(Input.mousePosition));
			Vector3 translation = vector - oldPos;
			translation.z = 0f;
			base.transform.Translate(translation);
			oldPos = vector;
		}
		else
		{
			Vector3 vector2 = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);
			if (vector2.sqrMagnitude > 0.1f)
			{
				base.transform.Translate(-vector2.normalized * Time.deltaTime * cameraSpeed * (6f / (5f + cameraZoom)));
			}
		}
	}

	private void UpdateZoom()
	{
		float num = cameraZoom;
		cameraZoom = Mathf.Clamp(cameraZoom + Input.mouseScrollDelta.y / 10f, 0.5f, 2f);
		base.transform.localScale = Vector3.one * cameraZoom;
		base.transform.localPosition *= cameraZoom / num;
		base.transform.localPosition += new Vector3(0f, 0.5f * (float)Camera.main.scaledPixelHeight * (cameraZoom - num), 0f);
	}
}
