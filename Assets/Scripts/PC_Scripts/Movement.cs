using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// @Author = Veli-Matti Vuoti
/// </summary>
public class Movement : MonoBehaviour
{

	PcCamera pcCamera;

	#region Movement Variables
	[Range(0, 100)] public float moveSpeed;
	[Range(0,100)]public float rotationSpeed;
	[Range(0,3)] public float playerHeight;
	[Range(0, 100)] public float verticalHeadMoveSpeed;
	[Range(0, 90)] public float verticalHeadAngleLimit;
	public bool inversedVertical;
	#endregion

	float rotationX;
	Rigidbody rb;

	private void Awake()
	{
		pcCamera = gameObject.GetComponentInChildren<PcCamera>();
		pcCamera.transform.position = new Vector3 (transform.position.x,transform.position.y + playerHeight,transform.position.z);
		rb = gameObject.GetComponent<Rigidbody>();
	}

	public int IsInversed()
	{
		if (inversedVertical)
		{
			return 1;
		}
		else
		{
			return -1;
		}	
	}

	public void MoveVertical(float moveY)
	{
		//Debug.Log(moveY);
		float moveVertical = moveY * moveSpeed * Time.deltaTime;
		rb.velocity = transform.forward * moveVertical;
	}
	public void MoveHorizontal(float moveX)
	{
		//Debug.Log(moveX);
		float rotationY = moveX * rotationSpeed * Time.deltaTime;
		transform.Rotate(Vector3.up, rotationY);
	}

	public void TurnHorizontal(float mouseX)
	{

		
		//Debug.Log(mouseX);
	}

	public void MoveHeadVertical(float mouseY)
	{
		//Debug.Log(mouseY);
	
		rotationX += mouseY * verticalHeadMoveSpeed * Time.deltaTime;
		rotationX = Mathf.Clamp(rotationX, -verticalHeadAngleLimit, verticalHeadAngleLimit);
		pcCamera.transform.rotation = Quaternion.Euler(rotationX * IsInversed(), pcCamera.transform.rotation.y, pcCamera.transform.rotation.z);
		
		
	}

}
