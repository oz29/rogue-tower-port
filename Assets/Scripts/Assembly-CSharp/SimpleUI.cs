using UnityEngine;
using UnityEngine.UI;

public class SimpleUI : MonoBehaviour
{
	private GameObject demolishableObject;

	private int goldBackOnDemolish;

	[SerializeField]
	private Text demolishText;

	[SerializeField]
	private Text discriptionText;

	[SerializeField]
	private GameObject demolishButton;

	private void Start()
	{
		UIManager.instance.SetNewUI(base.gameObject);
	}

	public void SetDemolishable(GameObject obj, int goldReturned)
	{
		demolishableObject = obj;
		goldBackOnDemolish = goldReturned;
		demolishButton.SetActive(value: true);
		if (demolishText != null)
		{
			demolishText.text = "Demolish (" + goldBackOnDemolish + "g)";
		}
	}

	public void Demolish()
	{
		demolishableObject.GetComponent<IBuildable>()?.Demolish();
		Object.Destroy(demolishableObject);
		ResourceManager.instance.AddMoney(goldBackOnDemolish);
		SFXManager.instance.ButtonClick();
		UIManager.instance.CloseUI(base.gameObject);
	}

	public void SetDiscriptionText(string txt)
	{
		if (discriptionText != null)
		{
			discriptionText.text = txt;
		}
	}
}
