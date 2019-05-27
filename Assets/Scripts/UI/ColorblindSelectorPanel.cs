using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class ColorblindSelectorPanel : MonoBehaviour
{

	ColorblindModeSimulator simulator;

	// Start is called before the first frame update
	void Start()
	{
		simulator = FindObjectOfType<ColorblindModeSimulator>();
	}

	public void OnNormalButtonClick()
	{
		if (simulator == null)
		{
			simulator = FindObjectOfType<ColorblindModeSimulator>();
		}
		simulator.SetColorblindMode(ColorblindMode.Normal);
	}

	public void OnDeuteranopiaButtonClick()
	{
		if (simulator == null)
		{
			simulator = FindObjectOfType<ColorblindModeSimulator>();
		}
		simulator.SetColorblindMode(ColorblindMode.Deuteranopa);
	}

	public void OnTritanopiaButtonClick()
	{
		if (simulator == null)
		{
			simulator = FindObjectOfType<ColorblindModeSimulator>();
		}
		simulator.SetColorblindMode(ColorblindMode.Tritanopia);
	}

	public void OnMonochromacyButtonClick()
	{
		if (simulator == null)
		{
			simulator = FindObjectOfType<ColorblindModeSimulator>();
		}
		simulator.SetColorblindMode(ColorblindMode.Monochromacy);
	}
}
