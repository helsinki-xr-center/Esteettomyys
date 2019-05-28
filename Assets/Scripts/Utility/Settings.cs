using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Settings
{
	private const string settings_key = "user_settings";
	private static bool loaded = false;
	private static Settings currentSettings;

	//Settings start from here
	public bool voiceChatEnabled = true;








	public static Settings Get()
	{
		if (!loaded)
		{
			Load();
		}
		return currentSettings;
	}

	public static void Load()
	{
		try
		{
			if (PlayerPrefs.HasKey(settings_key))
			{
				var json = PlayerPrefs.GetString(settings_key);
				currentSettings = JsonUtility.FromJson<Settings>(json);
			}
			else
			{
				Debug.Log("No settings found! Creating new one");
				currentSettings = new Settings();
			}

		}
		catch (System.Exception ex)
		{
			currentSettings = new Settings();
		}
		loaded = true;
	}

	public static void Save()
	{
		if (currentSettings == null)
		{
			Load();
		}
		var json = JsonUtility.ToJson(currentSettings);
		PlayerPrefs.SetString(settings_key, json);
		PlayerPrefs.Save();
	}

	public static void Reset()
	{
		currentSettings = new Settings();
	}
}
