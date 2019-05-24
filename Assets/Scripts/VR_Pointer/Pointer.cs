using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;
using UnityEngine.UI;

/// <summary>
/// Struct for storing raycasthit data
/// </summary>
public struct NewRayCastData
{
	public float distance;
	public Transform target;
	public Vector3 hitPoint;
}

/// <summary>
/// @Author = Veli-Matti Vuoti
/// 
/// the Pointer class, tracks the pointer hit data and events
/// </summary>
public class Pointer : MonoBehaviour
{

	#region Necessary Components
	[SerializeField] Transform leftHand;
	[SerializeField] Transform rightHand;
	[SerializeField] GameObject pointerDot;
	[SerializeField] GameObject targetObj = null;
	[SerializeField] GameObject selectedObj = null;
	LineRenderer pointerLineRenderer;
	#endregion

	[Header("Variables for Raycasting")]
	[Range(0, 15)] public float rayCastLength;
	[Tooltip("Object's that raycast will hit")] public LayerMask hitMask;
	[SerializeField] [Tooltip("Selected object color indicator")] Color selectedColor;
	Color originalColor;
	public static Pointer instance = null;

	#region Input variables
	public SteamVR_Action_Boolean pickUpMovable;
	public SteamVR_Action_Boolean clickUIButton;
	#endregion

	#region booleans
	[Tooltip("The current selected object with the pickUpMovable input click")] public bool hasTarget;
	[Tooltip("Enables Pointer hit events")] public bool isSenderActive;
	[Tooltip("Enabled when pointer is pointing on target")] public bool hovering;
	public bool lockLaserOn;
	#endregion

	/// <summary>
	/// Delegate Event for sending raycast data
	/// </summary>
	/// <param name="sender"> this class </param>
	/// <param name="hitData"> Raycast hit data </param>
	#region events
	public delegate void PointerHitInfoDelegate(object sender, NewRayCastData hitData);
	public static event PointerHitInfoDelegate PointerHit;
	public static event PointerHitInfoDelegate PointerLeft;
	public static event PointerHitInfoDelegate PointerClick;
	#endregion

	/// <summary>
	/// Finds the needed components for pointer to work and sets original color to white
	/// </summary>
	private void Awake()
	{
		leftHand = FindObjectOfType<Player>().gameObject.transform.GetChild(0).transform.GetChild(1).transform;
		rightHand = FindObjectOfType<Player>().gameObject.transform.GetChild(0).transform.GetChild(2).transform;
		pointerLineRenderer = gameObject.GetComponent<LineRenderer>();
		pointerDot = gameObject.transform.GetChild(0).transform.gameObject;
		originalColor = Color.white;
		instance = this;
	}

	private void Update()
	{
		RayCastFromHand();
		DropObject();
		HoverColor();

	}

	/// <summary>
	/// Sets the Hovering Color for object
	/// </summary>
	public void HoverColor()
	{

		if (hovering && !hasTarget && targetObj != null && targetObj.GetComponent<InteractableObject>())
		{

			ExtensionMethods.MaterialColorChange(targetObj, targetObj.GetComponent<InteractableObject>().GetHoverColor());
		}
		else if (!hovering && !hasTarget && targetObj != null && targetObj.GetComponent<InteractableObject>())
		{
			ExtensionMethods.MaterialResetColorChange(targetObj, originalColor);

		}
	}

	/// <summary>
	/// Activates the pointer graphics
	/// </summary>
	/// <param name="status">True/False</param>
	public void ActivatePointer(bool status)
	{
		pointerLineRenderer.enabled = status;
		pointerDot.SetActive(status);
	}

	/// <summary>
	/// Activates pointer graphics and sets it to correct position
	/// </summary>
	/// <param name="hit"></param>
	public void ActivatePointerAndUpdatePosition(RaycastHit hit)
	{
		pointerLineRenderer.enabled = true;
		pointerDot.SetActive(true);
		pointerLineRenderer.SetPosition(0, rightHand.position);
		pointerLineRenderer.SetPosition(1, hit.point);
		pointerDot.transform.position = hit.point;
	}

	/// <summary>
	/// Shoots Ray from Right Hand's Controller forward and Checks if hits something
	/// Enables Pointer LineRenderer + dot and sets the correct positions
	/// </summary>
	public void RayCastFromHand()
	{
		RaycastHit hit;

		bool rayHits = Physics.Raycast(rightHand.position, rightHand.forward, out hit, rayCastLength, hitMask);

		//Debug.DrawRay(rightHand.position, rightHand.forward * rayCastLength, Color.red, 0.1f);
		if (lockLaserOn)
		{
			ActivatePointer();
			UpdatePointerPositionCalculated();
		}

		if (rayHits)
		{
			isSenderActive = true;

			ActivatePointer();
			UpdatePointerPositionByHitPoint(hit);

			if (targetObj && targetObj != hit.transform.gameObject)
			{
				SaveHitData(hit);
			}
			else if (targetObj && targetObj == hit.transform.gameObject && !hovering)
			{
				SaveHitData(hit);
			}

			if (hit.transform.gameObject.GetComponent<InteractableObject>())
			{
				hovering = true;
				targetObj = hit.collider.gameObject;
				//Debug.Log(hit.collider.name);
				//Debug.DrawRay(rightHand.position, rightHand.forward * rayCastLength, Color.green, 0.1f);
				SelectObject(hit);
			}
			else
			{
				//Debug.Log("HITS UI ELEMENT");
				targetObj = hit.transform.gameObject;
				hovering = true;

				if (clickUIButton.GetStateDown(SteamVR_Input_Sources.RightHand))
				{
					if (PointerClick != null && isSenderActive)
					{
						NewRayCastData hitData = new NewRayCastData();
						hitData.distance = hit.distance;
						hitData.hitPoint = hit.point;
						hitData.target = hit.transform;
						PointerClick(this, hitData);
					}
				}
			}
		}
		else
		{
			hovering = false;

			if (!lockLaserOn)
			{
				ActivatePointer(false);
			}

			if (targetObj)
			{
				SaveExitData(targetObj.transform);
			}
			isSenderActive = false;
		}

	}

	/// <summary>
	/// PickUp hit target When Trigger is pressed fully down and Triggers Event with the raycast data
	/// </summary>
	/// <param name="hit">hitInfo from raycast</param>
	public void SelectObject(RaycastHit hit)
	{

		if (pickUpMovable.GetLastStateDown(SteamVR_Input_Sources.RightHand) && !hasTarget)
		{
			selectedObj = targetObj;
			ExtensionMethods.MaterialColorChange(selectedObj, selectedObj.GetComponent<InteractableObject>().GetSelectedColor());

			Debug.Log("SELECTED OBJECT");
			hasTarget = true;
		}
	}

	/// <summary>
	/// Drops Hit Target After Releasing Trigger UP and Triggers Event with the raycast data
	/// </summary>
	public void DropObject()
	{
		if (pickUpMovable.GetStateDown(SteamVR_Input_Sources.RightHand) && selectedObj != null && hasTarget)
		{
			Debug.Log("DESELECTED OBJECT");
			ExtensionMethods.MaterialResetColorChange(selectedObj, originalColor);
			//SaveExitData(targetObj.transform);			
			hasTarget = false;
			selectedObj = null;
		}
	}

	/// <summary>
	/// When Holding object and dropping it saves the endlocation
	/// </summary>
	/// <param name="obj">Hit Info from raycast</param>
	public void SaveExitData(Transform obj)
	{
		//Debug.Log("I SHOULDN'T HAPPEN OFTEN");
		NewRayCastData hitData = new NewRayCastData();
		hitData.distance = (transform.position - targetObj.transform.position).magnitude;
		hitData.hitPoint = targetObj.transform.position - transform.position;
		hitData.target = targetObj.transform;
		//Debug.Log(hitData.distance);
		//Debug.Log(hitData.hitPoint);
		//Debug.Log(hitData.target);
		OnPointerLeft(hitData);
	}

	/// <summary>
	/// Saves the start location of raycast hit
	/// </summary>
	/// <param name="hit">Hit Info from raycast</param>
	public void SaveHitData(RaycastHit hit)
	{
		//Debug.Log("I SHOULDN'T HAPPEN OFTEN");
		NewRayCastData hitData = new NewRayCastData();
		hitData.distance = hit.distance;
		hitData.hitPoint = hit.point;
		hitData.target = hit.collider.transform;
		//Debug.Log(hitData.distance);
		//Debug.Log(hitData.hitPoint);
		//Debug.Log(hitData.target);
		OnPointerHit(hitData);
	}

	/// <summary>
	/// Pointer Hit Event
	/// </summary>
	/// <param name="hitData">Data From Raycast hit</param>
	public void OnPointerHit(NewRayCastData hitData)
	{
		if (PointerHit != null && isSenderActive)
		{
			PointerHit(this, hitData);
		}
	}

	/// <summary>
	/// Pointer Left Event
	/// </summary>
	/// <param name="hitData">Data From Raycast hit</param>
	public void OnPointerLeft(NewRayCastData hitData)
	{
		if (PointerHit != null && isSenderActive)
		{
			PointerLeft(this, hitData);
		}
	}

}
