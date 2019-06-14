using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OtherFiltersTabUI : MonoBehaviour
{

	public void OnButtonClickWheelChairMode()
	{
		GlobalValues.settings.wheelChairMode = !GlobalValues.settings.wheelChairMode ? true : false;
		Settings.Save();
	}

}
