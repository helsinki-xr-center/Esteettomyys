using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{

	[SerializeField]LayerMask hitMask;
	[SerializeField]DoorOpen door;

	private void OnTriggerEnter(Collider other)
	{
		if(other.transform.gameObject.CompareTag("Player") || other.transform.gameObject.CompareTag("MainCamera"))
		{
			Debug.Log("Open");
			door.OpenAnimation();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.transform.gameObject.CompareTag("Player") || other.transform.gameObject.CompareTag("MainCamera"))
		{
			Debug.Log("Close");
			door.CloseAnimation();
		}
	}

}
