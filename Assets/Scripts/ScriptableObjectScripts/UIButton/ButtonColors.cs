using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @Author : Veli-Matti Vuoti
/// 
/// Scriptable Object for button theme
/// </summary>
[CreateAssetMenu(menuName = "ButtonColorTheme")]
public class ButtonColors : ScriptableObject
{
	public Sprite buttonSprite;
	public Color normalColor;
	public Color highlightedColor;
	public Color pressedColor;
	public Color selectedColor;
	public Color buttonTextColor;

}
