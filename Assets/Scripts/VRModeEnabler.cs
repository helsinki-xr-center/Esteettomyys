using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Enables or disables the VR mode on Start.
 * </summary>
 */
public class VRModeEnabler : MonoBehaviour
{

	public bool vrEnabled = true;

	void Start()
	{
		if (!UnityEngine.XR.XRDevice.isPresent && vrEnabled)
		{
			Debug.LogError("No VR device present while trying to enable VR.", this);
		}
		else
		{
			UnityEngine.XR.XRSettings.enabled = vrEnabled;
		}
	}

}
