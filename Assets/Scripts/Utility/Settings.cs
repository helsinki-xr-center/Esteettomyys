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
	public delegate void OnSettingsChangedDelegate(Settings newSettings);
	public static event OnSettingsChangedDelegate OnSettingsChanged;
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
	 * Loads settings from playerprefs. Also fires <see cref="OnSettingsChanged"/> event.
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
			Debug.LogWarning("Failed loading settings: \n" + ex);
			currentSettings = new Settings();
		}
		loaded = true;

		OnSettingsChanged?.Invoke(currentSettings);
	}

	/**
	 * <summary>
	 * Saves current settings to playerprefs. Also fires <see cref="OnSettingsChanged"/> event.
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

		OnSettingsChanged?.Invoke(currentSettings);
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
