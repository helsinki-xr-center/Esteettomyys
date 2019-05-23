using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Attached to roomListEntry prefabs. Handles setting values for the UI elements.
 * </summary>
 */
public class RoomListEntry : MonoBehaviour
{
	public TextMeshProUGUI roomNameText;
	public TextMeshProUGUI roomPlayerAmountText;

	private string roomName;

	/**
	 * <summary>
	 * Sets the values for different UI elements.
	 * </summary>
	 */
	public void SetValues(string name, int currentPlayers, int maxPlayers)
	{
		roomName = name;
		roomNameText.SetText(roomName);
		roomPlayerAmountText.SetText(currentPlayers + " / " + maxPlayers);
	}

	/**
	 * <summary>
	 * Gets called from unity UI Button. Will join the room in question.
	 * </summary>
	 */
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