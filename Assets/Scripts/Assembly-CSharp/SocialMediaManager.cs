using UnityEngine;

public class SocialMediaManager : MonoBehaviour
{
	public void OpenLink(string s)
	{
		Application.OpenURL(s);
	}
}
