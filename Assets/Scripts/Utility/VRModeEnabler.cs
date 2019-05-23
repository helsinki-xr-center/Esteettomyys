using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

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

	void Awake()
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
