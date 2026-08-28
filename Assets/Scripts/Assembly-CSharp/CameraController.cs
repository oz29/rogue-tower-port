using UnityEngine;

public class CameraController : MonoBehaviour
{
	public static CameraController instance;

	[SerializeField]
	private Vector3 velocity = Vector3.zero;

	[SerializeField]
	private float cameraSpeed = 10f;

	[SerializeField]
	private float cameraBaseZoom = 10f;

	[SerializeField]
	private GameObject cameraHolder;

	[SerializeField]
	private Transform audioListenerObject;

	[SerializeField]
	private LayerMask zeroMask;

	private Vector3 clickMoveOrigin;

	private Vector3 cameraOrigin;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		Camera.main.orthographicSize = cameraBaseZoom;
		if (audioListenerObject != null)
		{
			audioListenerObject.position = new Vector3(audioListenerObject.position.x, cameraBaseZoom, audioListenerObject.position.z);
		}
	}

	private void Update()
	{
		UpdateMovement();
		UpdateZoom();
	}

	private bool isTouchDragging;
	private float prevTouchDeltaMag;

	private void UpdateMovement()
	{
		// Mobile Touch Controls
		if (Input.touchCount == 1)
		{
			Touch touch = Input.GetTouch(0);
			if (UnityEngine.EventSystems.EventSystem.current == null || !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId))
			{
				if (touch.phase == TouchPhase.Began)
				{
					if (Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out var hitInfo, 2000f, zeroMask, QueryTriggerInteraction.Collide))
					{
						clickMoveOrigin = hitInfo.point;
						cameraOrigin = base.transform.position;
						isTouchDragging = true;
					}
				}
				else if (touch.phase == TouchPhase.Moved && isTouchDragging)
				{
					if (Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out var hitInfo2, 2000f, zeroMask, QueryTriggerInteraction.Collide))
					{
						Vector3 vector = cameraOrigin - 2f * (hitInfo2.point - clickMoveOrigin);
						base.transform.position = (base.transform.position + vector) / 2f;
					}
				}
				else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
				{
					isTouchDragging = false;
				}
			}
			return;
		}
		else
		{
			isTouchDragging = false;
		}

		// Mouse & Keyboard Controls
		if (Input.GetKeyDown(KeyCode.C))
		{
			base.transform.position = Vector3.zero;
		}
		if (Input.GetMouseButtonDown(1) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hitInfo3, 2000f, zeroMask, QueryTriggerInteraction.Collide))
		{
			clickMoveOrigin = hitInfo3.point;
			cameraOrigin = base.transform.position;
		}
		if (Input.GetMouseButton(1))
		{
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hitInfo4, 2000f, zeroMask, QueryTriggerInteraction.Collide))
			{
				Vector3 vector2 = cameraOrigin - 2f * (hitInfo4.point - clickMoveOrigin);
				base.transform.position = (base.transform.position + vector2) / 2f;
			}
			return;
		}
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			velocity *= 1f - Time.deltaTime;
			velocity += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")) * Time.deltaTime * 2f;
			float num = Mathf.Clamp(Camera.main.orthographicSize / 10f, 1f, 5f);
			base.transform.Translate(velocity * Time.deltaTime * cameraSpeed * num);
			return;
		}
		velocity = Vector3.zero;
		Vector3 vector3 = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
		if (vector3.sqrMagnitude > 0.1f)
		{
			float num2 = Mathf.Clamp(Camera.main.orthographicSize / 10f, 1f, 5f);
			base.transform.Translate(vector3.normalized * Time.deltaTime * cameraSpeed * num2);
		}
	}

	private void UpdateZoom()
	{
		float currentZoom = Camera.main.orthographicSize;

		// 2-Finger Pinch to Zoom for Mobile
		if (Input.touchCount == 2)
		{
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);

			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

			// Smooth, resolution-independent pinch sensitivity
			float screenScale = Mathf.Max(Screen.height, Screen.width, 1000f);
			float difference = (prevMagnitude - currentMagnitude) / screenScale * 18f;
			currentZoom = Mathf.Clamp(currentZoom + difference, 1f, 50f);
		}
		else
		{
			// Mouse scroll wheel
			currentZoom = Mathf.Clamp(currentZoom - Input.mouseScrollDelta.y, 1f, 50f);
		}

		Camera.main.orthographicSize = currentZoom;
		cameraHolder.transform.localPosition = new Vector3(0f, 5f + 2f * currentZoom, -2f * currentZoom - 5f);
		if (audioListenerObject != null)
		{
			audioListenerObject.position = new Vector3(audioListenerObject.position.x, (currentZoom + 10f) / 2f, audioListenerObject.position.z);
		}
	}
}
