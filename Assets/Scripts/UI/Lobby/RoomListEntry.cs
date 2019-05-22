using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListEntry : MonoBehaviour
{
	public TextMeshProUGUI roomNameText;
	public TextMeshProUGUI roomPlayerAmountText;

	private string roomName;

	public void SetValues(string name, int currentPlayers, int maxPlayers)
	{
		roomName = name;
		roomNameText.SetText(roomName);
		roomPlayerAmountText.SetText(currentPlayers + " / " + maxPlayers);
	}

	public void OnJoinRoomButtonClicked()
	{
		{
			if (PhotonNetwork.InLobby)
			{
				PhotonNetwork.LeaveLobby();
			}

			PhotonNetwork.JoinRoom(roomName);
		}
	}
}