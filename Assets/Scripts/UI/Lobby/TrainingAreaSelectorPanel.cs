using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Spawns <see cref="AreaUiItem"/> for each area in the list. 
 * These are used to select the starting point in the map.
 * </summary>
 */
public class TrainingAreaSelectorPanel : MonoBehaviour
{
	public List<AreaScriptableObject> areas;
	public GameObject areaUIItemPrefab;
	public Transform listParent;

	public void Start()
	{
		foreach(var area in areas)
		{
			GameObject go = Instantiate(areaUIItemPrefab, listParent ?? transform);
			var script = go.GetComponent<AreaUiItem>();
			script.SetValues(area, area.displayName, 0, 5, area.previewSprite, true);

			if (areas.IndexOf(area) == 0)
			{
				script.Select();
			}
		}

	}
}
