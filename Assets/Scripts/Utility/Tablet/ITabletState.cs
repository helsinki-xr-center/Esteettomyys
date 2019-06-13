using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITabletState
{

	void UpdateState();

	void StartedState(ITabletState state);

	void ToHoldState();

	void ToFollowState();

	void ToFrontOfControllerState();

	void ToFrontOfHMDState();

	void ToFollowSideState();

}
