using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class SaveTransfer : MonoBehaviour
{
	private void Start()
	{
		// Auto-import bundled save if first time running on device
		if (PlayerPrefs.GetInt("SaveImportedFromPC", 0) == 0)
		{
			StartCoroutine(LoadBundledSaveCoroutine());
		}
	}

	private System.Collections.IEnumerator LoadBundledSaveCoroutine()
	{
		string filePath = Path.Combine(Application.streamingAssetsPath, "roguetower_save.json");
		string json = "";

		if (filePath.Contains("://") || filePath.Contains(":///"))
		{
			using (UnityWebRequest www = UnityWebRequest.Get(filePath))
			{
				yield return www.SendWebRequest();
				if (www.result == UnityWebRequest.Result.Success)
				{
					json = www.downloadHandler.text;
				}
			}
		}
		else if (File.Exists(filePath))
		{
			json = File.ReadAllText(filePath);
		}

		if (!string.IsNullOrEmpty(json))
		{
			ApplyJsonToPlayerPrefs(json);
			PlayerPrefs.SetInt("SaveImportedFromPC", 1);
			PlayerPrefs.Save();
			Debug.Log("[SaveTransfer] Bundled PC save imported successfully!");
		}
	}

	public static void ApplyJsonToPlayerPrefs(string json)
	{
		try
		{
			var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
			if (dict == null) return;

			foreach (var kvp in dict)
			{
				string key = kvp.Key;
				object val = kvp.Value;

				if (val is long l)
				{
					PlayerPrefs.SetInt(key, (int)l);
				}
				else if (val is int i)
				{
					PlayerPrefs.SetInt(key, i);
				}
				else if (val is double d)
				{
					PlayerPrefs.SetFloat(key, (float)d);
				}
				else if (val is float f)
				{
					PlayerPrefs.SetFloat(key, f);
				}
				else if (val is string s)
				{
					PlayerPrefs.SetString(key, s);
				}
				else if (val is bool b)
				{
					PlayerPrefs.SetInt(key, b ? 1 : 0);
				}
			}
			PlayerPrefs.Save();
		}
		catch (Exception ex)
		{
			Debug.LogError("[SaveTransfer] Error applying save: " + ex.Message);
		}
	}
}
