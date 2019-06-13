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
		
	}

	public void ToHoldState()
	{

	}

	public void UpdateState()
	{
		//tablet.StartLerp(tablet.positions[2].position);
		
		//tablet.transform.position = new Vector3(tablet.playerT.position.x - 2, tablet.vrCamera.position.y, tablet.playerT.position.z);
		tablet.StartLerp(new Vector3(tablet.positions[2].position.x , tablet.positions[2].position.y, tablet.positions[2].position.z));
		tablet.ChangeTabletDistance(tablet.positions[2], tablet.positions[2].forward);
		tablet.WatchTarget(tablet.vrCamera.position);
	}
}
