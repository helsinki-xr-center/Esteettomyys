using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
				//go.hideFlags = HideFlags.DontSave;
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

	public void LoadSceneAndUnloadCurrent(string scene)
	{
		if (SceneManager.GetSceneByName(scene).isLoaded)
		{
			Debug.Log($"Scene {scene} is already loaded", this);
		}
		StartCoroutine(LoadSceneUnloadCurrentCorotuine(scene));
	}

	private IEnumerator LoadSceneUnloadCurrentCorotuine(string scene)
	{
		Scene lastActive = SceneManager.GetActiveScene();
		var loadOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
		while (!loadOperation.isDone)
		{
			Debug.Log(loadOperation.progress);
			yield return null;
		}

		SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));

		var unloadOperation = SceneManager.UnloadSceneAsync(lastActive);

		while(!unloadOperation.isDone){
			yield return null;
		}

		Debug.Log("Scene loading done!");
	}
}
