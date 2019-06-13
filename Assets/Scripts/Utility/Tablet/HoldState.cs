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
		tablet.tabletState = TabletStateID.FrontHMD;
	}

	public void ToHoldState()
	{
	}

	public void UpdateState()
	{
		if( Time.time > sec)
		{
			sec = Time.time + 3;
			Debug.Log("IM IN HOLD STATE");			
			//Debug.Log("tablet Position " + tablet.transform.position);
		}
	}
}
