using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldState : ITabletState
{

	private readonly TabletStatePattern tablet;
	float sec = 0;

	public HoldState(TabletStatePattern tabletStatePattern)
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
		if( Time.time > sec)
		{
			sec = Time.time + 3;
			Debug.Log("IM IN HOLD STATE");			
			//Debug.Log("tablet Position " + tablet.transform.position);
		}
		tablet.OnGrabGribActivate();

		if (tablet.touchPadPress.GetStateDown(Valve.VR.SteamVR_Input_Sources.Any))
		{
			Debug.Log("grabPINCH");
			ToPreviousState();
		}
	}

}
