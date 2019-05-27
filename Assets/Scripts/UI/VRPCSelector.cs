using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Handles UI elements in the VR PC mode selection panel.
 * </summary>
 */
public class VRPCSelector : AwaitableUIPanel
{
	public Button vrButton;
	public Button pcButton;

	public bool selected;

	private void Awake(){
		selected = false;
	}


	/**
	 * <summary>
	 * Called from Unity UI button.
	 * </summary>
	 */
	public void OnVRButtonPressed(){
		// TODO: disable this if VR device is not present.
		// Causes things to break if the VR is not working.
		XRSettings.enabled = true;
		XRSettings.LoadDeviceByName("OpenVR");

		vrButton.interactable = false;
		pcButton.interactable = false;


		Invoke("VrButtonImpl", 1);
	}

	private void VrButtonImpl(){
		GlobalValues.controllerMode = ControllerMode.VR;
		selected = true;
	}

	/**
	 * <summary>
	 * Called from Unity UI button.
	 * </summary>
	 */
	public void OnPCButtonPressed()
	{
		GlobalValues.controllerMode = ControllerMode.PC;
		selected = true;
	}

	/**
	 * <summary>
	 * Called from Unity UI button. Aborts selection and logs out.
	 * </summary>
	 */
	public void OnLogoutButtonPressed(){
		GlobalValues.loggedIn = false;
		GlobalValues.offlineMode = false;
		selected = true;
	}

	public override IEnumerator WaitForFinish()
	{
		yield return new WaitUntil(() => selected);
	}
}
