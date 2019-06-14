using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontOfHMDState : ITabletState
{

	private readonly TabletStatePattern tablet;

	float sec;
	

	public FrontOfHMDState(TabletStatePattern tabletStatePattern)
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
		tablet.ChangeState(TabletStateID.Follow);
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
		
	}

	public void UpdateState()
	{
		if (Time.time > sec)
		{
			sec = Time.time + 3;
			Debug.Log("IM IN FRONT OF HMD STATE");
			//Debug.Log(tablet.transform.position);
			//Debug.Log(tablet.positions[0].position);
			//Debug.Log(tablet.speed);
		}

		tablet.StartLerp(tablet.positions[0].position);
		tablet.WatchTarget(tablet.vrCamera.position);
		tablet.ChangeTabletDistance(tablet.positions[0], tablet.positions[0].forward);
		tablet.OnGrabGribActivate();
		if (tablet.grabPinch.GetStateDown(Valve.VR.SteamVR_Input_Sources.Any))
		{
			ToFollowState();
		}
	}
}
