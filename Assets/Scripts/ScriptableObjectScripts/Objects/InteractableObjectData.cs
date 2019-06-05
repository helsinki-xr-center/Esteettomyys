using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "InteractableObjectData")]
public class InteractableObjectData : ScriptableObject
{

	public Color hoveringColor;
	public Color criticalColor;
	public Color standardColor;
	public Color objColor;

	public string objectName;
	public string objectDescription;

}
