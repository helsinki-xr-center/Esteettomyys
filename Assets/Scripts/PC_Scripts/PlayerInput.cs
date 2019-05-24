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
		float horizontalMove = Input.GetAxis("Horizontal");
		float verticalMove = Input.GetAxis("Vertical");
		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = Input.GetAxis("Mouse Y");

		player.movement.DirectionalMove(horizontalMove, verticalMove); 
		player.movement.RotationalMove(mouseX, mouseY); 

	}

}
