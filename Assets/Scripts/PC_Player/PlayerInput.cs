using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @Author = Veli-Matti Vuoti
/// 
/// Checks the PC player input
/// </summary>
public class PlayerInput : MonoBehaviour
{

	PcPlayer player;
	Vector3 mousePosition;
	float horizontalMove;
	float verticalMove;
	float turnX;
	float mouseX;
	float mouseY;

	private void Start()
	{
		player = gameObject.GetComponent<PcPlayer>();
	}

	void Update()
	{

		switch (player.movement.pCCS)
		{
			case PCControlSet.First:

				horizontalMove = Input.GetAxis("Horizontal");
				verticalMove = Input.GetAxis("Vertical");
				mouseX = Input.GetAxis("Mouse X");
				mouseY = Input.GetAxis("Mouse Y");

				player.movement.DirectionalInput(horizontalMove, verticalMove);

				player.movement.RotationalInput(mouseX, mouseY);

				if (Input.GetButton("Fire3"))
				{
					player.movement.lockRotation = true;
				}

				if (Input.GetButtonUp("Fire3"))
				{
					player.movement.lockRotation = false;
				}

				if (Input.GetMouseButtonDown(1))
				{
					player.PortalIndicator();
				}
				if (Input.GetMouseButtonUp(1))
				{
					player.PCTeleport();
				}

				if (Input.GetButtonDown("Cancel"))
				{
					player.InteractionMenu();
				}

				break;
			case PCControlSet.Second:

				horizontalMove = Input.GetAxis("HorizontalTwo");
				verticalMove = Input.GetAxis("Vertical");
				turnX = Input.GetAxis("Horizontal");
				mouseX = Input.GetAxis("Mouse X");
				mouseY = Input.GetAxis("Mouse Y");
				mousePosition = Input.mousePosition;

				player.movement.DirectionalInput(horizontalMove, verticalMove);
				player.movement.TurnInput(turnX);
				player.movement.RotationalInput(mouseX, mouseY);
				player.movement.MousePosition(mousePosition);

				if (Input.GetButtonDown("Fire3"))
				{
					player.movement.ResetRotationX();
					StopCoroutine(player.movement.ResetLookAxis());
				}

				if (Input.GetButton("Fire3"))
				{
					player.movement.lockRotation = true;
					//StopCoroutine(player.movement.ResetLookAxis());
				}

				if (Input.GetButtonUp("Fire3"))
				{
					player.movement.lockRotation = false;
					//player.movement.resetedRotX = false;
					player.movement.StartSlerping();
					StartCoroutine(player.movement.ResetLookAxis());
				}


				if (Input.GetMouseButton(1))
				{

					player.PortalIndicator();

				}
				if (Input.GetMouseButtonUp(1))
				{

					player.PCTeleport();
					player.teleportIndicator.SetActive(false);

				}


				if (Input.GetButtonDown("Cancel"))
				{
					player.InteractionMenu();
				}

				break;
			default:
				break;
		}



	}
}
