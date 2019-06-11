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
	PcPlayer player;

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
	public bool rotating;
	public Transform resetPos;
	float moveX, moveY, mouseX, mouseY, turnX;
	Vector3 mousePos;
	[Tooltip("Bounds Where mouse moves the camera")] public float leftBounds, rightBounds, upBounds, downBounds;


	public float slerpTime;
	public float mouseRotateSpeedY;
	public float mouseRotateSpeedX;
	float time;
	float timer;
	float rotationX;
	float rotationY;
	Quaternion previousRotation;
	Rigidbody rb;
	CapsuleCollider col;

	private void Start()
	{
		pcCamera = gameObject.GetComponentInChildren<PcCamera>();
		pcCamera.transform.position = new Vector3(transform.position.x, transform.position.y + GlobalValues.settings.defaultHeight, transform.position.z);
		rb = gameObject.GetComponent<Rigidbody>();
		col = gameObject.GetComponent<CapsuleCollider>();
		player = gameObject.GetComponent<PcPlayer>();
		col.height = GlobalValues.settings.defaultHeight;
		col.center = Vector3.up * (GlobalValues.settings.defaultHeight / 2);
		resetPos = transform.GetChild(0).transform;
		

		StartSlerping();
		StartCoroutine(ResetLookAxis());
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

				CheckRotationStatus();
				DirectionalMoveTwo();
				RotationalMoveTwo();	
				if (!player.activeUI )
				{
					MouseOnEdge();
				}

				break;
			default:
				break;
		}
	}

	/// <summary>
	/// Different Move Style
	/// </summary>
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
		Vector3 moveDirection = new Vector3(moveHorizontal, rb.velocity.y, moveVertical);
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

	/// <summary>
	/// Different Move Style
	/// </summary>
	#region SecondControls
	
	public void DirectionalMoveTwo()
	{
		float moveVertical = moveY * moveSpeed * Time.deltaTime;
		float moveHorizontal = moveX * strafeSpeed * Time.deltaTime;
		Vector3 moveDirection = new Vector3(moveHorizontal, rb.velocity.y, moveVertical);
		moveDirection = transform.TransformDirection(moveDirection);
		rb.velocity = moveDirection;
	}

	public void ResetRotationX()
	{
		rotationX = 0;
		resetedRotX = true;
	}

	/// <summary>
	/// Checks if rotation is same as resetPosition rotation
	/// </summary>
	public void CheckRotationStatus()
	{
		if(pcCamera.transform.rotation == resetPos.rotation)
		{
			resetedRotX = true;
		}
		if (pcCamera.transform.rotation != resetPos.rotation)
		{
			resetedRotX = false;
		}
	}

	/// <summary>
	/// Slerptime
	/// </summary>
	public void StartSlerping()
	{

		time = slerpTime * Time.deltaTime;
	}

	/// <summary>
	/// Checks if Mouse is positioned on screen edges and moves camera
	/// </summary>
	public void MouseOnEdge()
	{
		//if (Time.time > timer)
		//{
		//	timer = Time.time + 1;
		//	Debug.Log(mousePos);

		if (mousePos.x < leftBounds && mousePos.x >= 0)
		{
			//Debug.Log("MOVE CAMERA LEFT");
			transform.Rotate(Vector3.up, -mouseRotateSpeedY * Time.deltaTime);

		}
		if (mousePos.x > rightBounds && mousePos.x <= 1)
		{
			//Debug.Log("MOVE CAMERA RIGHT");
			transform.Rotate(Vector3.up, mouseRotateSpeedY * Time.deltaTime);
		}
		if (mousePos.y > upBounds && mousePos.y <= 1)
		{
			//Debug.Log("MOVE CAMERA UP");
			pcCamera.transform.Rotate(Vector3.left, mouseRotateSpeedX * Time.deltaTime);
			rotating = true;
						
		}		
		if (mousePos.y < downBounds && mousePos.y >= 0)
		{
			//Debug.Log("MOVE CAMERA DOWN");
			pcCamera.transform.Rotate(Vector3.right, mouseRotateSpeedX * Time.deltaTime);
			rotating = true;
			
		}

		if (mousePos.y < upBounds && mousePos.y > downBounds)
		{
			
			if (rotating)
			{
				Debug.Log("Rotating Ended");	
				rotating = false;
				StartCoroutine(ResetLookAxis());
				//pcCamera.transform.rotation = resetPos.rotation;
			}
		}
		//}

	}

	/// <summary>
	/// Rotational movement
	/// </summary>
	public void RotationalMoveTwo()
	{

		if (lockRotation)
		{

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

	/// <summary>
	/// Resets the x axis rotation on camera slowly
	/// </summary>
	/// <returns></returns>
	public IEnumerator ResetLookAxis()
	{
		
		while ((!resetedRotX && !rotating))
		{
			//Debug.Log("STARTED COROUTINE");
			//time += Time.deltaTime;
			//Debug.Log(pcCamera.transform.rotation.x);

			pcCamera.transform.rotation = Quaternion.Slerp(pcCamera.transform.rotation, resetPos.rotation, time);
	
			yield return null;
		}

	}
	#endregion

	#region Input values
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

	public void TurnInput(float turnX)
	{
		this.turnX = turnX;
	}

	public void MousePosition(Vector3 mousePosition)
	{
		mousePos = mousePosition / new Vector2(Screen.width, Screen.height);
	}
	#endregion
}
