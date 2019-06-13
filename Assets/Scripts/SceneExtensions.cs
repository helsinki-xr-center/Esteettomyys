using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Contains small function(s) for that help with unity scnenes.
 * </summary>
 */
public static class SceneExtensions
{
	/**
	 * <summary>
	 * Returns an array of all the currently loaded scenes.
	 * </summary>
	 */
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
