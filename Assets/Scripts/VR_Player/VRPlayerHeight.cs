using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayerHeight : MonoBehaviour
{

	[SerializeField]
	private float defaultHeight = 1.8f;
	[SerializeField]
	private Camera cam;
	PlayerPosition playerPosition;

	private void Start()
	{
		playerPosition = FindObjectOfType<PlayerPosition>();
		if ( cam == null)
		{
			cam = gameObject.GetComponent<Camera>();
		}

		//Resize();
	}

	private void Resize()
	{
		float headHeight = playerPosition.GetHeadHeightFromBase();
		float scale = defaultHeight / headHeight;
		transform.localScale = Vector3.one * scale;
	}

	public void SetDefaultHeight( float height )
	{
		defaultHeight = height;
	}

}
