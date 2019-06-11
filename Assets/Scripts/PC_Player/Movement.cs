using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @Author = Veli-Matti Vuoti
/// 
/// Moves PC player
/// </summary>
public class Movement : MonoBehaviour
{

	PcCamera pcCamera;

	public PCControlSet pCCS;

	#region Movement Variables
	[SerializeField] [Range(0, 300)] float moveSpeed = 200f;
	[SerializeField] [Range(0, 300)] float strafeSpeed = 155f;
	[SerializeField] [Range(0, 300)] float rotationSpeed = 130f;
	[SerializeField] [Range(0, 300)] float verticalHeadMoveSpeed = 100f;
	[SerializeField] [Range(0, 90)] float verticalHeadAngleLimit = 90f;
	#endregion

	#region Player Setup
	[SerializeField] bool inversedVertical;
	#endregion

	public bool lockRotation;
	public bool resetedRotX;
	float moveX, moveY, mouseX, mouseY, turnX;

	float rotationX;
	float rotationY;
	Quaternion previousRotation;
	Rigidbody rb;
	CapsuleCollider col;

	private void Start()
	{
		pcCamera = gameObject.GetComponentInChildren<PcCamera>();
		pcCamera.transform.position = new Vector3 (transform.position.x,transform.position.y + GlobalValues.settings.defaultHeight,transform.position.z);
		rb = gameObject.GetComponent<Rigidbody>();
		col = gameObject.GetComponent<CapsuleCollider>();
		col.height = GlobalValues.settings.defaultHeight;
		col.center = Vector3.up * (GlobalValues.settings.defaultHeight / 2);
		previousRotation = transform.rotation;
	}

	/// <summary>
	/// Sets Inversed vertical head rotation
	/// </summary>
	/// <param name="status"></param>
	public void SetInversed(bool status)
	{
		inversedVertical = status;
	}

	//Checks if inversed
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

	private void FixedUpdate()
	{
		switch (pCCS)	
		{
			case PCControlSet.First:
				DirectionalMove();

				if (!lockRotation)
				{
					RotationalMove();
				}
				break;
			case PCControlSet.Second:

				DirectionalMoveTwo();			
				RotationalMoveTwo();
				
				break;
			default:
				break;
		}			
	}

	#region FirstControls
	/// <summary>
	/// Moves player to different directions
	/// </summary>
	/// <param name="moveX">Horizontal Key Input</param>
	/// <param name="moveY">Vertical Key Input</param>
	public void DirectionalMove()
	{
		//Debug.Log(moveY);
		float moveVertical = moveY * moveSpeed * Time.deltaTime;
		float moveHorizontal = moveX * strafeSpeed * Time.deltaTime;
		Vector3 moveDirection = new Vector3( moveHorizontal, rb.velocity.y , moveVertical);
		moveDirection = transform.TransformDirection(moveDirection);
		rb.velocity = moveDirection;
	}

	/// <summary>
	/// Rotates player to different rotations
	/// </summary>
	/// <param name="mouseX">Horizontal Mouse Input</param>
	/// <param name="mouseY">Vertical Mouse Input</param>
	public void RotationalMove()
	{

		rotationY = mouseX * rotationSpeed * Time.deltaTime;
		rotationX += mouseY * verticalHeadMoveSpeed * Time.deltaTime;
		rotationX = Mathf.Clamp(rotationX, -verticalHeadAngleLimit, verticalHeadAngleLimit);
		pcCamera.transform.rotation = Quaternion.Euler(rotationX * IsInversed(), transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
		transform.Rotate(Vector3.up, rotationY);
		//Debug.Log(mouseX);
	}
	#endregion

	#region SecondControls
	public void DirectionalMoveTwo()
	{
		float moveVertical = moveY * moveSpeed * Time.deltaTime;
		float moveHorizontal = moveX * strafeSpeed * Time.deltaTime;
		Vector3 moveDirection = new Vector3(moveHorizontal, rb.velocity.y, moveVertical);
		moveDirection = transform.TransformDirection(moveDirection);
		rb.velocity = moveDirection;
	}

	public void RotationalMoveTwo()
	{
		
		if (lockRotation)
		{
		
			Debug.Log(pcCamera.transform.rotation.x);
			if (pcCamera.transform.rotation.x > 0.05f || pcCamera.transform.rotation.x < -0.05f)
			{			
				previousRotation = transform.rotation;
				
				resetedRotX = false;
			}		
			rotationY = mouseX * rotationSpeed * Time.deltaTime;
			rotationX += mouseY * verticalHeadMoveSpeed * Time.deltaTime;
			rotationX = Mathf.Clamp(rotationX, -verticalHeadAngleLimit, verticalHeadAngleLimit);
			pcCamera.transform.rotation = Quaternion.Euler(rotationX * IsInversed(), transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
			transform.Rotate(Vector3.up, rotationY);	
		}
		else if (!lockRotation)
		{		
			rotationY = turnX * rotationSpeed * Time.deltaTime;
			transform.Rotate(Vector3.up, rotationY);
		}
		//Debug.Log(mouseX);
	}

	public IEnumerator ResetLookAxis()
	{
		while(!resetedRotX )
		{
			Debug.Log(pcCamera.transform.rotation.x);
			pcCamera.transform.rotation = Quaternion.Slerp(pcCamera.transform.rotation, previousRotation, Time.deltaTime);

			if (pcCamera.transform.rotation.x < 0.05f && pcCamera.transform.rotation.x > -0.05f)
			{
				resetedRotX = true;
			}
		
			yield return null;
		}

	}
	#endregion

	public void RotationalInput(float mouseX, float mouseY)
	{
		this.mouseX = mouseX;
		this.mouseY = mouseY;
	}

	public void DirectionalInput(float moveX, float moveY)
	{
		this.moveX = moveX;
		this.moveY = moveY;
	}

	public void TurnInput ( float turnX )
	{
		this.turnX = turnX;
	}
}
