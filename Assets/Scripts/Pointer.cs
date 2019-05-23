using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

/// <summary>
/// Struct for storing raycasthit data
/// </summary>
public struct NewRayCastData
{
	public float distance;
	public Transform target;
	public Vector3 hitPoint;
}

public delegate void PointerHitInfoDelegate(object sender, NewRayCastData hitData);

/// <summary>
/// @Author = Veli-Matti Vuoti
/// 
/// Class To Handle Pointer hit data
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

	#region Input variables
	public SteamVR_Action_Boolean pickUpMovable;
	#endregion

	#region booleans
	public bool hasTarget;
	public bool isSenderActive;
	public bool hovering;
	#endregion

	#region events
	public static event PointerHitInfoDelegate PointerHit;
	public static event PointerHitInfoDelegate PointerLeft;
	#endregion

	private void Awake()
	{
		leftHand = FindObjectOfType<Player>().gameObject.transform.GetChild(0).transform.GetChild(1).transform;
		rightHand = FindObjectOfType<Player>().gameObject.transform.GetChild(0).transform.GetChild(2).transform;
		pointerLineRenderer = gameObject.GetComponent<LineRenderer>();
		pointerDot = gameObject.transform.GetChild(0).transform.gameObject;
		originalColor = Color.white;
	}

	private void Update()
	{
		RayCastFromHand();
		DropObject();
		HoverColor();
	
	}

	/// <summary>
	/// Sets Howering Color for object
	/// </summary>
	public void HoverColor()
	{

		if (hovering && !hasTarget && targetObj != null)
		{
			ExtensionMethods.MaterialColorChange(targetObj, targetObj.GetComponent<InteractableObject>().GetHoverColor());
		}
		if (!hovering && !hasTarget && targetObj != null)
		{			
			ExtensionMethods.MaterialResetColorChange(targetObj, originalColor);
		}
	}

	/// <summary>
	/// Activates pointer
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

		if (rayHits)
		{
			hovering = true;
			targetObj = hit.collider.gameObject;
			ActivatePointerAndUpdatePosition(hit);
			//Debug.Log(hit.collider.name);
			//Debug.DrawRay(rightHand.position, rightHand.forward * rayCastLength, Color.green, 0.1f);
			SelectObject(hit);
		}
		else
		{
			hovering = false;
			ActivatePointer(false);
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
			SaveHitData(hit);			
			Debug.Log("TARGETED OBJECT");				
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
		if(PointerHit != null && isSenderActive)
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
