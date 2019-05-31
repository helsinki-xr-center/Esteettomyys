using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * UI script for eyesight mode selector panel.
 * </summary>
 */
public class EyesightSelectorPanel : MonoBehaviour
{
	NearSightedAdjuster sightAdjuster;

	void Start()
	{
		sightAdjuster = FindObjectOfType<NearSightedAdjuster>();
	}

	/**
	 * <summary>
	 * Unity UI callback for normal mode button.
	 * </summary>
	 */
	public void OnNormalButtonClick()
	{
		if (sightAdjuster == null)
		{
			sightAdjuster = FindObjectOfType<NearSightedAdjuster>();
		}
		sightAdjuster.SetSightMode(EyesightMode.Normal);
	}

	/**
	 * <summary>
	 * Unity UI callback for near sighted mode button.
	 * </summary>
	 */
	public void OnNearSightedButtonClick()
	{
		if (sightAdjuster == null)
		{
			sightAdjuster = FindObjectOfType<NearSightedAdjuster>();
		}
		sightAdjuster.SetSightMode(EyesightMode.NearSighted);
	}

	/**
	 * <summary>
	 * Unity UI callback for far sighted mode button.
	 * </summary>
	 */
	public void OnFarSightedButtonClick()
	{
		if (sightAdjuster == null)
		{
			sightAdjuster = FindObjectOfType<NearSightedAdjuster>();
		}
		sightAdjuster.SetSightMode(EyesightMode.FarSighted);
	}

	/**
	 * <summary>
	 * Unity UI callback for bad mode button.
	 * </summary>
	 */
	public void OnBadSightedButtonClick()
	{
		if (sightAdjuster == null)
		{
			sightAdjuster = FindObjectOfType<NearSightedAdjuster>();
		}
		sightAdjuster.SetSightMode(EyesightMode.Bad);
	}
}
