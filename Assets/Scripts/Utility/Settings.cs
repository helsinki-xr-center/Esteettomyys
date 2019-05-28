using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Holds current settings. Also handles loading and saving settings to computer.
 * </summary>
 */
[System.Serializable]
public class Settings
{
	private const string settings_key = "user_settings";
	private static bool loaded = false;
	private static Settings currentSettings;

	//Settings here
	public bool voiceChatEnabled = true;







	/**
	 * <summary>
	 * Returns the current settings. If not loaded, calls <see cref="Load"/> first.
	 * </summary>
	 */
	public static Settings Get()
	{
		if (!loaded)
		{
			Load();
		}
		return currentSettings;
	}

	/**
	 * <summary>
	 * Loads settings from playerprefs.
	 * </summary>
	 */
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

	/**
	 * <summary>
	 * Saves current settings to playerprefs.
	 * </summary>
	 */
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

	/**
	 * <summary>
	 * Resets settings to default.
	 * </summary>
	 */
	public static void Reset()
	{
		currentSettings = new Settings();
	}
}
