using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// 
/// This Class Handles the PC Option Tabs
/// </summary>
public class PCOptionTabs : MonoBehaviour
{

	[SerializeField] Button[] buttons;
	[SerializeField] Transform[] contentPages;

    void Start()
    {
		buttons = new Button[transform.childCount];
		contentPages = new Transform[transform.parent.transform.childCount - 1];

		for (int i = 0; i < transform.childCount; i++)
		{
			buttons[i] = transform.GetChild(i).GetComponent<Button>();
		}

		for (int i = 1; i < transform.parent.transform.childCount; i++)
		{
			contentPages[i - 1] = transform.parent.transform.GetChild(i);
		}


		buttons[0].onClick.AddListener(() => OpenVolumeContent());
		buttons[1].onClick.AddListener(() => OpenSettingsContent());
    }

	void OpenVolumeContent()
	{
		OpenTab(0, true);
	}

	void OpenSettingsContent()
	{
		OpenTab(1, true);
	}


	void OpenTab(int index, bool open)
	{
		if (open)
		{
			for (int i = 0; i < contentPages.Length; i++)
			{
				if (i == index && !contentPages[i].transform.gameObject.activeSelf)
				{
					contentPages[i].transform.gameObject.SetActive(true);

					continue;
				}

				contentPages[i].transform.gameObject.SetActive(false);

			}
		}
		else
		{
			for (int i = 0; i < contentPages.Length; i++)
			{

				contentPages[i].transform.gameObject.SetActive(false);

			}
		}
	}
}
