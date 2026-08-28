using UnityEngine;

public class Pathfinder : MonoBehaviour
{
	public float distanceFromEnd = 2.1474836E+09f;

	public bool atEnd;

	public float speed = 1f;

	public Waypoint currentWaypoint;

	[SerializeField]
	private Enemy enemyScript;

	private void Start()
	{
		if (enemyScript == null)
		{
			enemyScript = GetComponent<Enemy>();
		}
	}

	private void FixedUpdate()
	{
		CheckWaypointDistance();
		Move();
	}

	private void Move()
	{
		Vector3 vector = currentWaypoint.transform.position - base.transform.position;
		vector.Normalize();
		base.transform.Translate(vector * speed * Time.fixedDeltaTime);
		distanceFromEnd -= speed * Time.fixedDeltaTime;
	}

	private void CheckWaypointDistance()
	{
		if (Vector3.SqrMagnitude(currentWaypoint.transform.position - base.transform.position) < 4f * speed * speed * Time.fixedDeltaTime * Time.fixedDeltaTime && !GetNewWaypoint())
		{
			atEnd = true;
			enemyScript.AtEnd();
		}
	}

	public Vector3 GetFuturePosition(float distance)
	{
		Vector3 position = base.transform.position;
		Waypoint nextWaypoint = currentWaypoint;
		float num = distance;
		int num2 = 0;
		while (num > 0f)
		{
			if (Vector3.SqrMagnitude(nextWaypoint.transform.position - position) >= num * num)
			{
				return position + (nextWaypoint.transform.position - position).normalized * num;
			}
			if (nextWaypoint.GetNextWaypoint() == nextWaypoint)
			{
				return nextWaypoint.transform.position;
			}
			num -= Vector3.Magnitude(nextWaypoint.transform.position - position);
			position = nextWaypoint.transform.position;
			nextWaypoint = nextWaypoint.GetNextWaypoint();
			num2++;
			if (num2 > 100)
			{
				Debug.LogError("GetFuturePosition looping too much");
				break;
			}
		}
		Debug.LogError("GetFuturePosition broken");
		return Vector3.zero;
	}

	private bool GetNewWaypoint()
	{
		if (currentWaypoint.GetNextWaypoint() == currentWaypoint)
		{
			return false;
		}
		distanceFromEnd = currentWaypoint.distanceFromEnd;
		currentWaypoint = currentWaypoint.GetNextWaypoint();
		if (distanceFromEnd <= 24f)
		{
			enemyScript.CheckBattleCries(BattleCry.BattleCryTrigger.NearEnd);
		}
		return true;
	}
}
