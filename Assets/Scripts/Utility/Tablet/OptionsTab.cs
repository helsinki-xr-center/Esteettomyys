using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This Class handles hovertablet Options UI
/// </summary>
public class OptionsTab : MonoBehaviour
{
	[SerializeField] Transform[] tabs;
	[SerializeField] Button[] buttons;
	[SerializeField] Slider[] sliders;
	[SerializeField] TextMeshProUGUI[] texts;

	public delegate void WheelChairModeDelegate(bool status);
	public static event WheelChairModeDelegate WheelChairModeEvent;

	private void Start()
	{
		buttons[0].onClick.AddListener(() => OpenVolumeSettings());
		buttons[1].onClick.AddListener(() => OpenVRSettings());
		buttons[2].onClick.AddListener(() => OpenSettings());
		buttons[3].onClick.AddListener(() => LeftHandMode());
		buttons[4].onClick.AddListener(() => SomethingElse());
		buttons[5].onClick.AddListener(() => WheelChairMode());
	
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
	/// Makes player go wheelchairmode
	/// </summary>
	void WheelChairMode()
	{
		
		if( WheelChairModeEvent != null)
		{
			WheelChairModeEvent(GlobalValues.settings.wheelChairMode);
			texts[0].text = GlobalValues.settings.wheelChairMode.ToString();
		}
	}
}
