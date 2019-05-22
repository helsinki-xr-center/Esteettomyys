using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Author: Nomi Lakkala
 * 
 * Loads a scene additively on Start.
 */
public class SceneLoaderAdditive : MonoBehaviour
{
	public string sceneName;

	void Start()
	{
		if (!string.IsNullOrEmpty(sceneName))
		{
			SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
		}
	}
}
