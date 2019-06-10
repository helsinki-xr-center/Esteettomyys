using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// @author : Veli-Matti Vuoti
/// 
/// Might Need Later
/// </summary>
public class SettingSlot : MonoBehaviour
{

	public Button button;
	public TextMeshProUGUI[] textMeshes;

	private void Start()
	{
		button = GetComponentInChildren<Button>();
		textMeshes = GetComponentsInChildren<TextMeshProUGUI>();

	}

}
