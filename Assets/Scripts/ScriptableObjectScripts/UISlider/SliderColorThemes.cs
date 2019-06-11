using UnityEngine;

/// <summary>
/// @Author : Veli-Matti Vuoti
/// 
/// Scriptable Object for slider themes
/// </summary>
[CreateAssetMenu(menuName = "SliderColorTheme")]
public class SliderColorThemes : ScriptableObject
{
	public Sprite backGround;
	public Sprite fillSprite;

	public Color fillColor;
	public Color backgroundColor;

	public Color normalColor;
	public Color highlightedColor;
	public Color pressedColor;
	public Color selectedColor;
	
}
