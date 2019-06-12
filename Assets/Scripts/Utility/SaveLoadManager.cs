using SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class SaveLoadManager : MonoBehaviour
{
	public static SaveLoadManager instance;

	private PlayerPrefsSaveFileManager playerPrefsSave = PlayerPrefsSaveFileManager.instance;
	private BackendSaveFileManager backendSave = BackendSaveFileManager.instance;

	public SaveData currentData = new SaveData();

	private void Awake()
	{
		instance = this;
		SceneManager.sceneLoaded += SceneLoaded;
		SceneManager.sceneUnloaded += SceneUnloaded;
	}

	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= SceneLoaded;
		SceneManager.sceneUnloaded -= SceneUnloaded;
	}

	public async void Save()
	{
		foreach(Scene scene in SceneExtensions.GetAllLoadedScenes())
		{
			if (scene.HasAnythingToSave())
			{
				currentData.SaveSceneObjects(scene);
			}
		}

		currentData.saveName = "savegame";
		currentData.timestamp = DateTime.Now;

		if(GlobalValues.loggedIn && BackendConnector.authenticated)
		{
			await backendSave.Save(currentData);
		}
		else
		{
			playerPrefsSave.Save(currentData);
		}
	}



	public async void Load()
	{
		var prefsSaves = playerPrefsSave.GetSaveFiles();
		var backendSaves = (GlobalValues.loggedIn && BackendConnector.authenticated) ? await backendSave.GetSaveFiles() : Array.Empty<SaveFile>();

		SaveFile newest = prefsSaves.Concat(backendSaves).MaxBy(x => x.timestamp);
		
		if(newest != null)
		{
			if(prefsSaves.Contains(newest))
			{
				currentData = playerPrefsSave.Load(newest);
			}
			else
			{
				currentData = await backendSave.Load(newest);
			}
		}
		else
		{
			currentData = new SaveData();
		}

		foreach (Scene scene in SceneExtensions.GetAllLoadedScenes())
		{
			currentData.LoadSceneObjects(scene);
		}
	}


	private void SceneUnloaded(Scene s)
	{
		
	}

	private void SceneLoaded(Scene s, LoadSceneMode mode)
	{
		
	}
}
