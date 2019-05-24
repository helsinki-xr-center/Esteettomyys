using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Disables the button attached to this GameObject if the user is in offline mode.
 * </summary>
 */
[RequireComponent(typeof(Button))]
public class DisableButtonIfOffline : MonoBehaviour
{
	public Button button;

	public void Start()
	{
		if (button == null)
		{
			button = GetComponent<Button>();
		}
		if (GlobalValues.offlineMode)
		{
			button.interactable = false;
		}
	}
}
