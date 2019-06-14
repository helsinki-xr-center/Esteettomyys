using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * UI script attached to the Map UI. Handles switching between World and Area map.
 * </summary>
 */
public class MapUIPanel : MonoBehaviour
{
	public TextMeshProUGUI mapButtonText;
	public string worldMapText;
	public string areaMapText;


	private Mapper mapper;
	private bool worldMap = true;


	private void Start()
	{
		mapper = FindObjectOfType<Mapper>();
		if (mapper != null)
		{
			SwitchToWorldMap();
		}
	}

	/**
	 * <summary>
	 * Called from unity UI button. Switches between World and Area maps.
	 * </summary>
	 */
	public void OnSwitchMapButtonClick()
	{
		if (worldMap)
		{
			SwitchToAreaMap();
		}
		else
		{
			SwitchToWorldMap();
		}
	}

	/**
	 * <summary>
	 * Makes <see cref="Mapper"/> recalculate the whole bounds of the area.
	 * </summary>
	 */
	public void SwitchToWorldMap()
	{
		mapper.RecalculateBounds();
		worldMap = true;
		mapButtonText.SetText(areaMapText);
	}

	/**
	 * <summary>
	 * Finds the Area where player is in, and sets <see cref="Mapper"/> bounds to be the bounds of that area.
	 * </summary>
	 */
	public void SwitchToAreaMap()
	{
		var playerArea = FindObjectsOfType<AreaScript>().FirstOrDefault(x => x.IsPlayerInBounds());
		if (playerArea != null)
		{
			mapper.SetCustomBounds(playerArea.GetBounds());
			worldMap = false;
			mapButtonText.SetText(worldMapText);
		}
	}
}
