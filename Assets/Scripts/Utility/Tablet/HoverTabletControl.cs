using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

/// <summary>
/// This Class controls the hovertablet movement
/// </summary>
public class HoverTabletControl : MonoBehaviour
{

	PlayerPosition playerPosition;
	Vector3 backPosition;
	Vector3 frontPosition;

	public float followDistance;
	public float frontDistance;
	public float speed;

	public bool following;

	public SteamVR_Action_Boolean tabletToFront;

	private void Start()
	{
		playerPosition = FindObjectOfType<PlayerPosition>();

	}

	private void Update()
	{
		Debug.DrawRay(transform.position, GetBackPosition(), Color.red, 0.1f);
		Debug.DrawRay(transform.position, GetFrontPosition(), Color.blue, 0.1f);

		BringTabletToFront();
		TabletMovement();

	}

	/// <summary>
	/// Makes Hovertable follow and lookat players position
	/// </summary>
	private void TabletMovement()
	{

		if (following)
		{
			transform.position = Vector3.Lerp(transform.position, GetBackPosition(), Time.deltaTime * speed);
		}
		else
		{
			transform.position = Vector3.Lerp(transform.position, GetFrontPosition(), Time.deltaTime * speed);
		}

		float height;

		if (GlobalValues.controllerMode == ControllerMode.VR)
		{
			height = playerPosition.GetHeadHeightFromBase();
		}
		else
		{
			height = 1.8f;
		}

		transform.LookAt(new Vector3(playerPosition.transform.position.x, playerPosition.transform.position.y + height, playerPosition.transform.position.z));
	}

	/// <summary>
	/// Brings tablet front or back of player from GribButton
	/// </summary>
	private void BringTabletToFront()
	{
		if (GlobalValues.controllerMode == ControllerMode.VR)
		{
			if (tabletToFront.GetLastStateDown(SteamVR_Input_Sources.Any))
			{
				if (following)
				{
					following = false;
				}
				else
				{
					following = true;
				}
			}
		}
		else
		{
			if (Input.GetButtonDown("Jump"))
			{
				if (following)
				{
					following = false;
				}
				else
				{
					following = true;
				}
			}
		}
	}

	/// <summary>
	/// Sets new follow distance
	/// </summary>
	/// <param name="value"></param>
	public void SetFrontDistance( float value)
	{
		frontDistance = value;
	}

	/// <summary>
	/// Sets new follow distance
	/// </summary>
	/// <param name="value"></param>
	public void SetFollowDistance(float value)
	{
		followDistance = value;
	}

	/// <summary>
	/// Calculates follow position for hovertablet
	/// </summary>
	/// <returns>Vector3 follow position</returns>
	public Vector3 GetBackPosition()
	{
		Vector3 direction = playerPosition.GetRotation() * -Vector3.forward;
		float headHeight;
		if (GlobalValues.controllerMode == ControllerMode.VR)
		{
			headHeight = playerPosition.GetHeadHeightFromBase();
		}
		else
		{
			headHeight = 1.8f;
		}
		
		backPosition = playerPosition.GetPosition() + new Vector3(direction.x * followDistance, headHeight , direction.z * followDistance);
		return backPosition;
	}

	/// <summary>
	/// Calculates front position for hovertablet
	/// </summary>
	/// <returns>Vector3 front position</returns>
	public Vector3 GetFrontPosition()
	{
		Vector3 direction = playerPosition.GetRotation() * Vector3.forward;
		float headHeight;
		if (GlobalValues.controllerMode == ControllerMode.VR)
		{
			headHeight = playerPosition.GetHeadHeightFromBase();
		}
		else
		{
			headHeight = 1.8f;
		}
		frontPosition = playerPosition.GetPosition() + new Vector3(direction.x * frontDistance, headHeight , direction.z * frontDistance);
		return frontPosition;
	}
}
