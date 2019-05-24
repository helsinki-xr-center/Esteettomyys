using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Attached to a button in the UI. Exits the program.
 * </summary>
 */
public class ExitProgramButton : MonoBehaviour
{
	/**
	* <summary>
	* Called from the UI button.
	* </summary>
	*/
	public void OnExitPressed()
	{
		if (Application.isEditor)
		{
#if UNITY_EDITOR
			EditorApplication.ExitPlaymode();
#endif
		}
		else
		{
			Application.Quit();
		}
	}
}
