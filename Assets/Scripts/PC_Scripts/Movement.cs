using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

	PcCamera pcCamera;

	[Range(0,100)]public float rotationSpeed;
	[Range(0,3)] public float playerHeight;
	

	private void Awake()
	{
		pcCamera = gameObject.GetComponentInChildren<PcCamera>();
		pcCamera.transform.position = new Vector3 (transform.position.x,transform.position.y + playerHeight,transform.position.z);
	}

	public void MoveVertical(float moveY)
	{
		//Debug.Log(moveY);
	}
	public void MoveHorizontal(float moveX)
	{
		//Debug.Log(moveX);
	}

	public void TurnHorizontal(float mouseX)
	{

		transform.Rotate(Vector3.up, mouseX * rotationSpeed * Time.deltaTime);
		//Debug.Log(mouseX);
	}

	public void MoveHeadVertical(float mouseY)
	{
		//Debug.Log(mouseY);
		pcCamera.transform.Rotate(Vector3.left * mouseY);
		
	}

}
