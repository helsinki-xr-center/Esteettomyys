using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ApplyButtonTheme : MonoBehaviour
{

	public ButtonColors colorTheme;
	Button button;
	Image image;

	private void OnValidate()
	{
		image = GetComponent<Image>();
		button = GetComponent<Button>();
		var colors = button.colors;
		image.sprite = colorTheme.buttonSprite;
		colors.normalColor = colorTheme.normalColor;
		colors.highlightedColor = colorTheme.highlightedColor;
		colors.pressedColor = colorTheme.pressedColor;
		colors.selectedColor = colorTheme.selectedColor;
		button.colors = colors;

	}

}
