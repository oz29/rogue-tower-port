using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
	[SerializeField]
	private Waypoint next;

	[SerializeField]
	private List<Waypoint> previous = new List<Waypoint>();

	public float distanceFromEnd;

	public bool trueDistance;

	private void Start()
	{
		UpdateDistance();
	}

	public Waypoint GetNextWaypoint()
	{
		if (next != null)
		{
			return next;
		}
		return this;
	}

	public void SetNextWaypoint(Waypoint newNext)
	{
		next = newNext;
	}

	public void AddPreviousWaypoint(Waypoint previousWaypoint)
	{
		previous.Add(previousWaypoint);
	}

	public Waypoint[] GetPreviousWaypoints()
	{
		return previous.ToArray();
	}

	public void UpdateDistance()
	{
		if (next == null)
		{
			trueDistance = true;
			return;
		}
		if (!next.trueDistance)
		{
			next.UpdateDistance();
		}
		if (!trueDistance)
		{
			distanceFromEnd = next.distanceFromEnd + Vector3.Magnitude(next.transform.position - base.transform.position);
			trueDistance = true;
		}
	}
}
