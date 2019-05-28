using UnityEngine;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// 
/// Interactable Object class
/// </summary>
public class InteractableObject : MonoBehaviour
{

	[SerializeField] Color hoveringColor;
	[SerializeField] Color selectedColor;
	[SerializeField] Color examHoveringColor;
	[SerializeField] Color standardColor;
	[SerializeField] Color criticalColor;

	public ObjectType objectType;
	public bool manuallyPickedColors;

	public bool selected;

	private void OnEnable()
	{
		PcPlayer.mouseHoverIn += IsHovered;
		PcPlayer.mouseHoverOut += HoveredEnd;
		PcPlayer.mouseClick += IsClicked;
		Pointer.PointerHit += IsHovered;
		Pointer.PointerLeft += HoveredEnd;
		Pointer.PointerClick += IsClicked;
	}

	private void OnDisable()
	{
		PcPlayer.mouseHoverIn -= IsHovered;
		PcPlayer.mouseHoverOut -= HoveredEnd;
		PcPlayer.mouseClick -= IsClicked;
		Pointer.PointerHit -= IsHovered;
		Pointer.PointerLeft -= HoveredEnd;
		Pointer.PointerClick -= IsClicked;
	}

	public void Start()
	{
		ObjIndicatorColor();
	}

	private void ObjIndicatorColor()
	{
		if (!manuallyPickedColors)
		{
			hoveringColor = Color.yellow;
			selectedColor = Color.yellow;
			examHoveringColor = Color.yellow;
			standardColor = Color.green;
			criticalColor = Color.red;
		}

		if (GlobalValues.gameMode == GameMode.Tutorial ||
					GlobalValues.gameMode == GameMode.Training)
		{
			switch (objectType)
			{
				case ObjectType.Critical:
					hoveringColor = criticalColor;
					selectedColor = criticalColor;
					break;
				case ObjectType.Standard:
					hoveringColor = standardColor;
					selectedColor = standardColor;
					break;
				default:
					break;
			}
		}
		else if (GlobalValues.gameMode == GameMode.Exam)
		{
			switch (objectType)
			{
				case ObjectType.Critical:
					hoveringColor = examHoveringColor;
					selectedColor = criticalColor;
					break;
				case ObjectType.Standard:
					hoveringColor = examHoveringColor;
					selectedColor = standardColor;
					break;
				default:
					break;
			}
		}
	}

	public void IsHovered(object sender, RayCastData hitData)
	{
		if (hitData.target == transform)
		{
			//Debug.Log("IM HOVERED OVER" + gameObject.name);
			ExtensionMethods.MaterialColorChange(gameObject, hoveringColor);
		}
	}

	public void HoveredEnd(object sender, RayCastData hitData)
	{
		if (hitData.target == transform)
		{

			//Debug.Log("IM HOVERED EXIT" + gameObject.name);
			if (sender.GetType() == typeof(PcPlayer))
			{
				PcPlayer pl = (PcPlayer)sender;

				if (pl.selectedObject != gameObject)
				{
					selected = false;
					ExtensionMethods.MaterialColorChange(gameObject, Color.white);
				}
			}
			else if (sender.GetType() == typeof(Pointer))
			{
				Pointer pl = (Pointer)sender;

				if (pl.selectedObj != gameObject)
				{
					selected = false;
					ExtensionMethods.MaterialColorChange(gameObject, Color.white);
				}
			}
		}
	}

	public void IsClicked(object sender, RayCastData hitData)
	{
		if (hitData.target == transform)
		{
			//Debug.Log("IM CLICKED" + gameObject.name);
			if (sender.GetType() == typeof(PcPlayer))
			{
				PcPlayer pl = (PcPlayer)sender;

				if (pl.selectedObject == gameObject)
				{
					selected = true;
					ExtensionMethods.MaterialColorChange(gameObject, selectedColor);
				}
				else
				{
					selected = false;
					ExtensionMethods.MaterialColorChange(gameObject, Color.white);
				}
			}
			else if (sender.GetType() == typeof(Pointer))
			{
				Pointer pl = (Pointer)sender;

				if (pl.selectedObj == gameObject)
				{
					selected = true;
					ExtensionMethods.MaterialColorChange(gameObject, selectedColor);
				}
				else
				{
					selected = false;
					ExtensionMethods.MaterialColorChange(gameObject, Color.white);
				}

			}
		}
	}
}
