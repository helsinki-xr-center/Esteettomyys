using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapLocationMarker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public Mapper mapper;
	public bool hovered = false;
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

	// Update is called once per frame
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
    }

	public void SetValues(Transform follow)
	{
		followTransform = follow;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		hovered = true;
		image.color = highlightColor;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		hovered = false;
		image.color = normalColor;
	}
}
