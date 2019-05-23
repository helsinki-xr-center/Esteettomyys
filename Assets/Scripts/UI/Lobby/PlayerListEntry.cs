
using UnityEngine;
using UnityEngine.UI;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Pun;
using TMPro;

public class PlayerListEntry : MonoBehaviour
{
	public TextMeshProUGUI playerNameText;
	public Button playerReadyButton;
	public TextMeshProUGUI playerReadyButtonText;
	public Image playerReadyImage;

	private int ownerId;
	private bool isPlayerReady;

	public bool isLocalPlayer
	{
		get
		{
			return PhotonNetwork.LocalPlayer.ActorNumber == ownerId;
		}
	}

	public void Start()
	{
		if (PhotonNetwork.LocalPlayer.ActorNumber != ownerId)
		{
			playerReadyButton.gameObject.SetActive(false);
		}
		else
		{
			SetReadyProp();
		}
	}

	public void OnPlayerReadyButtonClick()
	{
		isPlayerReady = !isPlayerReady;
		SetPlayerReady(isPlayerReady);

		SetReadyProp();

		if (PhotonNetwork.IsMasterClient)
		{
			FindObjectOfType<LobbyMainPanel>().LocalPlayerPropertiesUpdated();
		}
	}

	private void SetReadyProp()
	{
		Hashtable props = new Hashtable() { { ConstStringKeys.PUN_PLAYER_READY, isPlayerReady } };
		PhotonNetwork.LocalPlayer.SetCustomProperties(props);
	}

	public void SetValues(int playerId, string playerName)
	{
		ownerId = playerId;
		playerNameText.text = playerName;
	}

	public void SetPlayerReady(bool playerReady)
	{
		playerReadyButtonText.SetText(playerReady ? "Ready!" : "Ready?");
		playerReadyImage.enabled = playerReady;
	}
}