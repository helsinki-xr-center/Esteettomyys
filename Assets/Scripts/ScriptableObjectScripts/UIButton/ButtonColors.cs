using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ButtonColorTheme")]
public class ButtonColors : ScriptableObject
{
	public Sprite buttonSprite;
	public Color normalColor;
	public Color highlightedColor;
	public Color pressedColor;
	public Color selectedColor;

}
