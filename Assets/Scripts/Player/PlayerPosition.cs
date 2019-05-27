using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Provides positional information of the player to avatar for network syncing purposes. Should be attached to both VR and PC player roots.
 * </summary>
 */
public class PlayerPosition : MonoBehaviour
{
	[Header("Set these only in VR")]
	public Transform vrCamera;
	public Transform rightHand;
	public Transform leftHand;


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

	/**
	 * <summary>
	 * Returns whether this script is tracking the player hands or not
	 * </summary>
	 */
	public bool IsTrackingHands()
	{
		return rightHand != null && leftHand != null;
	}

	/**
	 * <summary>
	 * Returns whether this script is tracking the player head or not
	 * </summary>
	 */
	public bool IsTrackingHead()
	{
		return vrCamera != null;
	}

	/**
	 * <summary>
	 * Returns the players right hand position in world space. If not tracking hands, returns the transform.position.
	 * </summary>
	 */
	public Vector3 GetRightHandPosition()
	{
		if (rightHand == null) return transform.position;

		return rightHand.position;
	}

	/**
	 * <summary>
	 * Returns the players left hand position in world space. If not tracking hands, returns the transform.position.
	 * </summary>
	 */
	public Vector3 GetLeftHandPosition()
	{
		if (leftHand == null) return transform.position;

		return leftHand.position;
	}

	/**
	 * <summary>
	 * Returns the players right hand rotation in world space. If not tracking hands, returns Quaternion.identity.
	 * </summary>
	 */
	public Quaternion GetRightHandRotation()
	{
		if (rightHand == null) return Quaternion.identity;

		return rightHand.rotation;
	}


	/**
	 * <summary>
	 * Returns the players left hand rotation in world space. If not tracking hands, returns Quaternion.identity.
	 * </summary>
	 */
	public Quaternion GetLeftHandRotation()
	{
		if (leftHand == null) return Quaternion.identity;

		return leftHand.rotation;
	}

	/**
	 * <summary>
	 * Returns height offset of the head measured from eye level. If not tracking head, returns 0.
	 * </summary>
	 */
	public float GetHeadHeightFromBase(){
		if (vrCamera == null) return 0;

		return vrCamera.position.y - transform.position.y;
	}
}
