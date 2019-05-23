using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JoinRandomRoomWithKey : MonoBehaviour
{

	public KeyCode startKey = KeyCode.H;
	public KeyCode joinKey = KeyCode.J;
	public KeyCode readyKey = KeyCode.K;
	public KeyCode startGameKey = KeyCode.L;

	void Update()
    {
        if(Input.GetKeyDown(startKey)){
			FindObjectOfType<LobbyMainPanel>().OnTrainingButtonClicked();
		}else if(Input.GetKeyDown(joinKey)){
			PhotonNetwork.JoinRandomRoom();
			Debug.Log("Joining Random...");
		}
		else if (Input.GetKeyDown(readyKey))
		{
			FindObjectsOfType<PlayerListEntry>().Single(x => x.isLocalPlayer).OnPlayerReadyButtonClick();
		}
		else if (Input.GetKeyDown(startGameKey))
		{
			FindObjectOfType<LobbyMainPanel>().OnStartGameButtonClicked();
		}
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(30, 0, 100, 50), $"{startKey} for training.");
		GUI.Label(new Rect(30, 50, 100, 50), $"{joinKey} to join random.");
		GUI.Label(new Rect(30, 100, 100, 50), $"{readyKey} to ready.");
		GUI.Label(new Rect(30, 150, 100, 50), $"{startGameKey} to start game.");
	}
}
