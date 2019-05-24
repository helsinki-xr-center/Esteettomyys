using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Checks if the VR player scene is loaded and if not, loads the PC player scene. Also unloads the PC player when exiting scene
 * </summary>
 */
public class LoadPCSceneIfVRNotLoaded : MonoBehaviour
{
	public string vrScene = "VRPlayer";
	public string pcScene = "PCPlayer";

	void Awake()
	{
		if (!SceneManager.GetSceneByName(vrScene).isLoaded)
		{
			Debug.Log("VRscene is not loaded. Loading PC scene", this);
			SceneManager.LoadScene(pcScene, LoadSceneMode.Additive);
		}
	}

	private void OnDestroy()
	{
		if (SceneManager.GetSceneByName(pcScene).isLoaded)
		{
			SceneManager.UnloadSceneAsync(pcScene);
		}
	}
}
