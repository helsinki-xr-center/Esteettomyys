using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// @Author = Veli-Matti Vuoti
/// </summary>
[RequireComponent(typeof(Movement),typeof(PlayerInput))]
public class PcPlayer : MonoBehaviour
{
	[HideInInspector] public Movement movement;
	[HideInInspector] public PcCamera playerEyes;
	Transform rightHand;
	Transform leftHand;
	FixedJoint objSnapPoint;

	[Range(0, 15)] public float rayCastDistance;
	public LayerMask hitMask;
	public LayerMask teleportMask;
	Camera eyeSight;
	RayCastData newHitData;

	public GameObject selectedObject;
	public GameObject hoveredGameObject;
	public bool hovering;
	public bool hasObjSelected;
	public bool senderActive;
	public bool canTeleport;
	public GameObject teleportIndicator;

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
		leftHand.transform.localPosition = new Vector3(-1,1.5f,1.5f);
		rightHand.transform.localPosition = new Vector3(1,1.5f,1.5f);
		objSnapPoint = rightHand.GetComponent<FixedJoint>();

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

	public void CheckSettings(Settings settings)
	{

		OnWheelChairModeEnabled();

	}

	private void FixedUpdate()
	{

		HasObjectSelected();

		if (!EventSystem.current.IsPointerOverGameObject())
		{
			RayCastToPointer();
		}
	}

	private void HasObjectSelected()
	{
		if (selectedObject != null)
		{
			canTeleport = false;
			hasObjSelected = true;
		}
		else
		{
			canTeleport = true;
			hasObjSelected = false;
		}
	}

	public void InteractionMenu()
	{
		OpenMenuEvent?.Invoke(hasObjSelected);
	}

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
				if(hoveredGameObject != hit.transform.gameObject && hoveredGameObject != null)
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

	public void DropObject()
	{
		if (objSnapPoint.connectedBody != null)
		{
			//Debug.Log("DROPPED OBJECT");		
			objSnapPoint.connectedBody.useGravity = true;
			objSnapPoint.connectedBody = null;
			
		}
	}

	public void PortalIndicator()
	{
		RaycastHit hit;

		Ray ray = eyeSight.ScreenPointToRay(Input.mousePosition);
		bool rayHit = Physics.Raycast(ray, out hit, rayCastDistance, teleportMask);

		if (rayHit)
		{
			if ( teleportIndicator != null && !teleportIndicator.activeSelf)
			{
				teleportIndicator.SetActive(true);
			}
		}
	}

	public void PCTeleport()
	{
		RaycastHit hit;

		Ray ray = eyeSight.ScreenPointToRay(Input.mousePosition);
		bool rayHit = Physics.Raycast(ray, out hit, rayCastDistance, teleportMask);

		if (rayHit && canTeleport)
		{
			if (teleportIndicator != null && !teleportIndicator.activeSelf)
			{
				teleportIndicator.SetActive(false);
			}

			transform.position = hit.point + Vector3.up*0.01f;
		}

	}

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
}

public struct RayCastData
{
	public float distance;
	public Vector3 hitPoint;
	public Transform target;
}
