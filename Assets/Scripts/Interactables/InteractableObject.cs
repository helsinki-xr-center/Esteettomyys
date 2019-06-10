using UnityEngine;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// 
/// Interactable Object class
/// </summary>
public class InteractableObject : MonoBehaviour
{
	//RuntimeColors
	Color hoveringColor;
	Color selectedColor;
	Color objColor;

	public ObjectType objectType;
	public InteractableObjectData objData;
	string objname;
	string objdesc;
	
	public bool selected;

	private void OnEnable()
	{
		PcPlayer.mouseHoverIn += IsHovered;
		PcPlayer.mouseHoverOut += HoveredEnd;
		PcPlayer.mouseClick += IsClicked;
		Pointer.PointerHit += IsHovered;
		Pointer.PointerLeft += HoveredEnd;
		Pointer.PointerClick += IsClicked;
		PcPlayer.OnDeselectObjectEvent += OnDeselect;
	}

	private void OnDisable()
	{
		PcPlayer.mouseHoverIn -= IsHovered;
		PcPlayer.mouseHoverOut -= HoveredEnd;
		PcPlayer.mouseClick -= IsClicked;
		Pointer.PointerHit -= IsHovered;
		Pointer.PointerLeft -= HoveredEnd;
		Pointer.PointerClick -= IsClicked;
		PcPlayer.OnDeselectObjectEvent -= OnDeselect;
	}

	public void Start()
	{
		ObjIndicatorColor();
	}

	/// <summary>
	/// Sets the colors for object
	/// </summary>
	private void ObjIndicatorColor()
	{
	
		if (objData != null)
		{
			objname = objData.objectName;
			objdesc = objData.objectDescription;
			objColor = objData.objColor;

			if (GlobalValues.gameMode == GameMode.Tutorial ||
						GlobalValues.gameMode == GameMode.Training)
			{
				switch (objectType)
				{
					case ObjectType.Critical:
						hoveringColor = objData.criticalColor;
						selectedColor = objData.criticalColor;
						break;
					case ObjectType.Standard:
						hoveringColor = objData.standardColor;
						selectedColor = objData.standardColor;
						break;
					default:
						break;
				}
			}
			else
			{
				switch (objectType)
				{
					case ObjectType.Critical:
						hoveringColor = objData.hoveringColor;
						selectedColor = objData.criticalColor;
						break;
					case ObjectType.Standard:
						hoveringColor = objData.hoveringColor;
						selectedColor = objData.standardColor;
						break;
					default:
						break;
				}
			}
		}
	}

	/// <summary>
	/// Sets color back on deselect
	/// </summary>
	/// <param name="obj"></param>
	public void OnDeselect(GameObject obj)
	{
		if (obj == gameObject)
		{
			selected = false;
			ExtensionMethods.MaterialColorChange(gameObject, objColor);
			Debug.Log("DESELECTED" + gameObject.name);
		}
	}

	/// <summary>
	/// Sets hovered color for object
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="hitData"></param>
	public void IsHovered(object sender, RayCastData hitData)
	{
		if (hitData.target.gameObject == transform.gameObject)
		{
			if (!selected)
			{
				//Debug.Log("IM HOVERED OVER" + gameObject.name);
				ExtensionMethods.MaterialColorChange(gameObject, hoveringColor);
			}
		}
	}

	/// <summary>
	/// Resets color on hovering end
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="hitData"></param>
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

					ExtensionMethods.MaterialColorChange(gameObject, objColor);

				}
			}
			else if (sender.GetType() == typeof(Pointer))
			{
				Pointer pl = (Pointer)sender;

				if (pl.selectedObj != gameObject)
				{

					ExtensionMethods.MaterialColorChange(gameObject, objColor);

				}
			}
		}
	}

	/// <summary>
	/// Click color
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="hitData"></param>
	public void IsClicked(object sender, RayCastData hitData)
	{
		if (hitData.target == transform)
		{
			//Debug.Log("IM CLICKED" + gameObject.name);
			if (sender.GetType() == typeof(PcPlayer))
			{
				
				if (hitData.target.gameObject == gameObject)
				{
					selected = true;
					ExtensionMethods.MaterialColorChange(gameObject, selectedColor);
					Debug.Log("SELECTED" + gameObject.name);
				}
				
			}
			else if (sender.GetType() == typeof(Pointer))
			{
				
				if (hitData.target.gameObject == gameObject)
				{
					selected = true;
					ExtensionMethods.MaterialColorChange(gameObject, selectedColor);
					Debug.Log("SELECTED" + gameObject.name);
				}
				else
				{
					selected = false;
					ExtensionMethods.MaterialColorChange(gameObject, objColor);
					Debug.Log("DESELECTED" + gameObject.name);
				}

			}
		}
	}
}
