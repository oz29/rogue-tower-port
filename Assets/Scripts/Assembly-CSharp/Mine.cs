using UnityEngine;

public class Mine : MonoBehaviour, IBuildable
{
	[SerializeField]
	private int goldBackOnDemolish;

	[SerializeField]
	private LayerMask layermask;

	[SerializeField]
	private GameObject UIObject;

	private bool gathering;

	private void Start()
	{
		SpawnManager.instance.mines.Add(this);
		DetectIron();
	}

	public void SetStats()
	{
	}

	public void Repair()
	{
		if (gathering && Random.Range(0f, 1f) < 0.1f)
		{
			GameManager.instance.RepairTower(10);
		}
	}

	private void SetBonus(int value)
	{
		GameManager.instance.IncreaseTowerHealth(value);
	}

	public void SpawnUI()
	{
		SimpleUI component = Object.Instantiate(UIObject, base.transform.position, Quaternion.identity).GetComponent<SimpleUI>();
		component.SetDemolishable(base.gameObject, goldBackOnDemolish);
		if (gathering)
		{
			component.SetDiscriptionText("Currently mining. Tower maximum health increased by 1 and a 10% chance to repair damage.");
		}
	}

	private void DetectIron()
	{
		if ((!Physics.Raycast(base.transform.position + new Vector3(1f, 1f, 0f), -base.transform.up, out var hitInfo, 1f, layermask, QueryTriggerInteraction.Ignore) || !Rotate(hitInfo)) && (!Physics.Raycast(base.transform.position + new Vector3(-1f, 1f, 0f), -base.transform.up, out hitInfo, 1f, layermask, QueryTriggerInteraction.Ignore) || !Rotate(hitInfo)) && (!Physics.Raycast(base.transform.position + new Vector3(0f, 1f, 1f), -base.transform.up, out hitInfo, 1f, layermask, QueryTriggerInteraction.Ignore) || !Rotate(hitInfo)) && Physics.Raycast(base.transform.position + new Vector3(0f, 1f, -1f), -base.transform.up, out hitInfo, 1f, layermask, QueryTriggerInteraction.Ignore))
		{
			Rotate(hitInfo);
		}
	}

	private bool Rotate(RaycastHit hit)
	{
		if (hit.collider.GetComponent<IronVein>() == null)
		{
			return false;
		}
		if ((double)Mathf.Abs(hit.collider.transform.position.y - base.transform.position.y) > 0.001)
		{
			return false;
		}
		base.transform.LookAt(hit.collider.transform.position, Vector3.up);
		gathering = true;
		SetBonus(1);
		return true;
	}

	public void Demolish()
	{
		SpawnManager.instance.mines.Remove(this);
		if (gathering)
		{
			SetBonus(-1);
		}
	}
}
