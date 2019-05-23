using UnityEngine;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// </summary>
public class InteractableObject : MonoBehaviour
{

	public InteractableObjectType objectType;

	[SerializeField] [Tooltip("Pointer on object color indicator")] Color hoveringColor;
	[SerializeField] [Tooltip("Selected object color indicator")] Color selectedColor;
	[SerializeField] [Tooltip("Critical object color indicator")] Color criticalObjectColor;
	[SerializeField] [Tooltip("Standard object color indicator")] Color standardObjectColor;

	public bool hardMode;

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

	public Color GetHoverColor()
	{
		return hoveringColor;
	}

	public Color GetSelectedColor()
	{
		return selectedColor;
	}
}
