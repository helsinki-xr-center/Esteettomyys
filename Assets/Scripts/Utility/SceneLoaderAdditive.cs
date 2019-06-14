using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

/**
 * Author: Nomi Lakkala
 * 
 * <summary>Loads a scene additively on Start.</summary>
 */
public class SceneLoaderAdditive : MonoBehaviour
{
	[FormerlySerializedAs("sceneName")]
	[Scene]
	public string scene;

	void Start()
	{
		if (!string.IsNullOrEmpty(scene) && !SceneManager.GetSceneByName(scene).isLoaded)
		{
			SceneManager.LoadScene(scene, LoadSceneMode.Additive);
		}
	}
}
