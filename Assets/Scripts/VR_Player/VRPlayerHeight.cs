using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Class handles player height scaling for wheelchair mode.
/// </summary>
public class VRPlayerHeight : MonoBehaviour
{

	[SerializeField] private float defaultHeight = 1.8f;
	[SerializeField] private float wheelChairHeight = 0.8f;
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
		OptionsTab.WheelChairModeEvent += OnWheelChairModeEnabled;
	}

	private void OnDisable()
	{
		OptionsTab.WheelChairModeEvent -= OnWheelChairModeEnabled;
	}

	/// <summary>
	/// Scales size can make player look like giant 
	/// </summary>
	private void Resize()
	{
		float headHeight = playerPosition.GetHeadHeightFromBase();
		float scale = defaultHeight / headHeight;
		transform.localScale = Vector3.one * scale;
	}

	/// <summary>
	/// if default height changes
	/// </summary>
	/// <param name="height"> quessed player height </param>
	public void SetDefaultHeight(float height)
	{
		defaultHeight = height;
	}

	/// <summary>
	/// Function gets player head height from playerPosition class and scales the height to wheelchairheight
	/// Need some improvements
	/// </summary>
	/// <param name="status">on/off</param>
	public void OnWheelChairModeEnabled(bool status)
	{

		float headHeight = playerPosition.GetHeadHeightFromBase();

		if (!GlobalValues.settings.wheelChairMode)
		{

			GlobalValues.settings.wheelChairMode = true;
			transform.localScale = new Vector3(transform.localScale.x, wheelChairHeight/headHeight, transform.localScale.z);
			
		}
		else
		{
			GlobalValues.settings.wheelChairMode = false;
			transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
			
		}
	}
}
