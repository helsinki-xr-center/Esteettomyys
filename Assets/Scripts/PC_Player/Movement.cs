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
	[SerializeField] [Range(0, 300)] float moveSpeed = 200f;
	[SerializeField] [Range(0, 300)] float strafeSpeed = 155f;
	[SerializeField] [Range(0, 300)] float rotationSpeed = 130f;
	[SerializeField] [Range(0, 300)] float verticalHeadMoveSpeed = 100f;
	[SerializeField] [Range(0, 90)] float verticalHeadAngleLimit = 90f;
	#endregion

	#region Player Setup
	[SerializeField] [Range(0, 3)] float playerHeight = 1.8f;
	[SerializeField] [Range(0, 3)] float playerWidth = 0.5f;
	[SerializeField] bool inversedVertical;
	#endregion

	float rotationX;
	float rotationY;
	Rigidbody rb;
	BoxCollider col;

	private void Start()
	{
		pcCamera = gameObject.GetComponentInChildren<PcCamera>();
		pcCamera.transform.position = new Vector3 (transform.position.x,transform.position.y + playerHeight,transform.position.z);
		rb = gameObject.GetComponent<Rigidbody>();
		col = gameObject.GetComponent<BoxCollider>();
		col.size = new Vector3(playerWidth, playerHeight, playerWidth);
	}

	public void SetInversed(bool status)
	{
		inversedVertical = status;
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
