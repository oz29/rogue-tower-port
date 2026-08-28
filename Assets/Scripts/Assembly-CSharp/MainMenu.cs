using UnityEngine;

public class MainMenu : MonoBehaviour
{
	private void Awake()
	{
		if (Object.FindObjectOfType<SaveTransfer>() == null)
		{
			base.gameObject.AddComponent<SaveTransfer>();
		}
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
