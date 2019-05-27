using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(PostProcessVolume))]
public class ColorblindModeSimulator : MonoBehaviour
{

	public PostProcessVolume volume;

	[Header("Different profiles")]
	public PostProcessProfile normal;
	public PostProcessProfile deuteranopa;
	public PostProcessProfile tritanopia;
	public PostProcessProfile monochromacy;

	public ColorblindMode currentMode;

	void Start()
	{
		volume = GetComponent<PostProcessVolume>();

		volume.profile = normal;
		currentMode = ColorblindMode.Normal;
	}


	public void SetColorblindMode(ColorblindMode mode)
	{
		switch (mode)
		{
			case ColorblindMode.Normal:
				volume.profile = normal;
				break;
			case ColorblindMode.Deuteranopa:
				volume.profile = deuteranopa;
				break;
			case ColorblindMode.Tritanopia:
				volume.profile = tritanopia;
				break;
			case ColorblindMode.Monochromacy:
				volume.profile = monochromacy;
				break;
			default:
				break;
		}

	}
}
