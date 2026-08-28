using UnityEngine;
using UnityEngine.UI;

public class BuildingGhost : MonoBehaviour
{
	[SerializeField]
	private Text text;

	public void SetText(string newText)
	{
		text.text = newText;
	}
}
