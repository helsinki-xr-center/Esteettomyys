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

	private void Awake()
	{
		player = gameObject.GetComponent<PcPlayer>();
	}

	void Update()
	{
		if (Input.GetAxis("Horizontal") != 0) { float horizontalMove = Input.GetAxis("Horizontal"); player.movement.MoveHorizontal(horizontalMove); }
		if (Input.GetAxis("Vertical") != 0) { float verticalMove = Input.GetAxis("Vertical"); player.movement.MoveVertical(verticalMove); }
		if (Input.GetAxis("Mouse X") != 0 && Input.GetAxis("Fire2") != 0) { float mouseX = Input.GetAxis("Mouse X"); player.movement.TurnHorizontal(mouseX); }
		if (Input.GetAxis("Mouse Y") != 0) { float mouseY = Input.GetAxis("Mouse Y"); player.movement.MoveHeadVertical(mouseY); }
	}

}
