using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * One of these should be in every separate area scene.
 * </summary>
 */
public class AreaScript : MonoBehaviour
{
	public AreaScriptableObject area;
	public bool drawDebugBounds = true;
	public bool playerInBounds = false;
	public SpawnLocation[] spawnLocations;

	private Bounds bounds;
	private PlayerPosition player;
	private bool detailsLoaded = false;

	private void Awake()
	{
		if(area == null)
		{
			Debug.LogError($"{nameof(area)} should not be null!", this);
		}
	}

	IEnumerator Start()
    {
		CalculateBounds();

		player = FindObjectOfType<PlayerPosition>();
		yield return new WaitForSeconds(0.1f);

		if (player != null && spawnLocations != null && GlobalValues.startingArea == area)
		{
			var spawn = spawnLocations.FirstOrDefault(x => x.IsFree());
			if(spawn != null)
			{
				new TeleportMessage(spawn.transform.position).Deliver();
			}
		}
    }

	private void Update()
	{
		if (drawDebugBounds)
		{
			bounds.DrawDebug();
		}

		if(player == null)
		{
			player = FindObjectOfType<PlayerPosition>();
			return;
		}

		playerInBounds = bounds.Contains(player.transform.position);

		if (!string.IsNullOrEmpty(area.detailsScene))
		{

			if (playerInBounds && !detailsLoaded)
			{
				SceneLoaderAsync.instance.LoadSceneAsync(area.detailsScene);
				detailsLoaded = true;
			}
			else if(!playerInBounds && detailsLoaded)
			{
				SceneLoaderAsync.instance.UnloadSceneAsync(area.detailsScene);
				detailsLoaded = false;
			}
		}
	}

	/**
	 * <summary>
	 * Calculates the bounds of this Area by aggregating all the Renderer bounds in this scene.
	 * </summary>
	 */
	private void CalculateBounds()
	{
		// linq query to find all Renderer bounds in all objects in this scene
		var roots = gameObject.scene.GetRootGameObjects().Select(x => x.transform);
		var visible = roots.SelectMany(x => x.EnumerateChildrenRecursive()).Concat(roots);
		var renderers = visible.Select(x => x.GetComponent<Renderer>()).Where(x => x != null);
		var allBounds = renderers.Select(x => x.bounds);

		if (allBounds.Count() == 0)
		{
			bounds = new Bounds(transform.position, Vector3.one * 10);
		}
		else
		{
			// encapsulate all bounds into a single bounds
			bounds = allBounds.Aggregate((combined, next) => { combined.Encapsulate(next); return combined; });
		}
	}

	private void OnDestroy()
	{
		if(detailsLoaded)
		{
			SceneLoaderAsync.instance.UnloadSceneAsync(area.detailsScene);
		}
	}

	private void OnValidate()
	{
		spawnLocations = FindObjectsOfType<SpawnLocation>().Where(x => x.gameObject.scene == gameObject.scene).ToArray();
	}

	/**
	 * <summary>
	 * Returns whether the player is inside the bounds of this area
	 * </summary>
	 */
	public bool IsPlayerInBounds()
	{
		return playerInBounds;
	}

	/**
	 * <summary>
	 * Gets the bounds of this area.
	 * </summary>
	 */
	public Bounds GetBounds()
	{
		return bounds;
	}
}
