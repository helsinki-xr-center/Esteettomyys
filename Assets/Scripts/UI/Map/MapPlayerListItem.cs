using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapPlayerListItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	public TextMeshProUGUI playerName;
	public Button teleportButton;
	public GameObject locationMarkerPrefab;
	public GameObject otherPlayerLocationMarkerPrefab;
	public Graphic colorTarget;

	public Color highlightColor = Color.yellow;
	public Color normalColor = Color.clear;

	private AvatarFollowPlayer player;
	private MapLocationMarker marker;

    void FixedUpdate()
    {
		if(player == null)
		{
			Destroy(gameObject);
		}

		if(marker.hovered || marker.selected)
		{
			colorTarget.color = highlightColor;
		}
		else
		{
			colorTarget.color = normalColor;
		}

		if(marker.selected && !teleportButton.gameObject.activeSelf)
		{
			teleportButton.gameObject.SetActive(true);
		}
		else if (!marker.selected && teleportButton.gameObject.activeSelf)
		{
			teleportButton.gameObject.SetActive(false);
		}
    }
	
	public void SetValues(AvatarFollowPlayer player, Transform mapImageTransfrom)
	{
		GameObject markerPrefab = otherPlayerLocationMarkerPrefab;
		if (player.IsLocal())
		{
			teleportButton.interactable = false;
			markerPrefab = locationMarkerPrefab;
		}
		this.player = player;
		playerName.SetText(player.photonView.Owner.NickName);

		GameObject go = Instantiate(markerPrefab, mapImageTransfrom);
		marker = go.GetComponent<MapLocationMarker>();
		marker.SetValues(player.transform);
	}

	public void OnTeleportButtonPressed()
	{
		new TeleportMessage(PlayerTeleportLocationFinder.FindTeleportLocation(player.transform.position)).Deliver();
	}
	private void OnDestroy()
	{
		if(marker != null)
		{
			Destroy(marker.gameObject);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		marker.hovered = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		marker.hovered = false;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		foreach(var mark in FindObjectsOfType<MapLocationMarker>())
		{
			mark.selected = false;
		}

		marker.selected = true;
	}
}
