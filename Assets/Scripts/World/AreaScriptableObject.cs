using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
