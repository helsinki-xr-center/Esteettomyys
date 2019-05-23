using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class LobbyMainPanel : MonoBehaviourPunCallbacks
{
	public const int maxRoomPlayers = 16;

	[Header("Mode selection panel")]
	public GameObject modeSelectionPanel;

	[Header("Training selection panel")]
	public GameObject trainingSelectionPanel;

	[Header("Create room panel")]
	public GameObject createRoomPanel;
	public TMP_InputField roomNameInputField;

	[Header("Room list panel")]
	public GameObject roomListPanel;
	public Transform roomListPanelContentParent;
	public GameObject roomListObjectPrefab;

	[Header("Inside room panel")]
	public GameObject roomPanel;
	public Button startGameButton;
	public GameObject playerListObjectPrefab;

	private Dictionary<int, GameObject> playerListObjects;
	private Dictionary<string, RoomInfo> cachedRoomList;
	private List<GameObject> roomListObjects;



	public void Awake()
	{
		cachedRoomList = new Dictionary<string, RoomInfo>();
		roomListObjects = new List<GameObject>();
	}

	#region PUN

	public override void OnConnectedToMaster()
	{
		SetActivePanel(trainingSelectionPanel);
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		ClearRoomListView();

		UpdateCachedRoomList(roomList);
		UpdateRoomListView();
	}

	public override void OnLeftLobby()
	{
		cachedRoomList.Clear();

		ClearRoomListView();
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		SetActivePanel(trainingSelectionPanel);
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		SetActivePanel(trainingSelectionPanel);
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		string roomName = "Room " + Random.Range(1000, 10000);

		RoomOptions options = new RoomOptions { MaxPlayers = maxRoomPlayers };

		PhotonNetwork.CreateRoom(roomName, options, null);
	}

	public override void OnJoinedRoom()
	{
		SetActivePanel(roomPanel);

		if (playerListObjects == null)
		{
			playerListObjects = new Dictionary<int, GameObject>();
		}

		foreach (Player p in PhotonNetwork.PlayerList)
		{
			GameObject entry = Instantiate(playerListObjectPrefab);
			entry.transform.SetParent(roomPanel.transform);
			entry.transform.localScale = Vector3.one;
			entry.GetComponent<PlayerListEntry>().SetValues(p.ActorNumber, p.NickName);

			object isPlayerReady;
			if (p.CustomProperties.TryGetValue(ConstStringKeys.PUN_PLAYER_READY, out isPlayerReady))
			{
				entry.GetComponent<PlayerListEntry>().SetPlayerReady((bool)isPlayerReady);
			}

			playerListObjects.Add(p.ActorNumber, entry);
		}

		startGameButton.gameObject.SetActive(CheckPlayersReady());

		Hashtable props = new Hashtable
			{
				{ConstStringKeys.PUN_PLAYER_READY, false}
			};
		PhotonNetwork.LocalPlayer.SetCustomProperties(props);
	}

	public override void OnLeftRoom()
	{
		SetActivePanel(trainingSelectionPanel);

		foreach (GameObject entry in playerListObjects.Values)
		{
			Destroy(entry.gameObject);
		}

		playerListObjects.Clear();
		playerListObjects = null;
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		GameObject entry = Instantiate(playerListObjectPrefab);
		entry.transform.SetParent(roomPanel.transform);
		entry.transform.localScale = Vector3.one;
		entry.GetComponent<PlayerListEntry>().SetValues(newPlayer.ActorNumber, newPlayer.NickName);

		playerListObjects.Add(newPlayer.ActorNumber, entry);

		startGameButton.gameObject.SetActive(CheckPlayersReady());
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		Destroy(playerListObjects[otherPlayer.ActorNumber].gameObject);
		playerListObjects.Remove(otherPlayer.ActorNumber);

		startGameButton.gameObject.SetActive(CheckPlayersReady());
	}

	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
		{
			startGameButton.gameObject.SetActive(CheckPlayersReady());
		}
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
	{
		if (playerListObjects == null)
		{
			playerListObjects = new Dictionary<int, GameObject>();
		}

		GameObject entry;
		if (playerListObjects.TryGetValue(targetPlayer.ActorNumber, out entry))
		{
			object isPlayerReady;
			if (changedProps.TryGetValue(ConstStringKeys.PUN_PLAYER_READY, out isPlayerReady))
			{
				entry.GetComponent<PlayerListEntry>().SetPlayerReady((bool)isPlayerReady);
			}
		}

		startGameButton.gameObject.SetActive(CheckPlayersReady());
	}

	#endregion


	#region UI
	public void OnBackButtonClicked()
	{
		if (PhotonNetwork.InLobby)
		{
			PhotonNetwork.LeaveLobby();
		}

		SetActivePanel(modeSelectionPanel);
	}

	public void OnCreateRoomPanelButtonClicked()
	{
		SetActivePanel(createRoomPanel);
	}

	public void OnCreateRoomButtonClicked()
	{
		string roomName = roomNameInputField.text;
		roomName = (string.IsNullOrEmpty(roomName)) ? "Room " + Random.Range(1, 100000) : roomName;

		RoomOptions options = new RoomOptions { MaxPlayers = maxRoomPlayers };

		PhotonNetwork.CreateRoom(roomName, options, null);
	}

	public void OnLeaveGameButtonClicked()
	{
		PhotonNetwork.LeaveRoom();
	}

	public void OnTutorialButtonClicked()
	{
		new UIInfoMessage("Not implemented yet.", UIInfoMessage.MessageType.Error).Deliver();
	}

	public void OnTrainingButtonClicked()
	{
		if (PhotonNetwork.NetworkingClient.LoadBalancingPeer.PeerState != PeerStateValue.Disconnected)
		{
			SetActivePanel(trainingSelectionPanel);
			return;
		}
		string playerName = GlobalValues.user;

		if (!playerName.Equals(""))
		{
			PhotonNetwork.LocalPlayer.NickName = playerName;
			PhotonNetwork.ConnectUsingSettings();
		}
		else
		{
			Debug.LogError("Player Name is invalid.");
		}
	}

	public void OnExamButtonClicked()
	{
		new UIInfoMessage("Not implemented yet.", UIInfoMessage.MessageType.Error).Deliver();
	}

	public void OnRoomListButtonClicked()
	{
		if (!PhotonNetwork.InLobby)
		{
			PhotonNetwork.JoinLobby();
		}

		SetActivePanel(roomListPanel);
	}

	public void OnStartGameButtonClicked()
	{
		PhotonNetwork.CurrentRoom.IsOpen = false;
		PhotonNetwork.CurrentRoom.IsVisible = false;

		// PhotonNetwork.LoadLevel("DemoAsteroids-GameScene");
		// TODO: start game here
		// Probably load the players into a separate VR-multiplayer scene, after which each one loads the correct room additively
	}


	#endregion


	private bool CheckPlayersReady()
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			return false;
		}

		foreach (Player p in PhotonNetwork.PlayerList)
		{
			if (p.CustomProperties.TryGetValue(ConstStringKeys.PUN_PLAYER_READY, out object isPlayerReady))
			{
				if (!(bool)isPlayerReady)
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		return true;
	}

	private void ClearRoomListView()
	{
		roomListObjects.ForEach(x => Destroy(x.gameObject));
		roomListObjects.Clear();
	}

	public void LocalPlayerPropertiesUpdated()
	{
		startGameButton.gameObject.SetActive(CheckPlayersReady());
	}


	private void SetActivePanel(GameObject panel)
	{
		modeSelectionPanel.SetActive(false);
		trainingSelectionPanel.SetActive(false);
		createRoomPanel.SetActive(false);
		roomListPanel.SetActive(false);
		roomPanel.SetActive(false);

		panel.SetActive(true);
	}

	private void UpdateCachedRoomList(List<RoomInfo> roomList)
	{
		foreach (RoomInfo info in roomList)
		{
			// Remove room from cached room list if it got closed, became invisible or was marked as removed
			if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
			{
				if (cachedRoomList.ContainsKey(info.Name))
				{
					cachedRoomList.Remove(info.Name);
				}

				continue;
			}

			// Update cached room info
			if (cachedRoomList.ContainsKey(info.Name))
			{
				cachedRoomList[info.Name] = info;
			}
			// Add new room info to cache
			else
			{
				cachedRoomList.Add(info.Name, info);
			}
		}
	}

	private void UpdateRoomListView()
	{
		foreach (RoomInfo info in cachedRoomList.Values)
		{
			GameObject entry = Instantiate(roomListObjectPrefab, roomListPanelContentParent);
			entry.transform.localScale = Vector3.one;
			entry.GetComponent<RoomListEntry>().SetValues(info.Name, info.PlayerCount, info.MaxPlayers);

			roomListObjects.Add(entry);
		}
	}
}
