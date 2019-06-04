using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveSystem
{
	public static class SaveManager
	{
		public static SceneSaveData SaveSceneObjects(Scene scene)
		{
			if (!scene.isLoaded)
			{
				return default(SceneSaveData);
			}

			var sceneSaveables = GameObject.FindObjectsOfType<SceneSaveable>().Where(x => x.gameObject.scene == scene).ToArray();
			var spawnedSaveables = GameObject.FindObjectsOfType<SpawnedSaveable>().Where(x => x.gameObject.scene == scene).ToArray();

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

			return saveData;
		}

		public static void LoadSceneObjects(SceneSaveData saveData)
		{
			var scenes = GetAllLoadedScenes();

			Scene scene = scenes.SingleOrDefault(x => x.name == saveData.sceneName);
			if (scene == null)
			{
				Debug.LogWarning($"Scene {saveData.sceneName} that matches saveData is not loaded!");
				return;
			}

			var sceneSaveables = GameObject.FindObjectsOfType<SceneSaveable>().Where(x => x.gameObject.scene == scene).ToArray();
			
			foreach(var objectData in saveData.sceneData)
			{
				var matching = sceneSaveables.SingleOrDefault(x => x.saveableID == objectData.saveableID);
				if(matching == null)
				{
					Debug.LogWarning($"No matching SceneSaveable found for {objectData.saveableID}.");
					continue;
				}
				matching.LoadSaveData(objectData);
			}

			foreach (var spawnedData in saveData.spawnedData)
			{
				GameObject prefab = Resources.Load<GameObject>(spawnedData.resourcePath);
				if(prefab == null)
				{
					Debug.LogWarning($"No matching prefab found for {spawnedData.resourcePath}.");
					continue;
				}
				GameObject spawned = GameObject.Instantiate(prefab);
				SpawnedSaveable saveable = spawned.GetComponent<SpawnedSaveable>();
				if(saveable == null)
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