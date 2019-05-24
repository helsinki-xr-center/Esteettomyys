using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @Author = Veli-Matti Vuoti
/// </summary>
[RequireComponent(typeof(Movement),typeof(PlayerInput))]
public class PcPlayer : MonoBehaviour
{

	[HideInInspector] public Movement movement;
	[HideInInspector] public PcCamera playerEyes;

	[Range(0,15)]public float rayCastDistance;
	public LayerMask hitMask;
	Camera eyeSight;

	public GameObject selectedObject;
	public GameObject hoveredGameObject;
	public bool hovering;
	public bool hasObjSelected;

	private void Awake()
	{
		movement = gameObject.GetComponent<Movement>();
		playerEyes = gameObject.GetComponentInChildren<PcCamera>();
		eyeSight = playerEyes.gameObject.GetComponent<Camera>();
	}

	private void Update()
	{
		if(selectedObject != null)
		{
			hasObjSelected = true;
		}
		else
		{
			hasObjSelected = false;
		}

		RayCastToPointer();

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
				hovering = true;
				hoveredGameObject = hit.transform.gameObject;
				//Debug.Log("HOVERING ON OBJECT ELEMENT");

				if(Input.GetMouseButtonDown(0) && hoveredGameObject == hit.transform.gameObject && !hasObjSelected)
				{
					Debug.Log("SELECTED OBJECT");
					selectedObject = hit.transform.gameObject;
				}
				else if (Input.GetMouseButtonDown(0) && selectedObject == hoveredGameObject && selectedObject != null && hasObjSelected)
				{
					Debug.Log("DESELECTED OBJECT");
					selectedObject = null;
				}
				
			}
			else
			{

				
				//Debug.Log("HOVERING ON UI ELEMENT");
			}
		}
		else
		{

			if (Input.GetMouseButtonDown(0) && selectedObject != null)
			{
				Debug.Log("DESELECTED OBJECT");
				selectedObject = null;
			}

			hovering = false;
		}

	}

}
