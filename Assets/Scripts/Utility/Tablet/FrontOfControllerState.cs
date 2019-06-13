using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontOfControllerState : ITabletState
{
	private readonly TabletStatePattern tablet;
	float sec;

	public FrontOfControllerState(TabletStatePattern tabletStatePattern)
	{
		tablet = tabletStatePattern;
	}

	public void StartedState(ITabletState state)
	{
		if (state == this)
		{
			Debug.Log("HEY " + state.ToString() + " Started ");
		}
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
		tablet.tabletState = TabletStateID.Hold;
	}

	public void UpdateState()
	{
		if (Time.time > sec)
		{
			sec = Time.time + 3;
			Debug.Log("IM IN FRONT OF CONTROLLER STATE");
			//Debug.Log(tablet.transform.position);
			//Debug.Log(tablet.positions[0].position);
			//Debug.Log(tablet.speed);
		}

		tablet.StartLerp(tablet.positions[4].position);
		tablet.WatchTarget(tablet.vrCamera.position);
		tablet.ChangeTabletDistance(tablet.positions[4], tablet.transform.forward);

		if (tablet.grabPinch.GetStateDown(Valve.VR.SteamVR_Input_Sources.Any))
		{
			Debug.Log("grabPINCH");
			ToHoldState();
		}

	}
}
