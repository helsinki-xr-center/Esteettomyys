using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Sets a postprocessing profile for different modes of colorblindness when needed.
 * </summary>
 */
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

	/**
	 * <summary>
	 * Set the current colorblind mode to the specified mode.
	 * </summary>
	 */
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
