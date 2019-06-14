using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// @Author Veli-Matti Vuoti
/// 
/// This Class Handles the Hover Tablets FilterTab UI
/// </summary>
public class FilterTab : MonoBehaviour
{

	[SerializeField]Button[] buttons;
	[SerializeField] Transform[] contentPages;
	[SerializeField] TextMeshProUGUI[] texts;
	[SerializeField] Button[] otherSettingButtons;

	public delegate void ChangeHeightDelegate();
	public static event ChangeHeightDelegate ChangeHeightEvent;

	private void Start()
	{
		buttons = transform.GetChild(0).GetComponentsInChildren<Button>();
		contentPages = new Transform[transform.childCount - 1];
		buttons[0].onClick.AddListener(() => OpenColorFilters());
		buttons[1].onClick.AddListener(() => OpenSightFilters());
		buttons[2].onClick.AddListener(() => OpenOtherFilters());

		for (int i = 1; i < transform.childCount; i++)
		{
			contentPages[i - 1] = transform.GetChild(i);
		}

		otherSettingButtons[0].onClick.AddListener(() => OnButtonClickWheelChairMode());
	}

	/// <summary>
	/// Opens Color filter tab
	/// </summary>
	void OpenColorFilters()
	{
		contentPages[0].gameObject.SetActive(true);
		contentPages[1].gameObject.SetActive(false);
		contentPages[2].gameObject.SetActive(false);
	}

	/// <summary>
	/// Opens Sight filter tab
	/// </summary>
	void OpenSightFilters()
	{

		contentPages[1].gameObject.SetActive(true);
		contentPages[0].gameObject.SetActive(false);
		contentPages[2].gameObject.SetActive(false);
	}

	/// <summary>
	/// Opens Other filters tab
	/// </summary>
	void OpenOtherFilters()
	{
		contentPages[2].gameObject.SetActive(true);
		contentPages[0].gameObject.SetActive(false);
		contentPages[1].gameObject.SetActive(false);
	}

	public void OnButtonClickWheelChairMode()
	{
		Debug.Log("CLICKED");
		if (GlobalValues.settings.wheelChairMode)
		{
			GlobalValues.settings.wheelChairMode = false;
		}
		else
		{
			GlobalValues.settings.wheelChairMode = true;
		}
		texts[0].text = GlobalValues.settings.wheelChairMode.ToString();
		Settings.Save();


		ChangeHeightEvent?.Invoke();

	}
}
