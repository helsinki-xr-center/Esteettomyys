using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * A scriptable object for different areas.
 * </summary>
 */
[CreateAssetMenu(fileName = "New Area", menuName = "Area")]
public class AreaScriptableObject : ScriptableObject
{
	[Scene]
	public string areaScene;
	[Scene]
	public string detailsScene;
	public string displayName;
	public Sprite previewSprite;

	//TODO: Mission List
}
