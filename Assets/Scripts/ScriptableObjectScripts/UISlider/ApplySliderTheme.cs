using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// 
/// Applies slider theme for slider element
/// </summary>
[RequireComponent(typeof(Slider))]
public class ApplySliderTheme : MonoBehaviour
{
	Slider slider;
	Image bg;
	Image fill;
	public SliderColorThemes sliderTheme;

	private void OnValidate()
	{
		if (sliderTheme != null)
		{

			slider = GetComponent<Slider>();
			bg = transform.GetChild(0).GetComponent<Image>();
			fill = transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
			var colors = slider.colors;
			colors.highlightedColor = sliderTheme.highlightedColor;
			colors.normalColor = sliderTheme.normalColor;
			colors.pressedColor = sliderTheme.pressedColor;
			colors.selectedColor = sliderTheme.selectedColor;
			fill.color = sliderTheme.fillColor;
			bg.color = sliderTheme.backgroundColor;
			bg.sprite = sliderTheme.backGround;
			fill.sprite = sliderTheme.fillSprite;
		}
	}
}
