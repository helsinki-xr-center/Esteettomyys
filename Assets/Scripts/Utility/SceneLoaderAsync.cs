using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Loads scenes asynchronously. If instance is not found, will create a new GameObject with this Script attached.
 * </summary>
 */
public class SceneLoaderAsync : MonoBehaviour
{
	private static SceneLoaderAsync p_instance;
	public static SceneLoaderAsync instance
	{
		get
		{
			if (p_instance == null)
			{
				GameObject go = new GameObject("SceneLoaderAsync");
				go.AddComponent<SceneLoaderAsync>();
				go.hideFlags = HideFlags.DontSave;
			}

			return p_instance;
		}
	}

	void Awake()
	{
		if (p_instance != null)
		{
			Destroy(gameObject);
			return;
		}
		p_instance = this;
		DontDestroyOnLoad(this);
	}


	/**
	 * <summary>
	 * Starts a coroutine that will load the desired scene, set it active and unload the old active scene.
	 * </summary>
	 */
	public void LoadSceneAndUnloadCurrent(string scene)
	{
		if (SceneManager.GetSceneByName(scene).isLoaded)
		{
			Debug.Log($"Scene {scene} is already loaded", this);
		}
		StartCoroutine(LoadSceneUnloadCurrentCorotuine(scene));
	}

	/**
	 * <summary>
	 * Starts a coroutine that will load the desired scene
	 * </summary>
	 */
	public void LoadSceneAsync(string scene)
	{
		if (SceneManager.GetSceneByName(scene).isLoaded)
		{
			Debug.Log($"Scene {scene} is already loaded", this);
		}
		StartCoroutine(LoadSceneAdditiveAsyncCoroutine(scene));
	}

	/**
	 * <summary>
	 * Starts a coroutine that will unload the desired scene.
	 * </summary>
	 */
	public void UnloadSceneAsync(string scene)
	{
		if (!SceneManager.GetSceneByName(scene).isLoaded)
		{
			Debug.Log($"Scene {scene} is not loaded", this);
		}
		StartCoroutine(UnloadSceneAsyncCoroutine(scene));
	}


	/**
	 * <summary>
	 * Coroutine that handles loading and unloading the scene.
	 * </summary>
	 */
	private IEnumerator LoadSceneUnloadCurrentCorotuine(string scene)
	{
		Scene lastActive = SceneManager.GetActiveScene();
		var loadOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
		while (!loadOperation.isDone)
		{
			yield return null;
		}

		SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));

		var unloadOperation = SceneManager.UnloadSceneAsync(lastActive);

		while(!unloadOperation.isDone){
			yield return null;
		}

		Debug.Log("Scene loading done!");
	}


	private IEnumerator LoadSceneAdditiveAsyncCoroutine(string scene)
	{
		var loadOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
		while (!loadOperation.isDone)
		{
			
			yield return null;
		}

		Debug.Log("Scene loading done!");
	}

	private IEnumerator UnloadSceneAsyncCoroutine(string scene)
	{
		var unloadOperation = SceneManager.UnloadSceneAsync(scene);

		while (!unloadOperation.isDone)
		{
			yield return null;
		}

		Debug.Log("Scene unloading done!");
	}
}
