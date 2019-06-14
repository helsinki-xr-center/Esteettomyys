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
		if (tablet.debugMode)
		{
			tablet.DebugStateStatus();
		}
		tablet.StartLerp(tablet.positions[1].position);
		tablet.WatchTarget(tablet.vrCamera.position);
		if (tablet.touchPadPress.GetStateDown(tablet.CheckHandMode()))
		{
			ToPreviousState();
		}
	}
}
