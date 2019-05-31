using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitTab : MonoBehaviour
{
	Button[] buttons;
	public delegate void ExitDelegate();
	public static event ExitDelegate ExitEvent;

	private void Start()
	{
		buttons = new Button[transform.childCount - 1];

		for (int i = 0; i < transform.childCount; i++)
		{
			if (i != 0)
			{
				buttons[i-1] = transform.GetChild(i).transform.gameObject.GetComponent<Button>();
			}
		}

		buttons[0].onClick.AddListener(() => ExitToMenu());
		buttons[1].onClick.AddListener(() => ExitApplication());
		
	}

	void ExitToMenu()
	{
		if(ExitEvent != null)
		{
			ExitEvent();
		}

	}

	void ExitApplication()
	{
		if(ExitEvent != null)
		{
			ExitEvent();
		}
	}
}
