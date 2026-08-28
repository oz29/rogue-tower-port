using UnityEngine;
using UnityEngine.UI;

public class PlayMenu : MonoBehaviour
{
	[SerializeField]
	private Text record1Text;

	[SerializeField]
	private Text record2Text;

	[SerializeField]
	private Text record3Text;

	private void Start()
	{
		record1Text.text = "Current Record\nLevel " + PlayerPrefs.GetInt("Record1", 0);
		record2Text.text = "Current Record\nLevel " + PlayerPrefs.GetInt("Record2", 0);
		record3Text.text = "Current Record\nLevel " + PlayerPrefs.GetInt("Record3", 0);
	}

	public void StartGame(int mode)
	{
		PlayerPrefs.SetInt("GameMode", Mathf.Clamp(mode, 1, 3));
		LevelLoader.instance.LoadLevel("GameScene");
	}
}
