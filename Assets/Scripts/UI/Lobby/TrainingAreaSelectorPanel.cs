using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
