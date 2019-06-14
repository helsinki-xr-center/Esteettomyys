using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontOfControllerState : ITabletState
{
	private readonly TabletStatePattern tablet;

	public FrontOfControllerState(TabletStatePattern tabletStatePattern)
	{
		tablet = tabletStatePattern;
	}

	public void ExitState()
	{
		if (tablet.debugMode)
		{
			Debug.Log("EXIT STATE " + this.ToString());
		}
	}

	public void StartState()
	{
		if (tablet.debugMode)
		{
			Debug.Log("START STATE " + this.ToString());
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
		tablet.ChangeState(TabletStateID.Hold);
	}

	public void ToPreviousState()
	{
		
	}

	public void UpdateState()
	{
		if (tablet.debugMode)
		{
			tablet.DebugStateStatus();
		}

		tablet.WatchTarget(tablet.vrCamera.position);
		tablet.OnGrabGribActivate();

		if (tablet.CheckHandMode() == Valve.VR.SteamVR_Input_Sources.RightHand)
		{
			tablet.StartLerp(tablet.positions[4].position);
			tablet.ChangeTabletDistance(tablet.positions[4], tablet.positions[4].forward, tablet.rightController);
		}
		else
		{
			tablet.StartLerp(tablet.positions[3].position);
			tablet.ChangeTabletDistance(tablet.positions[3], tablet.positions[3].forward, tablet.leftController);
		}
		

		if (tablet.touchPadPress.GetStateDown(tablet.CheckHandMode()))
		{
			Debug.Log("grabPINCH");
			ToHoldState();
		}
	}
}
