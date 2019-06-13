using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace SaveSystem
{
	public static class SceneSaveExtension
	{

		public static void SaveSceneObjects(this SaveData save, Scene scene)
		{
			if (!scene.isLoaded)
			{
				return;
			}

			var sceneSaveables = GameObject.FindObjectsOfType<SceneSaveable>().Where(x => x.gameObject.scene == scene).Reverse().ToArray();
			var spawnedSaveables = GameObject.FindObjectsOfType<SpawnedSaveable>().Where(x => x.gameObject.scene == scene).Reverse().ToArray();

			SceneSaveData saveData = new SceneSaveData()
			{
				sceneName = scene.name,
				sceneData = new SceneObjectSaveData[sceneSaveables.Length],
				spawnedData = new SpawnedObjectSaveData[spawnedSaveables.Length]
			};

			for (int i = 0; i < sceneSaveables.Length; i++)
			{
				saveData.sceneData[i] = sceneSaveables[i].GetSaveData();
			}

			for (int i = 0; i < spawnedSaveables.Length; i++)
			{
				saveData.spawnedData[i] = spawnedSaveables[i].GetSaveData();
			}

			if (save.savedScenes == null)
			{
				save.savedScenes = new SceneSaveData[1];
				save.savedScenes[0] = saveData;
			}
			else
			{
				int idx = save.savedScenes.FirstIndexOf(x => x.sceneName == saveData.sceneName);

				if (idx > -1)
				{
					save.savedScenes[idx] = saveData;
				}
				else
				{
					save.savedScenes = save.savedScenes.Append(saveData);
				}
			}


		}

		public static void LoadSceneObjects(this SaveData save, Scene scene)
		{
			if (!scene.isLoaded)
			{
				Debug.LogWarning($"Scene {scene.name} is not loaded!");
				return;
			}

			if (save.savedScenes == null)
			{
				Debug.Log($"No saved scenes in current save.");
				return;
			}

			SceneSaveData saveData = save.savedScenes.SingleOrDefault(x => x.sceneName == scene.name);
			if (string.IsNullOrEmpty(saveData.sceneName))
			{
				Debug.Log($"No saved data found for scene: {scene.name}.");
				return;
			}

			var oldSpawned = GameObject.FindObjectsOfType<SpawnedSaveable>().Where(x => x.gameObject.scene == scene);
			foreach (var item in oldSpawned)
			{
				GameObject.DestroyImmediate(item.gameObject);
			}

			var sceneSaveables = GameObject.FindObjectsOfType<SceneSaveable>().Where(x => x.gameObject.scene == scene).ToArray();

			foreach (var objectData in saveData.sceneData)
			{
				var matching = sceneSaveables.SingleOrDefault(x => x.saveableID == objectData.saveableID);
				if (matching == null)
				{
					Debug.LogWarning($"No matching SceneSaveable found for {objectData.saveableID}.");
					continue;
				}
				matching.LoadSaveData(objectData);
			}

			foreach (var spawnedData in saveData.spawnedData.OrderBy(x => x.spawnIndex))
			{
				GameObject prefab = Resources.Load<GameObject>(spawnedData.resourcePath);
				if (prefab == null)
				{
					Debug.LogWarning($"No matching prefab found for {spawnedData.resourcePath}.");
					continue;
				}
				GameObject spawned = GameObject.Instantiate(prefab);
				SpawnedSaveable saveable = spawned.GetComponent<SpawnedSaveable>();
				if (saveable == null)
				{
					Debug.LogWarning($"No SpawnedSaveable script found in prefab {spawnedData.resourcePath}.");
					GameObject.Destroy(spawned);
					continue;
				}

				SceneManager.MoveGameObjectToScene(spawned, scene);

				saveable.LoadSaveData(spawnedData);
			}

		}

		
		public static bool HasAnythingToSave(this Scene scene)
		{
			if(!scene.isLoaded || string.IsNullOrEmpty(scene.name))
			{
				return false;
			}

			bool sceneSaveables = GameObject.FindObjectsOfType<SceneSaveable>().Where(x => x.gameObject.scene == scene).Any();
			bool spawnedSaveables = GameObject.FindObjectsOfType<SpawnedSaveable>().Where(x => x.gameObject.scene == scene).Any();

			return sceneSaveables || spawnedSaveables;
		}
	}
}