﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// @Author : Veli-Matti Vuoti
/// 
/// Handles the PC Menu Settings Tab
/// </summary>
public class PCSettings : MonoBehaviour
{

	[SerializeField] Transform[] settingSlots;
	[SerializeField] Button[] buttons;
	//Mikäli haluaa instansioida settingsit

	public void MoveRight()
	{
		if (GlobalValues.settings.qualityLevelIndex <= 5)
		{
			GlobalValues.settings.qualityLevelIndex++;
			QualitySettings.SetQualityLevel(GlobalValues.settings.qualityLevelIndex);
			buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = QualitySettings.names[QualitySettings.GetQualityLevel()];
			Settings.Save();
		}
	}

	public void MoveLeft()
	{
		if (GlobalValues.settings.qualityLevelIndex >= 0)
		{
			GlobalValues.settings.qualityLevelIndex--;
			QualitySettings.SetQualityLevel(GlobalValues.settings.qualityLevelIndex);
			buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = QualitySettings.names[QualitySettings.GetQualityLevel()];
			Settings.Save();
		}
	}
}