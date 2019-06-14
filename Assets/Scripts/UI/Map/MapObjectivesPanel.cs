using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectivesPanel : MonoBehaviour
{
	public GameObject objectiveListPrefab;
	public Transform listParent;

	private Mapper mapper;

	private void OnEnable()
	{
		mapper = FindObjectOfType<Mapper>();
		ClearList();
		PopulateList();
	}

	private void PopulateList()
	{
		if (mapper == null)
		{
			return;
		}

		//TODO: spawn objectives when is posible
	}

	private void ClearList()
	{
		foreach (Transform child in listParent)
		{
			Destroy(child.gameObject);
		}
	}
}
