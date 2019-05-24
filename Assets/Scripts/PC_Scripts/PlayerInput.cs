using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @Author = Veli-Matti Vuoti
/// 
/// </summary>
public class PlayerInput : MonoBehaviour
{

	PcPlayer player;

	[Range(0, 100)] public float verticalHeadMoveSpeed;
	[Range(0, 90)] public float verticalHeadAngleLimit;

	private void Awake()
	{
		player = gameObject.GetComponent<PcPlayer>();
	}

	void Update()
	{
		if (Input.GetAxis("Horizontal") != 0) { float horizontalMove = Input.GetAxis("Horizontal"); player.movement.MoveHorizontal(horizontalMove); }
		if (Input.GetAxis("Vertical") != 0) { float verticalMove = Input.GetAxis("Vertical"); player.movement.MoveVertical(verticalMove); }
		if (Input.GetAxis("Mouse X") != 0 && Input.GetAxis("Fire2") != 0) { float mouseX = Input.GetAxis("Mouse X"); player.movement.TurnHorizontal(mouseX); }
		float mouseYclamped = Mathf.Clamp(Input.GetAxis("Mouse Y") * verticalHeadMoveSpeed * Time.deltaTime, -verticalHeadAngleLimit, verticalHeadAngleLimit);
		if (Input.GetAxis("Mouse Y") != 0) { player.movement.MoveHeadVertical(mouseYclamped); }
	}

}
