using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITabletState
{

	void UpdateState();

	void StartState();

	void ExitState();

	void ToPreviousState();

	void ToHoldState();

	void ToFollowState();

	void ToFrontOfControllerState();

	void ToFrontOfHMDState();

	void ToFollowSideState();


}
