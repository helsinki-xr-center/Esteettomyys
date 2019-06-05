using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ApplySliderTheme : MonoBehaviour
{
	Slider slider;
	Image bg;
	Image fill;
	public SliderColorThemes sliderTheme;

	private void OnValidate()
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
