
using UnityEngine;
using UnityEngine.UI;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Pun;
using TMPro;


/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Attached to the playerListEntry prefabs. Handles pressing the Ready button and setting the right values for UI elements.
 * </summary>
 */
public class PlayerListEntry : MonoBehaviour
{
	public TextMeshProUGUI playerNameText;
	public Button playerReadyButton;
	public TextMeshProUGUI playerReadyButtonText;
	public Image playerReadyImage;

	private int ownerId;
	private bool isPlayerReady;

	/**
	 * <summary>
	 * Returns a bool whether this <see cref="PlayerListEntry"/> is the local player or not.
	 * </summary>
	 */
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


	/**
	 * <summary>
	 * Gets called from Unity UI Button. Sets the player state as ready.
	 * </summary>
	 */
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


	/**
	 * <summary>
	 * Sets <see cref="Player"/> properties to indicate whether this player is ready.
	 * </summary>
	 */
	private void SetReadyProp()
	{
		Hashtable props = new Hashtable() { { ConstStringKeys.PUN_PLAYER_READY, isPlayerReady } };
		PhotonNetwork.LocalPlayer.SetCustomProperties(props);
	}


	/**
	 * <summary>
	 * Sets UI values.
	 * </summary>
	 */
	public void SetValues(int playerId, string playerName)
	{
		ownerId = playerId;
		playerNameText.text = playerName;
	}

	/**
	 * <summary>
	 * Sets state in the UI to indicate whether this player is ready or not.
	 * </summary>
	 */
	public void SetPlayerReady(bool playerReady)
	{
		playerReadyButtonText.SetText(playerReady ? "Ready!" : "Ready?");
		playerReadyImage.enabled = playerReady;
	}
}