using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowSideState : ITabletState
{
	private readonly TabletStatePattern tablet;

	public FollowSideState(TabletStatePattern tabletStatePattern)
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
		tablet.StartLerp(new Vector3(tablet.positions[2].position.x , tablet.positions[2].position.y, tablet.positions[2].position.z));
		tablet.ChangeTabletDistance(tablet.positions[2], tablet.positions[2].forward, tablet.vrCamera);
		tablet.WatchTarget(tablet.vrCamera.position);
		tablet.OnGrabGribActivate();
	}
}
