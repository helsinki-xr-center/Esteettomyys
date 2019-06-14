using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Handles exit from game scenes to lobby or whole program quit. Subscribes to static events from <see cref="ExitTab"/>. This script should be placed in all the root game scenes.
 * </summary>
 */
public class ExitHandler : MonoBehaviour
{
	[Header("Lobby scenes")]
	[Scene]
	public string pcLobby = "Lobby_PC";
	[Scene]
	public string vrLobby = "Lobby_VR";

	private void OnEnable()
	{
		ExitTab.ExitToMenuEvent += ExitToLobby;
		ExitTab.ExitAppEvent += ExitGame;
	}

	private void OnDisable()
	{
		ExitTab.ExitToMenuEvent -= ExitToLobby;
		ExitTab.ExitAppEvent -= ExitGame;
	}

	/**
	 * <summary>
	 * Unloads the current active scene and loads the lobby. Also leaves PhotonNetwork room if connected.
	 * </summary>
	 */
	private void ExitToLobby()
	{
		if (PhotonNetwork.InRoom)
		{
			PhotonNetwork.LeaveRoom();

		}
		string lobbyName = GlobalValues.controllerMode == ControllerMode.PC ? pcLobby : vrLobby;
		SceneLoaderAsync.instance.LoadSceneAndUnloadCurrent(lobbyName);
	}

	/**
	 * <summary>
	 * Quits the application.
	 * </summary>
	 */
	private void ExitGame()
	{
		if (Application.isEditor)
		{
#if UNITY_EDITOR
			EditorApplication.ExitPlaymode();
#endif
		}
		else
		{
			Application.Quit();
		}
	}
}
