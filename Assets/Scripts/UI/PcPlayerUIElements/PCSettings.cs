using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PCSettings : MonoBehaviour
{

	[SerializeField] Transform[] settingSlots;
	[SerializeField] Button[] buttons;
	//Mikäli haluaa instansioida settingsit

	void Start()
    {
		settingSlots = new Transform[transform.childCount];

		for (int i = 0; i < transform.childCount; i++)
		{
			settingSlots[i] = transform.GetChild(i);

			if (!settingSlots[i].gameObject.activeSelf)
			{
				settingSlots[i].gameObject.SetActive(true);
			}		
		}

		buttons = new Button[transform.childCount];

		for (int i = 0; i < transform.childCount; i++)
		{
			buttons[i] = transform.GetChild(i).GetComponentInChildren<Button>();
		}

		buttons[0].onClick.AddListener(() => WheelChair());
		buttons[0].transform.parent.GetChild(0).transform.gameObject.GetComponent<TextMeshProUGUI>().text = "Wheelchairmode";

		//More Settings
	}

	void WheelChair()
	{
		
		if (!GlobalValues.settings.wheelChairMode)
		{
			GlobalValues.settings.wheelChairMode = true;
			
		}
		else
		{
			GlobalValues.settings.wheelChairMode = false;
		}
				
		buttons[0].transform.parent.GetChild(2).transform.gameObject.GetComponent<TextMeshProUGUI>().text = GlobalValues.settings.wheelChairMode.ToString();
		
	}

}
