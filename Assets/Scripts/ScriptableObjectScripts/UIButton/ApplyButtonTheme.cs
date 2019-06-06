using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ApplyButtonTheme : MonoBehaviour
{
	TextMeshProUGUI textMesh;
	public ButtonColors colorTheme;
	Button button;
	Image image;

	private void OnValidate()
	{
		if (colorTheme != null)
		{
			image = GetComponent<Image>();
			button = GetComponent<Button>();
			textMesh = GetComponentInChildren<TextMeshProUGUI>();
			if(textMesh != null)
			{
				textMesh.color = colorTheme.buttonTextColor;
			}
			var colors = button.colors;
			image.sprite = colorTheme.buttonSprite;
			colors.normalColor = colorTheme.normalColor;
			colors.highlightedColor = colorTheme.highlightedColor;
			colors.pressedColor = colorTheme.pressedColor;
			colors.selectedColor = colorTheme.selectedColor;
			button.colors = colors;
			
		}
	}

}
