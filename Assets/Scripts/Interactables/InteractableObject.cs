using UnityEngine;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// 
/// Interactable Object class
/// </summary>
public class InteractableObject : MonoBehaviour
{

	public InteractableObjectType objectType;

	[SerializeField] [Tooltip("Pointer on object color indicator")] Color hoveringColor;
	[SerializeField] [Tooltip("Selected object color indicator")] Color selectedColor;
	[SerializeField] [Tooltip("Critical object color indicator")] Color criticalObjectColor;
	[SerializeField] [Tooltip("Standard object color indicator")] Color standardObjectColor;

	public bool hardMode;

	/// <summary>
	/// Chooses the object colors for interacting
	/// </summary>
	private void Start()
	{
		if (hardMode)
		{
			switch (objectType)
			{
				case InteractableObjectType.Critical:
					selectedColor = criticalObjectColor;
					break;
				case InteractableObjectType.Standard:
					selectedColor = standardObjectColor;
					break;
				default:
					break;
			}
		}
		else
		{
			switch (objectType)
			{
				case InteractableObjectType.Critical:
					hoveringColor = criticalObjectColor;
					selectedColor = criticalObjectColor;
					break;
				case InteractableObjectType.Standard:
					hoveringColor = standardObjectColor;
					selectedColor = standardObjectColor;
					break;		
				default:
					break;
			}
		}
	}

	/// <summary>
	/// Get hover color, indicates if the object is being hovered over by pointer
	/// </summary>
	/// <returns>Hovering color</returns>
	public Color GetHoverColor()
	{
		return hoveringColor;
	}

	/// <summary>
	/// Get selected color, indicates if the object is selected
	/// </summary>
	/// <returns>Selecting color</returns>
	public Color GetSelectedColor()
	{
		return selectedColor;
	}
}
