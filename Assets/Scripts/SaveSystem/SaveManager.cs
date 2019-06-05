using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace SaveSystem
{
	public static class SaveManager
	{
		private static SaveData currentData = new SaveData();


		public static SaveData Save(string saveName)
		{
			currentData.saveName = saveName;
			currentData.timestamp = System.DateTime.Now;
			return currentData;
		}

		public static void Load(SaveData data) => currentData = data;


		public static void SaveSceneObjects(Scene scene)
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

			if (currentData.savedScenes == null)
			{
				currentData.savedScenes = new SceneSaveData[1];
				currentData.savedScenes[0] = saveData;
			}
			else
			{
				int idx = currentData.savedScenes.FirstIndexOf(x => x.sceneName == saveData.sceneName);

				if (idx > -1)
				{
					currentData.savedScenes[idx] = saveData;
				}
				else
				{
					currentData.savedScenes = currentData.savedScenes.Append(saveData);
				}
			}


		}

		public static void LoadSceneObjects(Scene scene)
		{
			if (!scene.isLoaded)
			{
				Debug.LogWarning($"Scene {scene.name} is not loaded!");
				return;
			}

			if(currentData.savedScenes == null)
			{
				Debug.Log($"No saved scenes in current save. Have you called SaveManager.Load ?");
				return;
			}

			SceneSaveData saveData = currentData.savedScenes.SingleOrDefault(x => x.sceneName == scene.name);
			if (string.IsNullOrEmpty(saveData.sceneName))
			{
				Debug.Log($"No saved data found for {scene.name}.");
				return;
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


		private static Scene[] GetAllLoadedScenes()
		{
			int countLoaded = SceneManager.sceneCount;
			Scene[] loadedScenes = new Scene[countLoaded];

			for (int i = 0; i < countLoaded; i++)
			{
				loadedScenes[i] = SceneManager.GetSceneAt(i);
			}

			return loadedScenes;
		}
	}
}