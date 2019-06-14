using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SaveSystem
{

	/**
	 * Author: Nomi Lakkala
	 * 
	 * <summary>
	 * Handles saving and loading <see cref="SaveData"/> to and from Unity PlayerPrefs.
	 * </summary>
	 */
	public class PlayerPrefsSaveFileManager : ISaveFileManager
	{
		public static PlayerPrefsSaveFileManager instance { get; } = new PlayerPrefsSaveFileManager();
		private const string saveFileKey = "savesystem-save";
		private static SaveData cachedData = null;
		private static SaveFile[] cachedSaveFiles = null;

		public SaveFile[] GetSaveFiles()
		{
			if (cachedData != null && cachedSaveFiles != null)
			{
				return cachedSaveFiles;
			}
			else if(cachedData != null)
			{
				cachedSaveFiles = new SaveFile[] { new SaveFile() { saveName = cachedData.saveName, timestamp = cachedData.timestamp } };
				return cachedSaveFiles;
			}
			if (PlayerPrefs.HasKey(saveFileKey))
			{
				try
				{
					cachedData = SaveData.FromStringCompressed(PlayerPrefs.GetString(saveFileKey));
				}
				catch (Exception ex)
				{
					Debug.LogWarning("Can't load save file from playerprefs: \n" + ex);
					return Array.Empty<SaveFile>();
				}

				cachedSaveFiles = new SaveFile[] { new SaveFile() { saveName = cachedData.saveName, timestamp = cachedData.timestamp } };
				return cachedSaveFiles;
			}
			else
			{
				return Array.Empty<SaveFile>();
			}
		}

		public SaveData Load(SaveFile file)
		{
			if(file.saveName != cachedData.saveName)
			{
				Debug.LogError($"Savefile {file.saveName} does not exist!");
				return null;
			}
			else
			{
				return cachedData;
			}
		}

		public SaveFile Save(SaveData data)
		{
			cachedSaveFiles = null;
			cachedData = data;
			PlayerPrefs.SetString(saveFileKey, data.AsStringCompressed());
			PlayerPrefs.Save();
			return new SaveFile() { saveName = cachedData.saveName, timestamp = cachedData.timestamp };
		}

		public void Delete(SaveFile file)
		{
			if (file.saveName != cachedData.saveName)
			{
				Debug.LogError($"Savefile {file.saveName} does not exist!");
				return;
			}
			else
			{
				PlayerPrefs.DeleteKey(saveFileKey);
				PlayerPrefs.Save();
			}
		}
	}
}