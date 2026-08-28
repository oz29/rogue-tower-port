using UnityEngine;

public class ManaSiphon : MonoBehaviour, IBuildable
{
	[SerializeField]
	private int gatherRatePerSec;

	[SerializeField]
	private LayerMask layermask;

	[SerializeField]
	private GameObject UIObject;

	[SerializeField]
	private int goldBackOnDemolish;

	private bool gathering;

	private float timer = 1f;

	private void Start()
	{
		DetectMana();
		if (gathering)
		{
			ResourceManager.instance.UpdateManaGatherRate(gatherRatePerSec);
		}
	}

	private void Update()
	{
		if (timer <= 0f)
		{
			Gather();
			timer = 1f;
		}
		else if (gathering && SpawnManager.instance.combat)
		{
			timer -= Time.deltaTime;
		}
	}

	public void SetStats()
	{
	}

	public void SpawnUI()
	{
		SimpleUI component = Object.Instantiate(UIObject, base.transform.position, Quaternion.identity).GetComponent<SimpleUI>();
		component.SetDemolishable(base.gameObject, goldBackOnDemolish);
		if (gathering)
		{
			component.SetDiscriptionText("Currently gathering 1 mana/sec.");
		}
	}

	public void Demolish()
	{
		if (gathering)
		{
			ResourceManager.instance.UpdateManaGatherRate(-gatherRatePerSec);
		}
	}

	private void Gather()
	{
		ResourceManager.instance.AddMana(gatherRatePerSec);
	}

	private void DetectMana()
	{
		if ((!Physics.Raycast(base.transform.position + new Vector3(1f, 1f, 0f), -base.transform.up, out var hitInfo, 1f, layermask, QueryTriggerInteraction.Ignore) || !Rotate(hitInfo)) && (!Physics.Raycast(base.transform.position + new Vector3(-1f, 1f, 0f), -base.transform.up, out hitInfo, 1f, layermask, QueryTriggerInteraction.Ignore) || !Rotate(hitInfo)) && (!Physics.Raycast(base.transform.position + new Vector3(0f, 1f, 1f), -base.transform.up, out hitInfo, 1f, layermask, QueryTriggerInteraction.Ignore) || !Rotate(hitInfo)) && Physics.Raycast(base.transform.position + new Vector3(0f, 1f, -1f), -base.transform.up, out hitInfo, 1f, layermask, QueryTriggerInteraction.Ignore))
		{
			Rotate(hitInfo);
		}
	}

	private bool Rotate(RaycastHit hit)
	{
		if (hit.collider.GetComponent<ManaCrystal>() == null)
		{
			return false;
		}
		if ((double)Mathf.Abs(hit.collider.transform.position.y - base.transform.position.y) > 0.001)
		{
			return false;
		}
		base.transform.LookAt(hit.collider.transform.position, Vector3.up);
		gathering = true;
		return true;
	}
}
