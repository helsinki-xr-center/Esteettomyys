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
	public void VRButtonPressed(){
		if(!UnityEngine.XR.XRDevice.isPresent){
			new UIInfoMessage("No VR device found! Check that SteamVR is running.", UIInfoMessage.MessageType.Error).Deliver();
		}else{
			GlobalValues.gameMode = GlobalValues.GameMode.VR;
			selected = true;
		}
	}

	/**
	 * <summary>
	 * Called from Unity UI button.
	 * </summary>
	 */
	public void PCButtonPressed()
	{
		GlobalValues.gameMode = GlobalValues.GameMode.PC;
		selected = true;
	}

	public override IEnumerator WaitForFinish()
	{
		yield return new WaitUntil(() => selected);
	}
}
