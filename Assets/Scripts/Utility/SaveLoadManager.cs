using SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

/**
* Author: Nomi Lakkala
* 
* <summary>
* Handles saving and loading data.
* </summary>
*/
public class SaveLoadManager : MonoBehaviour
{
	public static SaveLoadManager instance;

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
		if (!(GlobalValues.loggedIn && BackendConnector.authenticated))
		{
			return;
		}

		foreach (Scene scene in SceneExtensions.GetAllLoadedScenes())
		{
			if (scene.HasAnythingToSave())
			{
				currentData.SaveSceneObjects(scene);
			}
		}

		currentData.saveName = "savegame";
		currentData.timestamp = DateTime.Now;

		await backendSave.Save(currentData);
	}



	public async void Load()
	{
		if (!(GlobalValues.loggedIn && BackendConnector.authenticated))
		{
			return;
		}

		var backendSaves = await backendSave.GetSaveFiles();

		SaveFile newest = backendSaves.MaxBy(x => x.timestamp);

		if (newest != null)
		{
			currentData = await backendSave.Load(newest);
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

	public void ResetScene(Scene scene)
	{
		currentData.ResetSceneData(scene);
	}


	private void SceneUnloaded(Scene s)
	{

	}

	private void SceneLoaded(Scene s, LoadSceneMode mode)
	{

	}
}
