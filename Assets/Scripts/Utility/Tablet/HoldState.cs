﻿using System.Collections;
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
		tablet.ChangeState(tablet.previousState);
	}

	public void UpdateState()
	{
		if (tablet.debugMode)
		{
			tablet.DebugStateStatus();
		}
		tablet.OnGrabGribActivate();

		if (tablet.touchPadPress.GetStateDown(tablet.CheckHandMode()))
		{
			Debug.Log("grabPINCH");
			ToPreviousState();
		}
	}

}