using UnityEngine;

public class DevelopmentManager : MonoBehaviour
{
	[SerializeField]
	private GameObject[] startingDevelopments;

	private float developmentPercent;

	private void Start()
	{
		developmentPercent = PlayerPrefs.GetInt("UnlockedCardCount", 0) + PlayerPrefs.GetInt("Development", 0);
		developmentPercent *= 0.4f;
		GameObject[] array = startingDevelopments;
		foreach (GameObject obj in array)
		{
			if (Random.Range(0f, 100f) > developmentPercent)
			{
				Object.Destroy(obj);
			}
		}
	}
}
