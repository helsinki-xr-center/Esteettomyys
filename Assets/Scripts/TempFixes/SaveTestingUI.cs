using SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO.Compression;
using System.IO;
using System.Text;
using System;

/**
* Author: Nomi Lakkala
* 
* <summary>
* Testing UI for saving and loading.
* </summary>
*/
public class SaveTestingUI : MonoBehaviour
{
	public GameObject spawnedObject;
	public Vector3 spawnLocation;
	public SaveData saveData;
	public Transform parent;

	/**
	* <summary>
	* Saves everything. Called from unity Button.
	* </summary>
	*/
	public void SaveScene()
	{
		SaveLoadManager.instance.Save();

		/*
		PlayerPrefsSaveFileManager.instance.Save(saveData);


		Debug.Log("Savestring: " + saveData.AsString());
		Debug.Log(saveData.AsString().Length + " bytes");
		Debug.Log("Compressed: " + saveData.AsStringCompressed());
		Debug.Log(saveData.AsStringCompressed().Length + " bytes");


		FileSystemSaveFileManager.Default.Save(saveData);*/
	}

	/**
	* <summary>
	* Loads everything. Called from unity Button.
	* </summary>
	*/
	public void LoadScene()
	{
		SaveLoadManager.instance.Load();

		/*
		var saveFiles = PlayerPrefsSaveFileManager.instance.GetSaveFiles();

		if(saveFiles.Length > 0)
		{
			saveData = PlayerPrefsSaveFileManager.instance.Load(saveFiles[0]);
			SaveManager.LoadSaveData(saveData);
			SaveManager.LoadSceneObjects(SceneManager.GetActiveScene());
		}

		var files = FileSystemSaveFileManager.Default.GetSaveFiles();
		foreach(var file in files)
		{
			Debug.Log(file.saveName);
			var thing = FileSystemSaveFileManager.Default.Load(file);
			Debug.Log(thing.AsString());
		}*/
	}

	/**
	* <summary>
	* Called from unity Button.
	* </summary>
	*/
	public void SpawnObject()
	{
		Instantiate(spawnedObject, spawnLocation, Quaternion.identity, parent);
	}
}
