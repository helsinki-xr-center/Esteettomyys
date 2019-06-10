using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// @Author = Veli-Matti Vuoti
/// 
/// Controls PC player skills and stuff
/// </summary>
[RequireComponent(typeof(Movement), typeof(PlayerInput))]
public class PcPlayer : MonoBehaviour
{
	[HideInInspector] public Movement movement;
	[HideInInspector] public PcCamera playerEyes;
	Transform rightHand;
	Transform leftHand;
	FixedJoint objSnapPoint;

	//public PCControlSet pCCS;

	[Range(0, 15)] public float rayCastDistance;
	public LayerMask hitMask;
	public LayerMask teleportMask;
	Camera eyeSight;
	RayCastData newHitData;

	public GameObject teleportIndicator;

	public GameObject selectedObject;
	public GameObject hoveredGameObject;

	public bool hovering;
	public bool hasObjSelected;
	public bool senderActive;
	public bool canTeleport;
	public bool telePortKeyDown;

	[SerializeField] float teleRestrictionTime;

	public delegate void MouseDelegate(object sender, RayCastData hitData);
	public static event MouseDelegate mouseHoverIn;
	public static event MouseDelegate mouseHoverOut;
	public static event MouseDelegate mouseClick;

	public delegate void OpenMenuDelegate(bool havingObj);
	public static event OpenMenuDelegate OpenMenuEvent;
	public delegate void OnDeselectObjectDelegate(GameObject obj);
	public static event OnDeselectObjectDelegate OnDeselectObjectEvent;

	private void Start()
	{
		movement = gameObject.GetComponent<Movement>();
		playerEyes = gameObject.GetComponentInChildren<PcCamera>();
		eyeSight = playerEyes.gameObject.GetComponent<Camera>();
		leftHand = gameObject.transform.GetChild(1);
		rightHand = gameObject.transform.GetChild(2);
		leftHand.transform.localPosition = new Vector3(-1, 1.5f, 1.5f);
		rightHand.transform.localPosition = new Vector3(1, 1.5f, 1.5f);
		objSnapPoint = rightHand.GetComponent<FixedJoint>();
		canTeleport = GlobalValues.settings.PCteleportAvailable;

		RayCastData newHitData = new RayCastData();
		newHitData.distance = 0;
		newHitData.hitPoint = Vector3.zero;
		newHitData.target = null;
	}

	private void OnEnable()
	{
		Settings.OnSettingsChanged += CheckSettings;
	}

	private void OnDisable()
	{
		Settings.OnSettingsChanged -= CheckSettings;
	}

	//Checks if special modes are activated
	public void CheckSettings(Settings settings)
	{

		OnWheelChairModeEnabled();

	}

	private void FixedUpdate()
	{
		FallToDeath();
		HasObjectSelected();

		if (!EventSystem.current.IsPointerOverGameObject())
		{

			RayCastToPointer();

		}
	}

	/// <summary>
	/// If the player manages to fall off from map, this function will reset the position back to start
	/// </summary>
	void FallToDeath()
	{
		if(transform.position.y <  -5f)
		{
			transform.position = Vector3.zero;
		}
	}

	/// <summary>
	/// If player has object selected
	/// </summary>
	private void HasObjectSelected()
	{
		if (selectedObject != null)
		{
			hasObjSelected = true;
		}
		else
		{
			hasObjSelected = false;
		}
	}

	/// <summary>
	/// Opens menu interaction menu if player has object selected
	/// </summary>
	public void InteractionMenu()
	{
		OpenMenuEvent?.Invoke(hasObjSelected);
	}

	/// <summary>
	/// Physical raycasts at pointers position and allows selecting/deselecting objects
	/// </summary>
	public void RayCastToPointer()
	{

		RaycastHit hit;

		Ray ray = eyeSight.ScreenPointToRay(Input.mousePosition);
		bool rayHit = Physics.Raycast(ray, out hit, rayCastDistance, hitMask);

		if (rayHit)
		{
			if (hit.collider.transform.gameObject.GetComponent<InteractableObject>())
			{
				senderActive = true;
				hovering = true;
				if (hoveredGameObject != hit.transform.gameObject && hoveredGameObject != null)
				{
					if (mouseHoverOut != null && senderActive)
					{
						newHitData.distance = hit.distance;
						newHitData.hitPoint = hit.point;
						newHitData.target = hoveredGameObject.transform;
						mouseHoverOut(this, newHitData);
					}
				}
				hoveredGameObject = hit.transform.gameObject;
				MouseHover(hit);

				//Debug.Log("HOVERING ON OBJECT ELEMENT");

				if (Input.GetMouseButtonDown(0) && hoveredGameObject == hit.transform.gameObject && !hasObjSelected)
				{
					//Debug.Log("SELECTED OBJECT");
					MouseClicked(hit);
					selectedObject = hit.transform.gameObject;
					if (hit.distance <= 2f)
					{
						//Debug.Log("PICKED UP THE OBJECT");
						canTeleport = false;
						hit.transform.position = rightHand.position;
						objSnapPoint.connectedBody = hit.rigidbody;
						objSnapPoint.connectedBody.useGravity = false;
					}
				}
				else if (Input.GetMouseButtonDown(1) && selectedObject == hoveredGameObject && selectedObject != null && hasObjSelected)
				{
					OnDeselectObjectEvent?.Invoke(selectedObject);
					selectedObject = null;
					DropObject();
				}
			}
			else
			{

				//Debug.Log("HOVERING ON UI ELEMENT");
				senderActive = true;
				hovering = true;
				hoveredGameObject = hit.transform.gameObject;
				MouseHover(hit);

				if (Input.GetMouseButtonDown(0) && senderActive)
				{
					MouseClicked(hit);
				}

			}
		}
		else
		{
			MouseExited(hit);

			if (Input.GetMouseButtonDown(1) && selectedObject != null)
			{
				OnDeselectObjectEvent?.Invoke(selectedObject);
				selectedObject = null;
				DropObject();
			}

			senderActive = false;
			hovering = false;
		}

	}

	/// <summary>
	/// Launches event if mouse is on interactable object
	/// </summary>
	/// <param name="hit"></param>
	public void MouseHover(RaycastHit hit)
	{
		if (mouseHoverIn != null && senderActive)
		{
			newHitData.distance = hit.distance;
			newHitData.hitPoint = hit.point;
			newHitData.target = hoveredGameObject.transform;
			mouseHoverIn(this, newHitData);
		}
	}

	/// <summary>
	/// Launches event if clicks mouse on interactable object
	/// </summary>
	/// <param name="hit"></param>
	public void MouseClicked(RaycastHit hit)
	{
		if (mouseClick != null && senderActive)
		{
			newHitData.distance = hit.distance;
			newHitData.hitPoint = hit.point;
			newHitData.target = hit.transform;
			mouseClick(this, newHitData);
		}
	}

	/// <summary>
	/// Launches event if mouse exits interactable object
	/// </summary>
	/// <param name="hit"></param>
	public void MouseExited(RaycastHit hit)
	{
		if (mouseHoverOut != null && senderActive)
		{
			newHitData.distance = hit.distance;
			newHitData.hitPoint = hit.point;
			newHitData.target = hoveredGameObject.transform;
			mouseHoverOut(this, newHitData);
		}
	}

	/// <summary>
	/// Drops the object if holding it
	/// </summary>
	public void DropObject()
	{
		if (objSnapPoint.connectedBody != null)
		{
			//Debug.Log("DROPPED OBJECT");		
			objSnapPoint.connectedBody.useGravity = true;
			objSnapPoint.connectedBody = null;
			StartCoroutine(ActivateTeleport(true, teleRestrictionTime));
		}
	}

	/// <summary>
	/// Will activate position marker for teleport when holding right mouse button down
	/// </summary>
	public void PortalIndicator()
	{

		if (!canTeleport || !GlobalValues.settings.PCteleportAvailable || hasObjSelected)
		{
			return;
		}
		else
		{

			telePortKeyDown = true;
			RaycastHit hit;

			Ray ray = eyeSight.ScreenPointToRay(Input.mousePosition);
			bool rayHit = Physics.Raycast(ray, out hit, rayCastDistance, teleportMask);

			if (rayHit)
			{
				if (teleportIndicator != null && !teleportIndicator.activeSelf)
				{
					teleportIndicator.SetActive(true);
				}
			}
		}
	}

	/// <summary>
	/// when release right mouse key teleports the player to pointers location
	/// </summary>
	public void PCTeleport()
	{

		if (!canTeleport || !GlobalValues.settings.PCteleportAvailable || hasObjSelected)
		{
			return;
		}
		else
		{

			RaycastHit hit;

			Ray ray = eyeSight.ScreenPointToRay(Input.mousePosition);
			bool rayHit = Physics.Raycast(ray, out hit, rayCastDistance, teleportMask);
			telePortKeyDown = false;

			if (rayHit)
			{
				if (teleportIndicator != null && !teleportIndicator.activeSelf)
				{
					teleportIndicator.SetActive(false);
				}

				transform.position = hit.point + Vector3.up * 0.01f;
			}

		}
	}

	/// <summary>
	/// Adjusts player height when activate wheelchair mode
	/// </summary>
	public void OnWheelChairModeEnabled()
	{

		if (GlobalValues.settings.wheelChairMode)
		{
			transform.localScale = new Vector3(transform.localScale.x, GlobalValues.settings.wheelChairHeight / GlobalValues.settings.defaultHeight, transform.localScale.z);
		}
		else
		{
			transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
		}

	}

	/// <summary>
	/// Delay to enable or disable teleporting again
	/// </summary>
	/// <param name="status"></param>
	/// <param name="time"></param>
	/// <returns></returns>
	public IEnumerator ActivateTeleport(bool status, float time)
	{
		yield return new WaitForSeconds(time);
		canTeleport = status;
	}
}

/// <summary>
/// struct for pointer events
/// </summary>
public struct RayCastData
{
	public float distance;
	public Vector3 hitPoint;
	public Transform target;
}
