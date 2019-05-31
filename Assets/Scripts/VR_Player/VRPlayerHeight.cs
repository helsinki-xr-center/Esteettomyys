using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Class handles player height scaling for wheelchair mode.
/// </summary>
public class VRPlayerHeight : MonoBehaviour
{

	[SerializeField] private Camera cam;
	PlayerPosition playerPosition;

	private void Start()
	{
		playerPosition = FindObjectOfType<PlayerPosition>();
		if (cam == null)
		{
			cam = gameObject.GetComponent<Camera>();
		}

		//Resize();
	}

	private void OnEnable()
	{
		OptionsTab.ChangeHeightEvent += OnWheelChairModeEnabled;
	}

	private void OnDisable()
	{
		OptionsTab.ChangeHeightEvent -= OnWheelChairModeEnabled;
	}

	/// <summary>
	/// Function gets player head height from playerPosition class and scales the height to wheelchairheight
	/// Need some improvements
	/// </summary>
	/// <param name="status">on/off</param>
	public void OnWheelChairModeEnabled(bool status, float currentHeight, float targetHeight)
	{

		float headHeight = playerPosition.GetHeadHeightFromBase();

		if (!GlobalValues.settings.wheelChairMode)
		{

			GlobalValues.settings.wheelChairMode = true;
			transform.localScale = new Vector3(transform.localScale.x, targetHeight/headHeight, transform.localScale.z);
			
		}
		else
		{
			GlobalValues.settings.wheelChairMode = false;
			transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
			
		}
	}
}
