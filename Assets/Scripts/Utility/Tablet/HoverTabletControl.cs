using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

/// <summary>
/// 
/// @Author: Veli-Matti Vuoti
/// 
/// This Class controls the hovertablet movement
/// currently holds 5 different movement styles:
/// 1.Follow and bring front on click
/// 2.Follow Left and bring front on click
/// 3. Follow Left and click to deactivate
/// 4. Instant appear front of hmd on click
/// 5. Instant appear front of controller on click
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

	public bool visible;
	public bool grabbed;
	public bool changedToFollow;
	public bool lockedOnPlace;
	public bool moveToSelectedObjectMode;
	public bool lockedOnObject;
	bool hand;

	public FollowMode followMode;

	public SteamVR_Action_Boolean grabClick;
	public Transform targetObj;
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

	private void OnEnable()
	{
		Pointer.SelectedObjectEvent += OnSelectedObject;
	}

	private void OnDisable()
	{
		Pointer.SelectedObjectEvent -= OnSelectedObject;
	}

	public void OnSelectedObject(bool status, Transform obj)
	{
		if (obj.gameObject.GetComponent<InteractableObject>() && status)
		{
			lockedOnObject = true;
			targetObj = obj;
		}
		else if (!status)
		{
			lockedOnObject = false;
			targetObj = null;
		}
	}

	private void FixedUpdate()
	{
		//Debug.DrawRay(transform.position, GetBackPosition(), Color.red, 0.1f);
		//Debug.DrawRay(transform.position, GetFrontPosition(), Color.blue, 0.1f);

		ChangeTabletPosition();
		TabletTrackPlayerHead();

		if (lockedOnObject)
		{	
			lockedOnPlace = false;		
			MoveToSelectedObject();
		}

		if(!lockedOnObject)
		{
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
				case FollowMode.ControllerInstant:
					TabletModeInstantController();
					break;
				default:
					break;
			}
		}
	}

	public void MoveToSelectedObject()
	{
		if (targetObj != null)
		{
			Vector3.Lerp(transform.position, targetObj.position, Time.fixedDeltaTime * speed);
		}
	}

	/// <summary>
	/// If Moving Mode This tracks player head position
	/// </summary>
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

		if (visible)
		{
			transform.position = Vector3.Lerp(transform.position, GetBackPosition(), Time.fixedDeltaTime * speed);

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

			transform.position = Vector3.Lerp(transform.position, GetFrontPosition(), Time.fixedDeltaTime * speed);
		}

	}

	/// <summary>
	/// Makes Tablet follow left and press brings it front
	/// </summary>
	void TabletModeLeft()
	{
		if (visible)
		{
			transform.position = Vector3.Lerp(transform.position, GetLeftPosition(), Time.fixedDeltaTime * speed);
		}
		else
		{
			transform.position = Vector3.Lerp(transform.position, GetFrontPosition(), Time.fixedDeltaTime * speed);
		}
	}

	/// <summary>
	/// Makes Tables follow left and press makes it disappear
	/// </summary>
	void TabletModeLeftHide()
	{

		if (visible)
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

			transform.position = Vector3.Lerp(transform.position, GetLeftPosition(), Time.fixedDeltaTime * speed);

		}
	}

	/// <summary>
	/// Instantly spawns tablet front of controller
	/// </summary>
	void TabletModeInstantController()
	{
		if (GlobalValues.controllerMode == ControllerMode.VR)
		{
			if (visible)
			{
				ActivateTablet(true);
				PositionFrontOfController(hand);
			}
			else
			{
				ActivateTablet(false);
			}
		}
	}

	/// <summary>
	/// Instantly spawn tablet front of vision
	/// </summary>
	void TabletModeInstant()
	{
		if (GlobalValues.controllerMode == ControllerMode.VR)
		{
			if (visible)
			{
				ActivateTablet(true);
				transform.position = GetFrontPosition();
			}
			else
			{
				ActivateTablet(false);
			}
		}
	}

	/// <summary>
	/// Activates or deactivates the tablet
	/// </summary>
	/// <param name="status">activate or not</param>
	void ActivateTablet(bool status)
	{
		//LockedOnPlace = status;
		tabletCol.enabled = status;
		for (int i = 0; i < transform.childCount; i++)
		{
			tablet[i].gameObject.SetActive(status);
		}
	}

	/// <summary>
	/// Brings tablet front or back of player from GribButton
	/// </summary>
	private void ChangeTabletPosition()
	{
		if (GlobalValues.controllerMode == ControllerMode.VR)
		{
			if (grabClick.GetStateDown(SteamVR_Input_Sources.RightHand))
			{
				Debug.Log("PRESSED grab " + hand);
				visible = visible ? false : true;
				
				hand = true;				
				
			}
		}
		else //If PC MODE
		{
			if (Input.GetButtonDown("Jump"))
			{
				if (visible)
				{
					visible = false;
				}
				else
				{
					visible = true;
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

	/// <summary>
	/// Sets Tablet Poisition to front of tracked controller
	/// </summary>
	/// <param name="right"></param>
	public void PositionFrontOfController(bool isRightHand)
	{
		if (isRightHand)
		{
			Vector3 direction = playerPosition.GetRightHandRotation() * Vector3.forward;
			transform.position = /*playerPosition.rightHand.GetComponent<Hand>().trackedObject.transform.position*/playerPosition.GetRightHandPosition() + new Vector3(direction.x, direction.y, direction.z);
		}
		else
		{
			Vector3 direction = playerPosition.GetLeftHandRotation() * Vector3.forward;
			transform.position =/* playerPosition.leftHand.GetComponent<Hand>().trackedObject.transform.position*/ playerPosition.GetLeftHandPosition() + new Vector3(direction.x, direction.y, direction.z);
		}
	}

	/// <summary>
	/// Activate or Deactivate Tablet with Delay
	/// </summary>
	/// <param name="time"></param>
	/// <param name="status"></param>
	/// <returns></returns>
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
