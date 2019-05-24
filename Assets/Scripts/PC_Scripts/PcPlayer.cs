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

	private void Awake()
	{
		movement = gameObject.GetComponent<Movement>();
		playerEyes = gameObject.GetComponentInChildren<PcCamera>();
		eyeSight = playerEyes.gameObject.GetComponent<Camera>();
	}

	private void Update()
	{

		RayCastToPointer();

	}

	public void RayCastToPointer()
	{

		RaycastHit hit;
		
		Ray ray = eyeSight.ScreenPointToRay(Input.mousePosition);
		bool rayHit = Physics.Raycast(ray, out hit, rayCastDistance, hitMask);

		if (rayHit)
		{
			
		}

	}

}
