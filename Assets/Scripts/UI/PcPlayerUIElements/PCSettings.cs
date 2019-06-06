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

		//More Settings
	}

	

}
