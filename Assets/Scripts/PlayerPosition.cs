using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Author: Nomi Lakkala
 * 
 * 
 */
public class PlayerPosition : MonoBehaviour
{
	public Transform vrCamera;

	public Vector3 GetPosition()
	{
		if (vrCamera == null)
		{
			return transform.position;
		}

		return new Vector3(vrCamera.position.x, transform.position.y, vrCamera.position.z);
	}

	public Quaternion GetRotation()
	{
		if (vrCamera == null)
		{
			return transform.rotation;
		}
		Vector3 vrRot = vrCamera.eulerAngles;
		return Quaternion.Euler(new Vector3(0, vrRot.y, 0));
	}
}
