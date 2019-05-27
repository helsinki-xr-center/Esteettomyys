using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;
using UnityEngine.UI;

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
	public GameObject targetObj = null;
	public GameObject selectedObj = null;
	LineRenderer pointerLineRenderer;
	#endregion

	[Header("Variables for Raycasting")]
	[Range(0, 15)] public float rayCastLength;
	[Tooltip("Object's that raycast will hit")] public LayerMask hitMask;
	[SerializeField] [Tooltip("Selected object color indicator")] Color selectedColor;
	Color originalColor;
	public static Pointer instance = null;

	#region Input variables
	public SteamVR_Action_Boolean selectObj;
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
	public delegate void PointerHitInfoDelegate(object sender, RayCastData hitData);
	public static event PointerHitInfoDelegate PointerHit;
	public static event PointerHitInfoDelegate PointerLeft;
	public static event PointerHitInfoDelegate PointerClick;
	#endregion

	/// <summary>
	/// Finds the needed components for pointer to work and sets original color to white
	/// </summary>
	private void Start()
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
		HasObjectSelected();
		RayCastFromHand();
	}

	private void HasObjectSelected()
	{
		if (selectedObj != null)
		{
			hasTarget = true;
		}
		else
		{
			hasTarget = false;
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
	public void UpdatePointerPositionByHitPoint(RaycastHit hit)
	{

		pointerLineRenderer.SetPosition(0, rightHand.position);
		pointerLineRenderer.SetPosition(1, hit.point);
		pointerDot.transform.position = hit.point;
	}

	public void UpdatePointerPositionCalculated()
	{
		pointerLineRenderer.SetPosition(0, rightHand.position);
		pointerLineRenderer.SetPosition(1, rightHand.position + rightHand.forward * rayCastLength);
		pointerDot.transform.position = rightHand.position + rightHand.forward * rayCastLength;
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
			ActivatePointer(true);
			UpdatePointerPositionCalculated();
		}

		if (rayHits)
		{
			if (!lockLaserOn)
			{
				ActivatePointer(true);
				UpdatePointerPositionByHitPoint(hit);
			}

			if (hit.collider.transform.gameObject.GetComponent<InteractableObject>())
			{
				isSenderActive = true;
				hovering = true;

				if (targetObj != hit.transform.gameObject && targetObj != null)
				{
					if (PointerLeft != null && isSenderActive)
					{
						RayCastData newHitData = new RayCastData();
						newHitData.distance = hit.distance;
						newHitData.hitPoint = hit.point;
						newHitData.target = targetObj.transform;
						PointerLeft(this, newHitData);
					}
				}
				targetObj = hit.transform.gameObject;
				OnPointerHover(hit);

				//Debug.Log("HOVERING ON OBJECT");

				if (selectObj.GetStateDown(SteamVR_Input_Sources.RightHand) && targetObj == hit.transform.gameObject && !hasTarget)
				{
					Debug.Log("SELECTED OBJECT");
					OnPointerClick(hit);
					selectedObj = hit.transform.gameObject;

				}
				else if (selectObj.GetStateDown(SteamVR_Input_Sources.RightHand) && selectedObj == targetObj && selectedObj != null && hasTarget)
				{
					Debug.Log("DESELECTED OBJECT");
					selectedObj = null;

					DropObject();
				}
			}
			else
			{

				//Debug.Log("HOVERING ON UI ELEMENT");
				isSenderActive = true;
				hovering = true;
				targetObj = hit.transform.gameObject;
				OnPointerHover(hit);

				if (clickUIButton.GetStateDown(SteamVR_Input_Sources.RightHand) && isSenderActive)
				{
					OnPointerClick(hit);
				}
			}
		}
		else
		{
			OnPointerLeft(hit);

			if (selectObj.GetStateDown(SteamVR_Input_Sources.RightHand) && selectedObj != null)
			{
				Debug.Log("DESELECTED OBJECT");
				selectedObj.GetComponent<InteractableObject>().selected = false;
				ExtensionMethods.MaterialColorChange(selectedObj, Color.white);
				selectedObj = null;

				DropObject();
			}
			if (!lockLaserOn)
			{
				ActivatePointer(false);
			}

			isSenderActive = false;
			hovering = false;
		}
	}

	/// <summary>
	/// Drops Hit Target After Releasing Trigger UP and Triggers Event with the raycast data
	/// </summary>
	public void DropObject()
	{

		Debug.Log("DESELECTED OBJECT");

		//SaveExitData(targetObj.transform);			
		hasTarget = false;
		selectedObj = null;

	}

	public void OnPointerHover(RaycastHit hit)
	{
		if (PointerHit != null && isSenderActive)
		{
			RayCastData newHitData = new RayCastData();
			newHitData.distance = hit.distance;
			newHitData.hitPoint = hit.point;
			newHitData.target = targetObj.transform;
			PointerHit(this, newHitData);
		}
	}

	public void OnPointerClick(RaycastHit hit)
	{
		if (PointerClick != null && isSenderActive)
		{
			RayCastData newHitData = new RayCastData();
			newHitData.distance = hit.distance;
			newHitData.hitPoint = hit.point;
			newHitData.target = targetObj.transform;
			PointerClick(this, newHitData);
		}
	}

	public void OnPointerLeft(RaycastHit hit)
	{
		if (PointerLeft != null && isSenderActive)
		{
			RayCastData newHitData = new RayCastData();
			newHitData.distance = hit.distance;
			newHitData.hitPoint = hit.point;
			newHitData.target = targetObj.transform;
			PointerLeft(this, newHitData);
		}
	}

}
