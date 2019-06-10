using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * A script for the map UI playerlist items. Spawns a MapLocationMarker for itself, and handles hovering and clicking events.
 * </summary>
 */
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

	/**
	 * <summary>
	 * Sets the necessary values for this script and spawns a marker. Should be called immediately after creating this object.
	 * <param name="mapImageTransfrom">
	 * The parameter mapImageTransform should be the transform of the RawImage element that contains the <see cref="MapperImageStream"/> script.
	 * </param>
	 * </summary>
	 */
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

	/**
	 * <summary>
	 * UI callback for the teleport Button. Sends a new <see cref="TeleportMessage"/> with a position to teleport to near the tracked player.
	 * </summary>
	 */
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

	/**
	 * <summary>
	 * Finds all MapLocationMarkers and deselects them. Also selects own marker.
	 * </summary>
	 */
	public void OnPointerClick(PointerEventData eventData)
	{
		foreach(var mark in FindObjectsOfType<MapLocationMarker>())
		{
			mark.selected = false;
		}

		marker.selected = true;
	}
}
