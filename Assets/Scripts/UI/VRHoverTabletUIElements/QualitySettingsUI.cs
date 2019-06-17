using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QualitySettingsUI : MonoBehaviour
{
	public List<Transform> settingElements = new List<Transform>();
	public Button[] buttons;

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
