using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadVRScene : MonoBehaviour
{
	public string vrSceneName = "VRPlayer";

    void Awake()
    {
		Scene scene = SceneManager.GetSceneByName(vrSceneName);
		if(scene == null){
			Debug.LogError("No VR scene found by the name: " + vrSceneName);
			return;
		}

		if(scene.isLoaded){
			Debug.Log("VR scene already loaded");
			return;
		}

		SceneManager.LoadScene(vrSceneName, LoadSceneMode.Additive);
	}

}
