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
		Settings.OnSettingsChanged += CheckSettings;
	}

	private void OnDisable()
	{
		Settings.OnSettingsChanged -= CheckSettings;
	}

	public void CheckSettings(Settings settings)
	{
		
		OnWheelChairModeEnabled();
		
	}

	/// <summary>
	/// Function gets player head height from playerPosition class and scales the height to wheelchairheight
	/// Need some improvements
	/// </summary>
	/// <param name="status">on/off</param>
	public void OnWheelChairModeEnabled()
	{

		float headHeight = playerPosition.GetHeadHeightFromBase();

		if (GlobalValues.settings.wheelChairMode)
		{
		
			transform.localScale = new Vector3(transform.localScale.x, GlobalValues.settings.wheelChairHeight/headHeight, transform.localScale.z);
		
		}
		else
		{
		
			transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
			
		}
	}
}
