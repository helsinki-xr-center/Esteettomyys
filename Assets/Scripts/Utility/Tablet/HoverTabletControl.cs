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
	Transform[] tablet;
	BoxCollider tabletCol;

	public float followDistance;
	public float leftDistance;
	public float frontDistance;
	public float speed;
	public float timeToActivate;
	float height;
	public bool following;
	public bool grabbed;
	public bool changedToFollow;
	public bool LockedOnPlace;

	public FollowMode followMode;

	public SteamVR_Action_Boolean tabletToFront;
	private Vector3 leftPosition;

	private void Start()
	{
		playerPosition = FindObjectOfType<PlayerPosition>();
		tabletCol = transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<BoxCollider>();
		tablet = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; i++)
		{
			tablet[i] = transform.GetChild(i);
		}

	}

	private void Update()
	{
		//Debug.DrawRay(transform.position, GetBackPosition(), Color.red, 0.1f);
		//Debug.DrawRay(transform.position, GetFrontPosition(), Color.blue, 0.1f);
		

		BringTabletToFront();

		if (!LockedOnPlace)
		{
			TabletTrackPlayerHead();
		}

		switch (followMode)
		{
			case FollowMode.FollowBehind:
				TabletModeBehind();
				break;
			case FollowMode.FollowLeft:
				TabletModeLeft();
				break;
			case FollowMode.FollowLeftAndHide:
				TabletModeLeftHide();
				break;
			case FollowMode.Instant:
				TabletModeInstant();
				break;
			default:
				break;
		}

	}

	void TabletTrackPlayerHead()
	{

		if (GlobalValues.controllerMode == ControllerMode.VR)
		{
			height = playerPosition.GetHeadHeightFromBase();
		}
		else
		{
			if (!GlobalValues.settings.wheelChairMode)
			{
				height = 1.8f; //If Change get reference
			}
			else
			{
				height = 1f;
			}
		}

		transform.LookAt(new Vector3(playerPosition.transform.position.x, playerPosition.transform.position.y + height, playerPosition.transform.position.z));
	}

	/// <summary>
	/// Makes Hovertable follow and lookat players position
	/// </summary>
	private void TabletModeBehind()
	{

		if (following)
		{
			transform.position = Vector3.Lerp(transform.position, GetBackPosition(), Time.deltaTime * speed);

			if (tablet[0].gameObject.activeSelf && !changedToFollow)
			{
				changedToFollow = true;
				StartCoroutine(ActivateAfterTime(timeToActivate, false));
			}
		}
		else
		{
			if (tablet[0].gameObject.activeSelf && changedToFollow)
			{
				changedToFollow = false;
				StartCoroutine(ActivateAfterTime(timeToActivate, true));
			}
			else
			{
				changedToFollow = false;
				tabletCol.enabled = true;
				for (int i = 0; i < transform.childCount; i++)
				{
					tablet[i].gameObject.SetActive(true);
				}
			}

			transform.position = Vector3.Lerp(transform.position, GetFrontPosition(), Time.deltaTime * speed);
		}

	}

	void TabletModeLeft()
	{
		if (following)
		{
			transform.position = Vector3.Lerp(transform.position, GetLeftPosition(), Time.deltaTime * speed);
		}
		else
		{
			transform.position = Vector3.Lerp(transform.position, GetFrontPosition(), Time.deltaTime * speed);
		}
	}

	void TabletModeLeftHide()
	{

		if (following)
		{
			
			tabletCol.enabled = false;
			for (int i = 0; i < transform.childCount; i++)
			{
				tablet[i].gameObject.SetActive(false);
			}

		}
		else
		{
		
			tabletCol.enabled = true;
			for (int i = 0; i < transform.childCount; i++)
			{
				tablet[i].gameObject.SetActive(true);
			}


			transform.position = Vector3.Lerp(transform.position, GetLeftPosition(), Time.deltaTime * speed);

		}
	}

	void TabletModeInstant()
	{

		if (following)
		{
			LockedOnPlace = false;
			tabletCol.enabled = false;
			for (int i = 0; i < transform.childCount; i++)
			{
				tablet[i].gameObject.SetActive(false);
			}
		}
		else
		{
			if (!LockedOnPlace)
			{
				LockedOnPlace = true;
				TabletTrackPlayerHead();

				for (int i = 0; i < transform.childCount; i++)
				{
					tablet[i].gameObject.SetActive(true);
				}

				tabletCol.enabled = true;		
				transform.position = GetFrontPosition();
			}
		}
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
	public void SetFrontDistance(float value)
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
		backPosition = playerPosition.GetPosition() + new Vector3(direction.x * followDistance, height, direction.z * followDistance);
		return backPosition;
	}

	/// <summary>
	/// Calculates front position for hovertablet
	/// </summary>
	/// <returns>Vector3 front position</returns>
	public Vector3 GetFrontPosition()
	{
		Vector3 direction = playerPosition.GetRotation() * Vector3.forward;
		frontPosition = playerPosition.GetPosition() + new Vector3(direction.x * frontDistance, height, direction.z * frontDistance);
		return frontPosition;
	}

	public Vector3 GetLeftPosition()
	{
		leftPosition = playerPosition.GetPosition() + new Vector3(-playerPosition.transform.right.x * leftDistance, height, -playerPosition.transform.right.z * leftDistance);
		return leftPosition;
	}

	IEnumerator ActivateAfterTime(float time, bool status)
	{

		yield return new WaitForSeconds(time);

		for (int i = 0; i < transform.childCount; i++)
		{
			tablet[i].gameObject.SetActive(status);
		}
		tabletCol.enabled = status;
	}
}
