using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * If the VR scene is not already loaded, loads the scene on top of current scenes.
 * </summary>
 */
public class LoadVRScene : MonoBehaviour
{
	[FormerlySerializedAs("vrSceneName")]
	[Scene]
	public string vrScene = "VRPlayer";

    void Awake()
    {
		Scene scene = SceneManager.GetSceneByName(vrScene);
		if(scene == null){
			Debug.LogError("No VR scene found by the name: " + vrScene);
			return;
		}

		if(scene.isLoaded){
			Debug.Log("VR scene already loaded");
			return;
		}

		SceneManager.LoadScene(vrScene, LoadSceneMode.Additive);
	}

}
