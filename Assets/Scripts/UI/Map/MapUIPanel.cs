using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MapUIPanel : MonoBehaviour
{
	public TextMeshProUGUI mapButtonText;
	public string worldMapText;
	public string areaMapText;


	private Mapper mapper;
	private bool worldMap = true;


    // Start is called before the first frame update
    void Start()
    {
		mapper = FindObjectOfType<Mapper>();
		if(mapper != null)
		{
			SwitchToWorldMap();
		}
    }

	
	public void OnSwitchMapButtonClick()
	{
		if(worldMap)
		{
			SwitchToAreaMap();
		}
		else
		{
			SwitchToWorldMap();
		}
	}

	public void SwitchToWorldMap()
	{
		mapper.RecalculateBounds();
		worldMap = true;
		mapButtonText.SetText(areaMapText);
	}

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
