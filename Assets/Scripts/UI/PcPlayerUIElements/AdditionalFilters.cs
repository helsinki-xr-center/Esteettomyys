using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// @Author : Veli-Matti Vuoti
/// 
/// Extra Filter Settings For PC Filter Content
/// </summary>
public class AdditionalFilters : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI wheelchairModeStatus;

	public void WheelChair()
	{

		if (!GlobalValues.settings.wheelChairMode)
		{
			GlobalValues.settings.wheelChairMode = true;
			wheelchairModeStatus.text = GlobalValues.settings.wheelChairMode.ToString();
		}
		else
		{
			GlobalValues.settings.wheelChairMode = false;
			wheelchairModeStatus.text = GlobalValues.settings.wheelChairMode.ToString();
		}

		Settings.Save();

	}
}
