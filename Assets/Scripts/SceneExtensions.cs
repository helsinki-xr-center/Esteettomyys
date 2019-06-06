using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneExtensions
{
	public static Scene[] GetAllLoadedScenes()
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
