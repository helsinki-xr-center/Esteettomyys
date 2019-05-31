using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Just a script for marking a spawn location.
 * </summary>
 */
public class SpawnLocation : MonoBehaviour {
	private const float yOffset = 0.1f;

	[SerializeField]
	private float collisionCheckRange = 1f;

	private static Collider[] collisionBuffer = new Collider[2];

	public bool IsFree(){
		return Physics.OverlapSphereNonAlloc(transform.position + Vector3.up * (collisionCheckRange + yOffset), collisionCheckRange, collisionBuffer) == 0;
	}

	public void OnDrawGizmosSelected()
	{
		if (Physics.OverlapSphereNonAlloc(transform.position + Vector3.up * (collisionCheckRange + yOffset), collisionCheckRange, collisionBuffer) == 0)
		{
			Gizmos.color = Color.green;
		}else{
			Gizmos.color = Color.red;
		}

		Gizmos.DrawWireSphere(transform.position + Vector3.up * (collisionCheckRange + yOffset), collisionCheckRange);

	}
}
