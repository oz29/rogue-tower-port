using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
	public static LevelLoader instance;

	[SerializeField]
	private Image circle;

	[SerializeField]
	private Text loadingText;

	[SerializeField]
	private Text tipText;

	[SerializeField]
	private string[] tips;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		loadingText.text = "";
		tipText.text = "";
		StartCoroutine(CircleExitStart());
	}

	public void LoadLevel(string sceneName)
	{
		if (MusicManager.instance != null)
		{
			MusicManager.instance.FadeOut(2.5f);
		}
		StartCoroutine(LoadAsyncronously(sceneName));
	}

	private IEnumerator LoadAsyncronously(string sceneName)
	{
		float num = Mathf.Sqrt(Screen.height * Screen.width) * 1.1f;
		Vector2 endSize = new Vector2(2f, 1f) * num * 2f;
		Vector2 startSize = new Vector2(0f, 1f) * num * 2f;
		circle.rectTransform.sizeDelta = startSize;
		Vector3 endPosition = new Vector3((float)Screen.height / 2f, (float)Screen.height / 2f, 0f);
		Vector3 startPosition = new Vector3((float)Screen.width * 1.1f, (float)Screen.height / 2f, 0f);
		circle.transform.position = startPosition;
		float t = 0f;
		while (t < 1f)
		{
			circle.rectTransform.sizeDelta = Vector2.Lerp(startSize, endSize, t);
			circle.transform.position = Vector3.Lerp(startPosition, endPosition, t);
			t += Time.deltaTime / 1.5f;
			yield return null;
		}
		circle.rectTransform.sizeDelta = endSize;
		circle.transform.position = endPosition;
		tipText.text = GetTipsText();
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
		operation.allowSceneActivation = false;
		float waitFill = 0f;
		while (!operation.isDone || waitFill < 1f)
		{
			waitFill = Mathf.Clamp(waitFill + Time.deltaTime / 3f, 0f, 1f);
			float num2 = Mathf.Clamp(operation.progress / 0.9f, 0f, 1f);
			if (waitFill >= 1f)
			{
				operation.allowSceneActivation = true;
			}
			loadingText.text = GetLoadingText((waitFill * num2 + waitFill) / 2f);
			yield return null;
		}
	}

	private string GetLoadingText(float percent)
	{
		return "Loading...".Substring(0, (int)Mathf.Round(percent * 10f));
	}

	private string GetTipsText()
	{
		int num = Random.Range(0, tips.Length);
		int num2 = PlayerPrefs.GetInt("PreviousTip", 0);
		if (num == num2)
		{
			num = (num + 1) % tips.Length;
		}
		PlayerPrefs.SetInt("PreviousTip", num);
		return tips[num];
	}

	private IEnumerator CircleExitStart()
	{
		float num = Mathf.Sqrt(Screen.height * Screen.width) * 1.1f;
		Vector2 startSize = new Vector2(2f, 1f) * num * 2f;
		Vector2 endSize = new Vector2(0f, 1f) * num * 2f;
		circle.rectTransform.sizeDelta = startSize;
		Vector3 startPosition = new Vector3((float)Screen.height / 2f, (float)Screen.height / 2f, 0f);
		Vector3 endPosition = new Vector3((float)(-Screen.width) * 0.1f, (float)Screen.height / 2f, 0f);
		circle.transform.position = startPosition;
		float t = 0f;
		while (t < 1f)
		{
			circle.rectTransform.sizeDelta = Vector2.Lerp(startSize, endSize, t);
			circle.transform.position = Vector3.Lerp(startPosition, endPosition, t);
			t += Time.deltaTime / 1.5f;
			yield return null;
		}
		circle.rectTransform.sizeDelta = endSize;
		circle.transform.position = endPosition;
	}
}
