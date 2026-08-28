using UnityEngine;

public class TreasureUI : MonoBehaviour
{
	public GameObject myChest;

	private int cardDraw = 2;

	private void Start()
	{
		UIManager.instance.SetNewUI(base.gameObject);
		cardDraw = 2 + PlayerPrefs.GetInt("TreasureDraw1", 0);
	}

	public void Open()
	{
		SFXManager.instance.PlaySound(Sound.CritBig, myChest.transform.position);
		for (int i = 0; i < myChest.GetComponent<TreasureChest>().numberOfDrops; i++)
		{
			CardManager.instance.DrawCards(cardDraw);
		}
		Die();
	}

	public void Die()
	{
		Object.Destroy(myChest);
		UIManager.instance.CloseUI(base.gameObject);
	}
}
