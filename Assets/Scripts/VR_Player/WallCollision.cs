using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

/// <summary>
/// @Author : Veli-Matti Vuoti
/// 
/// Checks if player head go inside walls or objects
/// </summary>
public class WallCollision : MonoBehaviour
{

	[SerializeField] float radius;
	PlayerPosition pos;
	[SerializeField] LayerMask hitMask;
	[SerializeField] float timeToBlack;
	[SerializeField] float timeToClear;

	private void Start()
	{
		pos = FindObjectOfType<PlayerPosition>();
	}

	private void FixedUpdate()
	{
		Collider[] wallColliders = Physics.OverlapSphere(pos.vrCamera.position, radius, hitMask);
		
		if (wallColliders.Length == 0)
		{
			SteamVR_Fade.Start(Color.clear, timeToClear);
		}

		foreach (var col in wallColliders)
		{
			if (col)
			{
				if (col.gameObject.layer == LayerMask.NameToLayer("Wall"))
				{
					StartCoroutine(TeleportToBegin(timeToBlack + timeToBlack + timeToBlack));
				}
				else
				{
					SteamVR_Fade.Start(Color.black, timeToBlack);
				}
			}
		}			
	}

	/// <summary>
	/// Teleport player to begin and clear vision
	/// </summary>
	/// <param name="time">After this time</param>
	/// <returns></returns>
	IEnumerator TeleportToBegin(float time)
	{
		SteamVR_Fade.Start(Color.black, timeToBlack);
		yield return new WaitForSeconds(time);
		transform.position = Vector3.zero;
		SteamVR_Fade.Start(Color.clear, timeToClear);
	}

	//private void OnDrawGizmos()
	//{
		
	//	Gizmos.color = Color.blue;
	//	if (GlobalValues.controllerMode == ControllerMode.VR)
	//	{
	//		Gizmos.DrawWireSphere(pos.vrCamera.position, radius);
	//	}
	//	else
	//	{
	//		Gizmos.DrawWireSphere(new Vector3(transform.position.x, 1.8f, transform.position.z), radius);
	//	}
	//}



}
