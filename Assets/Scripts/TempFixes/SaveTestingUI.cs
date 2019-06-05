using SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveTestingUI : MonoBehaviour
{
	public GameObject spawnedObject;
	public Vector3 spawnLocation;
	public SceneSaveData saveData;
	public Transform parent;

	public void SaveScene()
	{
		saveData = SaveManager.SaveSceneObjects(SceneManager.GetActiveScene());
		string saveString = SaveSerializer.Serialize(saveData);

		Debug.Log("Savestring: " + saveString);
		PlayerPrefs.SetString("testSave", saveString);
		PlayerPrefs.Save();
	}

	public void LoadScene()
	{
		if(string.IsNullOrEmpty(saveData.sceneName))
		{
			string saveString = PlayerPrefs.GetString("testSave");
			saveData = SaveSerializer.Deserialize<SceneSaveData>(saveString);
		}
		var spawned = FindObjectsOfType<SpawnedSaveable>();
		foreach (var item in spawned)
		{
			Destroy(item.gameObject);
		}

		SaveManager.LoadSceneObjects(saveData);
	}

	public void SpawnObject()
	{
		Instantiate(spawnedObject, spawnLocation, Quaternion.identity, parent);
	}
}
