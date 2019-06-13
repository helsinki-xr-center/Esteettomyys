using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * A script for the UI that tracks a single transform in 3D space and sets its own position to match the position in ui space relative to the rendered map image. Also handles hovering and clicking to highlight and select itself.
 * </summary>
 */
public class MapLocationMarker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	public Mapper mapper;
	public bool hovered = false;
	public bool selected = false;
	public Transform followTransform;

	public Color normalColor = Color.white;
	public Color highlightColor = Color.yellow;

	private RectTransform rect;
	private Image image;

	private void Start()
	{
		rect = GetComponent<RectTransform>();
		mapper = FindObjectOfType<Mapper>();
		image = GetComponent<Image>();
		image.color = normalColor;
	}

	void FixedUpdate()
    {
        if(mapper == null || followTransform == null)
		{
			Destroy(gameObject);
			return;
		}

		Vector2 pos = mapper.XZWorldToMapPositionXY(followTransform.position);

		rect.anchorMin = pos;
		rect.anchorMax = pos;

		if (hovered || selected)
		{
			image.color = highlightColor;
		}
		else
		{
			image.color = normalColor;
		}
	}

	/**
	 * <summary>
	 * Sets the necessary values for this script. Should be called immediately after creating this object.
	 * </summary>
	 */
	public void SetValues(Transform follow)
	{
		followTransform = follow;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		hovered = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		hovered = false;
	}

	/**
	 * <summary>
	 * Finds all other MapLocationMarkers and deselects them. Also selects itself.
	 * </summary>
	 */
	public void OnPointerClick(PointerEventData eventData)
	{
		foreach (var mark in FindObjectsOfType<MapLocationMarker>())
		{
			mark.selected = false;
		}

		selected = true;
	}
}
