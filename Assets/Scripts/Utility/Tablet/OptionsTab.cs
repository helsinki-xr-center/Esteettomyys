using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsTab : MonoBehaviour
{

	[SerializeField] Button[] buttons;
	[SerializeField] Slider[] sliders;

	private void Start()
	{
		buttons = transform.GetChild(1).transform.GetChild(1).transform.GetChild(1).GetComponentsInChildren<Button>();
		buttons = transform.GetChild(1).transform.GetChild(1).transform.GetChild(2).GetComponentsInChildren<Button>();
		sliders = transform.GetChild(1).transform.GetChild(1).transform.GetChild(0).GetComponentsInChildren<Slider>();

	}

}
