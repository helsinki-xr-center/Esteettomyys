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
		saveData = SaveManager.Save("test");


		Debug.Log("Savestring: " + saveData.AsString());
		Debug.Log(saveData.AsString().Length + " bytes");
		Debug.Log("Compressed: " + saveData.AsStringCompressed());

		Debug.Log(saveData.AsStringCompressed().Length + " bytes");
		PlayerPrefs.SetString("testSave", saveData.AsStringCompressed());
		PlayerPrefs.Save();
	}

	public void LoadScene()
	{
		if(saveData == null || string.IsNullOrEmpty(saveData.saveName))
		{
			string saveString = PlayerPrefs.GetString("testSave");

			Debug.Log("Read: " + saveString);
			Debug.Log(saveString.Length + " bytes");

			saveData = SaveData.FromStringCompressed(saveString);
		}


		var spawned = FindObjectsOfType<SpawnedSaveable>();
		foreach (var item in spawned)
		{
			DestroyImmediate(item.gameObject);
		}

		SaveManager.Load(saveData);
		SaveManager.LoadSceneObjects(SceneManager.GetActiveScene());
	}

	public void SpawnObject()
	{
		Instantiate(spawnedObject, spawnLocation, Quaternion.identity, parent);
	}
}
