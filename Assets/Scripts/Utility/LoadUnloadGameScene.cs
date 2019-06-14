using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Loads a specified game scene on start and unloads it when the GameObject is destroyed.
 * </summary>
 */
public class LoadUnloadGameScene : MonoBehaviour
{
	[FormerlySerializedAs("sceneName")]
	[Scene]
	public string scene;

	void Start()
	{
		if (!string.IsNullOrEmpty(scene))
		{
			SceneLoaderAsync.instance.LoadSceneAsync(scene);
		}
	}

	private void OnDestroy()
	{
		SceneLoaderAsync.instance.UnloadSceneAsync(scene);
	}


}
