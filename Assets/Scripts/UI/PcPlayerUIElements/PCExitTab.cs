using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PCExitTab : MonoBehaviour
{

	[SerializeField] Button[] buttons;
	public delegate void ExitDelegate();
	public static event ExitDelegate ExitToMenuEvent;
	public static event ExitDelegate ExitAppEvent;

	private void Start()
	{
		buttons = new Button[transform.childCount - 1];
		for (int i = 1; i < transform.childCount; i++)
		{
			buttons[i - 1] = transform.GetChild(i).GetComponent<Button>();
		}

		buttons[0].onClick.AddListener(() => ExitToMenu());
		buttons[1].onClick.AddListener(() => ExitToDesktop());
	}

	void ExitToMenu()
	{
		ExitToMenuEvent?.Invoke();
	}

	void ExitToDesktop()
	{
		ExitAppEvent?.Invoke();
	}

}
