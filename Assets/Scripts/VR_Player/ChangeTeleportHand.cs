using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

/// <summary>
/// This Class Changes teleport action input
/// </summary>
public class ChangeTeleportHand : MonoBehaviour
{
	public SteamVR_Action_Boolean rightHandModeInput;
	public SteamVR_Action_Boolean leftHandModeInput;
	Teleport teleport;

	private void Start()
	{
		teleport = gameObject.GetComponent<Teleport>();
	
	}

	private void Update()
	{
		if (!GlobalValues.settings.leftHandMode)
		{
			teleport.teleportAction = rightHandModeInput;
		}
		else
		{
			teleport.teleportAction = leftHandModeInput;
		}
	}

}
