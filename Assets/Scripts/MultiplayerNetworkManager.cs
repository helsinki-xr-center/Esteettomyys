using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerNetworkManager : MonoBehaviour
{

	public string avatarPrefabName;

    IEnumerator Start()
    {
		MoveLocalPlayerToSpawnLocation();
		yield return new WaitForSeconds(1);
		PhotonNetwork.Instantiate(avatarPrefabName, Vector3.zero, Quaternion.identity, 0);
	}


	private void MoveLocalPlayerToSpawnLocation(){
		var locations = FindObjectsOfType<SpawnLocation>();
		if (locations.Length == 0) return;
		SpawnLocation location = locations[PhotonNetwork.LocalPlayer.GetPlayerNumber() % locations.Length];

		FindObjectOfType<PlayerPosition>().transform.position = location.transform.position;
	}
}
