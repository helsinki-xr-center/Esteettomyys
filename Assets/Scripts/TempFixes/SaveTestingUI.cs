using SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO.Compression;
using System.IO;
using System.Text;
using System;

public class SaveTestingUI : MonoBehaviour
{
	public GameObject spawnedObject;
	public Vector3 spawnLocation;
	public SaveData saveData;
	public Transform parent;

	public void SaveScene()
	{
		SaveManager.SaveSceneObjects(SceneManager.GetActiveScene());
		saveData = SaveManager.GetSaveData("test3");

		PlayerPrefsSaveFileManager.instance.Save(saveData);


		Debug.Log("Savestring: " + saveData.AsString());
		Debug.Log(saveData.AsString().Length + " bytes");
		Debug.Log("Compressed: " + saveData.AsStringCompressed());
		Debug.Log(saveData.AsStringCompressed().Length + " bytes");


		FileSystemSaveFileManager.Default.Save(saveData);
	}

	public void LoadScene()
	{
		var spawned = FindObjectsOfType<SpawnedSaveable>();
		foreach (var item in spawned)
		{
			DestroyImmediate(item.gameObject);
		}

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
		}
	}

	public void SpawnObject()
	{
		Instantiate(spawnedObject, spawnLocation, Quaternion.identity, parent);
	}
}
