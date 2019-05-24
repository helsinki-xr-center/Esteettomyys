using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Loads a specified game scene on start and unloads it when the GameObject is destroyed.
 * </summary>
 */
public class LoadUnloadGameScene : MonoBehaviour
{

	public string sceneName;

	void Start()
	{
		if (!string.IsNullOrEmpty(sceneName))
		{
			SceneLoaderAsync.instance.LoadSceneAsync(sceneName);
		}
	}

	private void OnDestroy()
	{
		SceneLoaderAsync.instance.UnloadSceneAsync(sceneName);
	}


}
