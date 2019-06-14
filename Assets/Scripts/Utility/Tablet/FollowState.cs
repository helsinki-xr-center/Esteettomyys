using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowState : ITabletState
{

	private readonly TabletStatePattern tablet;
	bool status;

	public FollowState(TabletStatePattern tabletStatePattern)
	{
		tablet = tabletStatePattern;
	}

	public void ExitState()
	{
		Debug.Log("EXIT STATE " + this.ToString());
	}

	public void StartState()
	{
		Debug.Log("START STATE " + this.ToString());
		tablet.StartCoroutine(tablet.TabletActivationStateChange());
	}

	public void ToFollowSideState()
	{

	}

	public void ToFollowState()
	{

	}

	public void ToFrontOfControllerState()
	{

	}

	public void ToFrontOfHMDState()
	{

	}

	public void ToHoldState()
	{

	}

	public void ToPreviousState()
	{
		tablet.ChangeState(tablet.previousState);
	}

	public void UpdateState()
	{
		tablet.StartLerp(tablet.positions[1].position);
		tablet.WatchTarget(tablet.vrCamera.position);
		if (tablet.grabPinch.GetStateDown(Valve.VR.SteamVR_Input_Sources.Any))
		{
			ToPreviousState();
		}
	}
}
