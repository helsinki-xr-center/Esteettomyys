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
	[Range(0, 200)] public float moveSpeed;
	[Range(0, 200)] public float strafeSpeed;
	[Range(0,200)]public float rotationSpeed;
	[Range(0,3)] public float playerHeight;
	[Range(0, 200)] public float verticalHeadMoveSpeed;
	[Range(0, 90)] public float verticalHeadAngleLimit;
	public bool inversedVertical;
	#endregion

	float rotationX;
	float rotationY;
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

	public void DirectionalMove(float moveX , float moveY)
	{
		//Debug.Log(moveY);
		float moveVertical = moveY * moveSpeed * Time.deltaTime;
		float moveHorizontal = moveX * strafeSpeed * Time.deltaTime;
		Vector3 moveDirection = new Vector3( moveHorizontal, transform.position.y , moveVertical);
		moveDirection = transform.TransformDirection(moveDirection);
		rb.velocity = moveDirection;
	}

	public void RotationalMove(float mouseX, float mouseY)
	{

		rotationY = mouseX * rotationSpeed * Time.deltaTime;
		rotationX += mouseY * verticalHeadMoveSpeed * Time.deltaTime;
		rotationX = Mathf.Clamp(rotationX, -verticalHeadAngleLimit, verticalHeadAngleLimit);
		pcCamera.transform.rotation = Quaternion.Euler(rotationX * IsInversed(), transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
		transform.Rotate(Vector3.up, rotationY);
		//Debug.Log(mouseX);
	}

}
