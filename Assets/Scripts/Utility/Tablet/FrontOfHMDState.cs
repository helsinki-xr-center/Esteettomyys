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
		if (tablet.debugMode)
		{
			tablet.DebugStateStatus();
		}

		tablet.StartLerp(tablet.positions[0].position);
		tablet.WatchTarget(tablet.vrCamera.position);
		tablet.ChangeTabletDistance(tablet.positions[0], tablet.positions[0].forward, tablet.vrCamera);
		tablet.OnGrabGribActivate();

		if (tablet.touchPadPress.GetStateDown(tablet.CheckHandMode())) 
		{
			ToFollowState();
		}
	}
}
