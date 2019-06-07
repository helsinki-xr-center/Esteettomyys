using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapPlayerListItem : MonoBehaviour
{
	public TextMeshProUGUI playerName;
	public Button teleportButton;

	private AvatarFollowPlayer player;

    void FixedUpdate()
    {
		if(player == null)
		{
			Destroy(gameObject);
		}
    }
	
	public void SetValues(AvatarFollowPlayer player)
	{
		if (player.IsLocal())
		{
			teleportButton.interactable = false;
		}
		this.player = player;
		playerName.SetText(player.photonView.Owner.NickName);
	}

	public void OnTeleportButtonPressed()
	{
		new TeleportMessage(PlayerTeleportLocationFinder.FindTeleportLocation(player.transform.position)).Deliver();
	}
}
