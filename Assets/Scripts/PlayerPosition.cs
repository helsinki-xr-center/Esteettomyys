﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Passes vrCamera position and rotation to avatar for network syncing purposes. Should be attached to both VR and PC player roots.
 * </summary>
 */
public class PlayerPosition : MonoBehaviour
{
	public Transform vrCamera;


	/**
	 * <summary>
	 * Returns the position of the player. Y component is expected to be on ground level.
	 * </summary>
	 */
	public Vector3 GetPosition()
	{
		if (vrCamera == null)
		{
			return transform.position;
		}

		return new Vector3(vrCamera.position.x, transform.position.y, vrCamera.position.z);
	}

	/**
	 * <summary>
	 * Returns the rotation of the player.
	 * </summary>
	 */
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
