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
		tablet.ChangeState(TabletStateID.Hold);
	}

	public void ToPreviousState()
	{
		
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
		tablet.ChangeTabletDistance(tablet.positions[4], tablet.positions[4].forward, tablet.rightController);
		tablet.OnGrabGribActivate();

		if (tablet.touchPadPress.GetStateDown(tablet.CheckHandMode()))
		{
			Debug.Log("grabPINCH");
			ToHoldState();
		}

	}
}
