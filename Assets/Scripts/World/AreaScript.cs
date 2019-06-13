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
	[Scene]
	public string detailsScene;
	public bool drawDebugBounds = true;
	public bool playerInBounds = false;

	private Bounds bounds;
	private PlayerPosition player;
	private bool detailsLoaded = false;

    // Start is called before the first frame update
    void Start()
    {
		player = FindObjectOfType<PlayerPosition>();
		CalculateBounds();
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

		if (!string.IsNullOrEmpty(detailsScene))
		{

			if (playerInBounds && !detailsLoaded)
			{
				SceneLoaderAsync.instance.LoadSceneAsync(detailsScene);
				detailsLoaded = true;
			}
			else if(!playerInBounds && detailsLoaded)
			{
				SceneLoaderAsync.instance.UnloadSceneAsync(detailsScene);
				detailsLoaded = false;
			}
		}
	}

	private void CalculateBounds()
	{
		// linq query to find all Renderer bounds in all objects in this scene
		var roots = gameObject.scene.GetRootGameObjects().Select(x => x.transform);
		var visible = roots.SelectMany(x => x.EnumerateChildrenRecursive());
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
			SceneLoaderAsync.instance.UnloadSceneAsync(detailsScene);
		}
	}
}
