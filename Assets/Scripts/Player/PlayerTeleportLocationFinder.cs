using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Author: Nomi Lakkala
 * <summary>
 * A static helper class that finds a suitable teleport location near the desired target.
 * </summary>
 */
public static class PlayerTeleportLocationFinder
{
	private const float range = 2f;
	private const float colliderRange = 1f;


	/**
	 * <summary>
	 * Finds a position around the targetPosition with no intersecting colliders. Y is checked to be the ground level.
	 * Rather expensive in terms of GC allocations so don't use this every frame for some reason.
	 * </summary>
	 */
	public static Vector3 FindTeleportLocation(Vector3 targetPosition)
	{

		for(float r = 0; r < 360; r += 45)
		{
			Vector3 offset = Quaternion.Euler(Vector3.up * r) * Vector3.forward * range;
			Ray ray = new Ray(targetPosition + offset + Vector3.up, Vector3.down);

			if(Physics.Raycast(ray,out RaycastHit hit, 2, LayerMask.GetMask("Floor")))
			{
				if(Physics.OverlapSphere(hit.point + Vector3.up * (colliderRange + 0.5f), colliderRange).Length == 0)
				{
					return hit.point;
				}
			}
		}

		return targetPosition;
	}
}
