using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverTabletControl : MonoBehaviour
{

	PlayerPosition playerPosition;
	Vector3 backPosition;
	Vector3 frontPosition;

	public float distanceToFollowPlayer;
	public float distance;
	public float speed;
	public float time;

	public bool following;

	private void Start()
	{
		playerPosition = FindObjectOfType<PlayerPosition>();

	}

	private void Update()
	{
		Debug.DrawRay(transform.position, GetBackPosition(), Color.red, 0.1f);
		Debug.DrawRay(transform.position, GetFrontPosition(), Color.blue, 0.1f);

		if (following)
		{
			transform.position = Vector3.Lerp(transform.position, GetBackPosition(), Time.deltaTime * speed);
		}
		else
		{
			transform.position = Vector3.Lerp(transform.position, GetFrontPosition(), Time.deltaTime * speed);
		}



		transform.LookAt(new Vector3(playerPosition.transform.position.x, playerPosition.transform.position.y + playerPosition.GetHeadHeightFromBase(), playerPosition.transform.position.z));

	}

	public Vector3 GetBackPosition()
	{
		Vector3 direction = playerPosition.GetRotation() * Vector3.forward;
		float headHeight = playerPosition.GetHeadHeightFromBase();
		backPosition = playerPosition.GetPosition() + new Vector3(direction.x * distanceToFollowPlayer, headHeight /2, direction.z * distanceToFollowPlayer);
		return backPosition;
	}

	public Vector3 GetFrontPosition()
	{
		Vector3 direction = playerPosition.GetRotation() * -Vector3.forward;
		float headHeight = playerPosition.GetHeadHeightFromBase();
		frontPosition = playerPosition.GetPosition() + new Vector3(direction.x * distanceToFollowPlayer, headHeight/2, direction.z * distanceToFollowPlayer);
		return frontPosition;
	}
}
