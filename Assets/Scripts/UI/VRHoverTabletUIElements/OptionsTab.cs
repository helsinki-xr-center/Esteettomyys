using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// @Author : Veli-Matti Vuoti
/// 
/// This Class handles hovertablet Options UI
/// </summary>
public class OptionsTab : MonoBehaviour
{
	[SerializeField] Transform[] tabs;
	[SerializeField] Button[] buttons;
	[SerializeField] Slider[] sliders;
	[SerializeField] TextMeshProUGUI[] texts;

	[SerializeField] private float defaultHeight = 1.8f;
	[SerializeField] private float wheelChairHeight = 0.8f;
	[SerializeField] private float wheelChairWidth = 1f;

	private void Start()
	{
		buttons[0].onClick.AddListener(() => OpenVolumeSettings());
		buttons[1].onClick.AddListener(() => OpenVRSettings());
		buttons[2].onClick.AddListener(() => OpenSettings());
		buttons[3].onClick.AddListener(() => LeftHandMode());
		buttons[4].onClick.AddListener(() => SomethingElse());
	
	}

	/// <summary>
	/// Opens Volume settings tab
	/// </summary>
	void OpenVolumeSettings()
	{
		tabs[0].gameObject.SetActive(true);
		tabs[1].gameObject.SetActive(false);
		tabs[2].gameObject.SetActive(false);
	}

	/// <summary>
	/// Opens VR settings tab
	/// </summary>
	void OpenVRSettings()
	{
		tabs[0].gameObject.SetActive(false);
		tabs[1].gameObject.SetActive(true);
		tabs[2].gameObject.SetActive(false);
	}

	/// <summary>
	/// Opens Global settings tab
	/// </summary>
	void OpenSettings()
	{
		tabs[0].gameObject.SetActive(false);
		tabs[1].gameObject.SetActive(false);
		tabs[2].gameObject.SetActive(true);
	}

	/// <summary>
	/// Makes Player change to lefthanded on / off
	/// </summary>
	void LeftHandMode()
	{
		if (!GlobalValues.settings.leftHandMode)
		{
			GlobalValues.settings.leftHandMode = true;
		}
		else
		{
			GlobalValues.settings.leftHandMode = false;
		}
	}

	/// <summary>
	/// Does something else
	/// </summary>
	void SomethingElse()
	{
		// DO something
		Debug.Log("DO SOMETHING");
	}

	/// <summary>
	/// if default height changes
	/// </summary>
	/// <param name="height"> quessed player height </param>
	public void SetDefaultHeight(float height)
	{
		defaultHeight = height;
	}

	/// <summary>
	/// if wheelChair height changes
	/// </summary>
	/// <param name="height"> wheelchair height </param>
	public void SetDefaultWheelChairHeight(float height)
	{
		wheelChairHeight = height;
	}
}
