using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Handles PhotonNetwork interactions in Multiplayer.
 * </summary>
 */
public class MultiplayerNetworkManager : MonoBehaviour
{

	public string avatarPrefabName;
	public string vrLobby;
	public string pcLobby;

    IEnumerator Start()
    {
		yield return new WaitForSeconds(0.1f);
		MoveLocalPlayerToSpawnLocation();
		yield return new WaitForSeconds(1);
		PhotonNetwork.Instantiate(avatarPrefabName, Vector3.zero, Quaternion.identity, 0);
	}

	/**
	 * <summary>
	 * Chooses a random spawn location from the scene and moves the player to one of them.
	 * </summary>
	 */
	private void MoveLocalPlayerToSpawnLocation(){
		var locations = FindObjectsOfType<SpawnLocation>();
		if (locations.Length == 0) return;
		SpawnLocation location = locations[PhotonNetwork.LocalPlayer.GetPlayerNumber() % locations.Length];

		FindObjectOfType<PlayerPosition>().transform.position = location.transform.position;
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape)){
			PhotonNetwork.LeaveRoom();
			string lobbyName = GlobalValues.gameMode == GlobalValues.GameMode.PC ? pcLobby : vrLobby;
			SceneLoaderAsync.instance.LoadSceneAndUnloadCurrent(lobbyName);
		}
	}
}
