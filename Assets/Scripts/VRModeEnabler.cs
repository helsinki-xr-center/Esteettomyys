using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Nomi Lakkala
 * 
 * Enables or disables the VR mode on Start.
 */
public class VRModeEnabler : MonoBehaviour
{

	public bool vrEnabled = true;

	void Start()
	{
		UnityEngine.XR.XRSettings.enabled = vrEnabled;
	}

}
