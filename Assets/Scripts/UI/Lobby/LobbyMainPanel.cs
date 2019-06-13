using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using Hashtable = ExitGames.Client.Photon.Hashtable;



/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * A monolith class that handles all the UI and PUN callbacks necessary for a working lobby screen.
 * </summary>
 */
public class LobbyMainPanel : MonoBehaviourPunCallbacks
{
	public const int maxRoomPlayers = 16;

	public string multiplayerRootScene = "Multiplayer";
	public string tutorialRootScene = "Tutorial";
	public string examRootScene = "Exam";

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

	[Header("Area selectors")]
	public GameObject trainingAreaSelector;

	private Dictionary<int, GameObject> playerListObjects;
	private Dictionary<string, RoomInfo> cachedRoomList;
	private List<GameObject> roomListObjects;



	public void Awake()
	{
		cachedRoomList = new Dictionary<string, RoomInfo>();
		roomListObjects = new List<GameObject>();
	}

	#region PUN

	/**
	 * <summary>
	 * PUN callback override.
	 * </summary>
	 */
	public override void OnConnectedToMaster()
	{
		SetActivePanel(trainingSelectionPanel);
	}

	/**
	 * <summary>
	 * PUN callback override.
	 * </summary>
	 */
	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		ClearRoomListView();

		UpdateCachedRoomList(roomList);
		UpdateRoomListView();
	}

	/**
	 * <summary>
	 * PUN callback override.
	 * </summary>
	 */
	public override void OnLeftLobby()
	{
		cachedRoomList.Clear();

		ClearRoomListView();
	}

	/**
	 * <summary>
	 * PUN callback override. 
	 * </summary>
	 */
	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		SetActivePanel(trainingSelectionPanel);
	}

	/**
	 * <summary>
	 * PUN callback override.
	 * </summary>
	 */
	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		SetActivePanel(trainingSelectionPanel);
	}

	/**
	 * <summary>
	 * PUN callback override. Will create a random room.
	 * </summary>
	 */
	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		string roomName = "Room " + Random.Range(1000, 10000);

		RoomOptions options = new RoomOptions { MaxPlayers = maxRoomPlayers };

		PhotonNetwork.CreateRoom(roomName, options, null);
	}

	/**
	 * <summary>
	 * PUN callback override. Creates playerlist entries for every player in the room.
	 * </summary>
	 */
	public override void OnJoinedRoom()
	{
		Debug.Log("Joined room!");

		SetActivePanel(roomPanel);
		trainingAreaSelector.SetActive(true);

		if (playerListObjects == null)
		{
			playerListObjects = new Dictionary<int, GameObject>();
		}

		foreach (Player p in PhotonNetwork.PlayerList)
		{
			GameObject entry = CreatePlayerEntry(p);

			if (p.CustomProperties.TryGetValue(ConstStringKeys.PUN_PLAYER_READY, out object isPlayerReady))
			{
				entry.GetComponent<PlayerListEntry>().SetPlayerReady((bool)isPlayerReady);
			}

		}

		startGameButton.gameObject.SetActive(CheckPlayersReady());

		Hashtable props = new Hashtable
			{
				{ConstStringKeys.PUN_PLAYER_READY, false}
			};
		PhotonNetwork.LocalPlayer.SetCustomProperties(props);
	}

	/**
	 * <summary>
	 * PUN callback override.
	 * </summary>
	 */
	public override void OnLeftRoom()
	{
		SetActivePanel(trainingSelectionPanel);
		trainingAreaSelector.SetActive(false);

		if (playerListObjects != null)
		{
			foreach (GameObject entry in playerListObjects.Values)
			{
				Destroy(entry.gameObject);
			}

			playerListObjects.Clear();
			playerListObjects = null;
		}
	}

	/**
	 * <summary>
	 * PUN callback override. Creates a new playerListEntry for the new player.
	 * </summary>
	 */
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		GameObject entry = CreatePlayerEntry(newPlayer);

		startGameButton.gameObject.SetActive(CheckPlayersReady());
	}

	/**
	 * <summary>
	 * PUN callback override. Destroys the playerListEntry associated with the player who left.
	 * </summary>
	 */
	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		Destroy(playerListObjects[otherPlayer.ActorNumber].gameObject);
		playerListObjects.Remove(otherPlayer.ActorNumber);

		startGameButton.gameObject.SetActive(CheckPlayersReady());
	}

	/**
	 * <summary>
	 * PUN callback override. 
	 * </summary>
	 */
	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
		{
			startGameButton.gameObject.SetActive(CheckPlayersReady());
		}
	}

	/**
	 * <summary>
	 * PUN callback override. Updates ready state for the player whose properties were changed.
	 * </summary>
	 */
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

	/**
	 * <summary>
	 * PUN callback override. Handles loading the game scene for clients when <see cref="ConstStringKeys.PUN_MATCH_START"/> is set to true.
	 * </summary>
	 */
	public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
	{
		if (propertiesThatChanged.TryGetValue(ConstStringKeys.PUN_MATCH_START, out object val))
		{
			if ((bool)val && !PhotonNetwork.IsMasterClient)
			{
				//PhotonNetwork.IsMessageQueueRunning = false;
				SceneLoaderAsync.instance.LoadSceneAndUnloadCurrent("Multiplayer");
			}
		}
	}

	#endregion


	#region UI
	/**
	 * <summary>
	 * Gets called from Unity UI Button. Leaves current lobby and goes back to mode selection panel.
	 * </summary>
	 */
	public void OnBackButtonClicked()
	{
		if (PhotonNetwork.InLobby)
		{
			PhotonNetwork.LeaveLobby();
		}

		SetActivePanel(modeSelectionPanel);
	}

	/**
	 * <summary>
	 * Gets called from Unity UI Button. Goes back to the MainMenu and shuts down VR.
	 * </summary>
	 */
	public void OnLogOutButtonClicked()
	{
		if (PhotonNetwork.InLobby)
		{
			PhotonNetwork.LeaveLobby();
		}

		LoginManager.Logout();
		XRSettings.enabled = false;
		SceneManager.LoadScene("MainMenu");
	}

	/**
	 * <summary>
	 * Gets called from Unity UI Button. Sets createRoomPanel as the active panel.
	 * </summary>
	 */
	public void OnCreateRoomPanelButtonClicked()
	{
		SetActivePanel(createRoomPanel);
	}

	/**
	* <summary>
	* Gets called from Unity UI Button. Creates a new room with name from the roomNameInputField, or chooses a random name.
	* </summary>
	*/
	public void OnCreateRoomButtonClicked()
	{
		string roomName = roomNameInputField.text;
		roomName = (string.IsNullOrEmpty(roomName)) ? "Room " + Random.Range(1, 100000) : roomName;

		RoomOptions options = new RoomOptions { MaxPlayers = maxRoomPlayers };

		PhotonNetwork.CreateRoom(roomName, options, null);
	}

	/**
	 * <summary>
	 * Gets called from Unity UI Button. Leaves the current room.
	 * </summary>
	 */
	public void OnLeaveGameButtonClicked()
	{
		PhotonNetwork.LeaveRoom();
	}


	/**
	 * <summary>
	 * Gets called from Unity UI Button. Should change to the tutorial scene. (Not implemented yet)
	 * </summary>
	 */
	public void OnTutorialButtonClicked()
	{
		if(PhotonNetwork.IsConnected)
		{
			PhotonNetwork.Disconnect();
		}
		SceneLoaderAsync.instance.LoadSceneAndUnloadCurrent(tutorialRootScene);
	}


	/**
	 * <summary>
	 * Gets called from Unity UI Button. Connects to the Photon network with the current player name from <see cref="GlobalValues"/> and sets trainingSelectionPanel as active.
	 * </summary>
	 */
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


	/**
	 * <summary>
	 * Gets called from Unity UI Button. Should start the exam. (Not implemented yet)
	 * </summary>
	 */
	public void OnExamButtonClicked()
	{
		if (PhotonNetwork.IsConnected)
		{
			PhotonNetwork.Disconnect();
		}
		SceneLoaderAsync.instance.LoadSceneAndUnloadCurrent(examRootScene);
	}

	/**
	 * <summary>
	 * Gets called from Unity UI Button. Join Photon lobby and sets roomListPanel as active.
	 * </summary>
	 */
	public void OnRoomListButtonClicked()
	{
		if (!PhotonNetwork.InLobby)
		{
			PhotonNetwork.JoinLobby();
		}

		SetActivePanel(roomListPanel);
	}

	/**
	 * <summary>
	 * Gets called from Unity UI Button. Starts the game and changes the scene.
	 * </summary>
	 */
	public void OnStartGameButtonClicked()
	{
		PhotonNetwork.CurrentRoom.IsOpen = false;
		PhotonNetwork.CurrentRoom.IsVisible = false;

		Hashtable props = new Hashtable
			{
				{ConstStringKeys.PUN_MATCH_START, true}
			};

		PhotonNetwork.CurrentRoom.SetCustomProperties(props);
		PhotonNetwork.SendAllOutgoingCommands();
		//PhotonNetwork.IsMessageQueueRunning = false;

		SceneLoaderAsync.instance.LoadSceneAndUnloadCurrent("Multiplayer");
	}


	#endregion


	/**
	 * <summary>
	 * Checks whether all players in the current room are ready. Returns true if everyone is ready.
	 * </summary>
	 */
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


	/**
	 * <summary>
	 * Destroys all roomListEntries in the roomList.
	 * </summary>
	 */
	private void ClearRoomListView()
	{
		roomListObjects.ForEach(x => Destroy(x.gameObject));
		roomListObjects.Clear();
	}

	/**
	 * <summary>
	 * Gets called when the properties of the local player are changed.
	 * </summary>
	 */
	public void LocalPlayerPropertiesUpdated()
	{
		startGameButton.gameObject.SetActive(CheckPlayersReady());
	}

	/**
	 * <summary>
	 * Disables all panels and enables the desired one.
	 * </summary>
	 */
	private void SetActivePanel(GameObject panel)
	{
		trainingAreaSelector.SetActive(false);
		modeSelectionPanel.SetActive(false);
		trainingSelectionPanel.SetActive(false);
		createRoomPanel.SetActive(false);
		roomListPanel.SetActive(false);
		roomPanel.SetActive(false);

		panel.SetActive(true);
	}

	/**
	 * <summary>
	 * Updates the state of the cached roomlist.
	 * </summary>
	 */
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

	/**
	 * <summary>
	 * Instantiates roomlist entries to the roomList and sets their values.
	 * </summary>
	 */
	private void UpdateRoomListView()
	{
		foreach (RoomInfo info in cachedRoomList.Values)
		{
			GameObject entry = Instantiate(roomListObjectPrefab, roomListPanelContentParent);
			entry.transform.localScale = Vector3.one;
			entry.transform.localPosition = Vector3.zero;
			entry.GetComponent<RoomListEntry>().SetValues(info.Name, info.PlayerCount, info.MaxPlayers);

			roomListObjects.Add(entry);
		}
	}

	/**
	 * <summary>
	 * Creates a PlayerListEntry and adds it to the playerListObjects list.
	 * </summary>
	 */
	private GameObject CreatePlayerEntry(Player p)
	{
		GameObject entry = Instantiate(playerListObjectPrefab);
		entry.transform.SetParent(roomPanel.transform);
		entry.transform.localPosition = Vector3.zero;
		entry.transform.localScale = Vector3.one;
		entry.GetComponent<PlayerListEntry>().SetValues(p.ActorNumber, p.NickName);

		playerListObjects.Add(p.ActorNumber, entry);
		return entry;
	}
}
