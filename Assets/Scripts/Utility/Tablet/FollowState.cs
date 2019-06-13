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

	public void StartedState(ITabletState state)
	{
		if ( state == this)
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
	}

	public void UpdateState()
	{

		tablet.StartLerp(tablet.positions[1].position);
		tablet.WatchTarget(tablet.vrCamera.position);	

	}
}
