using System;
using UnityEngine;

public class WalkAnimator : MonoBehaviour
{
	private Pathfinder p;

	[SerializeField]
	private Transform directionalTransform;

	[SerializeField]
	private Transform rotationalOffsetTransform;

	[SerializeField]
	private Transform artTransform;

	[SerializeField]
	private float strideLength = 1f;

	[SerializeField]
	private float strideHeight = 1f;

	[SerializeField]
	private float sideAngles = 20f;

	[SerializeField]
	private float strideVariationPercentage = 0.1f;

	private float step;

	private Vector3 preiviousPosition;

	private Vector3 direction;

	private void Start()
	{
		strideLength = UnityEngine.Random.Range(strideLength * (1f - strideVariationPercentage), strideLength * (1f + strideVariationPercentage));
		strideHeight = UnityEngine.Random.Range(strideHeight * (1f - strideVariationPercentage), strideHeight * (1f + strideVariationPercentage));
		p = GetComponent<Pathfinder>();
		preiviousPosition = base.transform.position;
	}

	private void FixedUpdate()
	{
		direction = base.transform.position - preiviousPosition;
		preiviousPosition = base.transform.position;
		if (direction.sqrMagnitude != 0f)
		{
			directionalTransform.rotation = Quaternion.LookRotation(direction, Vector3.up);
		}
		step = (step + p.speed * Time.fixedDeltaTime / strideLength) % 2f;
		artTransform.localPosition = strideHeight * Vector3.up * Mathf.Abs(Mathf.Cos(step * (float)Math.PI));
		artTransform.localEulerAngles = new Vector3(sideAngles * Mathf.Sin(step * (float)Math.PI), 0f, 0f);
	}
}
