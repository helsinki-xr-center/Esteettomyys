using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Loads multiple scenes on Awake and unloads them when the GameObject is destroyed.
 * </summary>
 */
public class LoadUnloadMultipleScenes : MonoBehaviour
{
	[Scene]
	public string[] scenes;

    private void Awake()
    {
        foreach(string scene in scenes)
		{
			if (!string.IsNullOrEmpty(scene))
			{
				SceneLoaderAsync.instance.LoadSceneAsync(scene);
			}
		}
    }

	private void OnDestroy()
	{
		foreach (string scene in scenes)
		{
			if (!string.IsNullOrEmpty(scene))
			{
				SceneLoaderAsync.instance.UnloadSceneAsync(scene);
			}
		}
	}
}
