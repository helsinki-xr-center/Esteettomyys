using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnMarkerIfNotOnline : MonoBehaviour
{
	public string multiplayerScene = "Multiplayer";
	public Transform mapImageTransfrom;
	public GameObject playerMarkerPrefab;
	private GameObject spawnedMarker;

	private void OnEnable()
	{
		if (!CheckIfInMultiplayer())
		{
			var player = FindObjectOfType<PlayerPosition>();
			if (player != null)
			{
				spawnedMarker = Instantiate(playerMarkerPrefab, mapImageTransfrom);
				var marker = spawnedMarker.GetComponent<MapLocationMarker>();
				marker.SetValues(player.transform);
			}
		}
	}

	private void OnDisable()
	{
		if (spawnedMarker)
		{
			Destroy(spawnedMarker);
		}
	}

	private bool CheckIfInMultiplayer()
	{
		return SceneExtensions.GetAllLoadedScenes().Any(x => x.name == multiplayerScene);
	}
}
