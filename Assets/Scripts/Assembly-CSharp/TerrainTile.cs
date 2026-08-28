using UnityEngine;

public class TerrainTile : MonoBehaviour
{
	public Waypoint last;

	[SerializeField]
	private Waypoint left;

	[SerializeField]
	private Waypoint top;

	[SerializeField]
	private Waypoint right;

	public Waypoint south;

	public Waypoint west;

	public Waypoint north;

	public Waypoint east;

	public void SetCardinalDirections()
	{
		if (base.transform.eulerAngles.y == 0f)
		{
			south = last;
			west = left;
			north = top;
			east = right;
		}
		else if (base.transform.eulerAngles.y == 90f)
		{
			south = right;
			west = last;
			north = left;
			east = top;
		}
		else if (base.transform.eulerAngles.y == 180f)
		{
			south = top;
			west = right;
			north = last;
			east = left;
		}
		else if (base.transform.eulerAngles.y == 270f)
		{
			south = left;
			west = top;
			north = right;
			east = last;
		}
		else
		{
			Debug.LogError(base.name + " not at a proper rotation. Current rotation: " + base.transform.eulerAngles.y);
		}
	}

	public void ConnectToTile(TerrainTile next)
	{
		if (base.transform.eulerAngles.y == 0f)
		{
			last.SetNextWaypoint(next.north);
			next.north.AddPreviousWaypoint(last);
		}
		else if (base.transform.eulerAngles.y == 90f)
		{
			last.SetNextWaypoint(next.east);
			next.east.AddPreviousWaypoint(last);
		}
		else if (base.transform.eulerAngles.y == 180f)
		{
			last.SetNextWaypoint(next.south);
			next.south.AddPreviousWaypoint(last);
		}
		else if (base.transform.eulerAngles.y == 270f)
		{
			last.SetNextWaypoint(next.west);
			next.west.AddPreviousWaypoint(last);
		}
		else
		{
			Debug.LogError(base.name + " not at a proper rotation");
		}
	}
}
