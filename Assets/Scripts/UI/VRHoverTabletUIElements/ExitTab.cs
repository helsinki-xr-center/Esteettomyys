using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// @Author : Veli-Matti Vuoti
/// 
/// This Class handles the hovertablet exit tab buttons
/// </summary>
public class ExitTab : MonoBehaviour
{
	Button[] buttons;
	public delegate void ExitDelegate();
	public static event ExitDelegate ExitToMenuEvent;
	public static event ExitDelegate ExitAppEvent;

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
		ExitToMenuEvent?.Invoke();
	}

	void ExitApplication()
	{
		ExitAppEvent?.Invoke();
	}
}
