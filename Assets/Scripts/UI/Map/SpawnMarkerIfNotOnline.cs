using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Spawns a player marker if the player is not in multiplayer mode.
 * </summary>
 */
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

	/**
	 * <summary>
	 * Checks wheter or not any of the loaded scenes match <see cref="multiplayerScene"/>.
	 * </summary>
	 */
	private bool CheckIfInMultiplayer()
	{
		return SceneExtensions.GetAllLoadedScenes().Any(x => x.name == multiplayerScene);
	}
}
