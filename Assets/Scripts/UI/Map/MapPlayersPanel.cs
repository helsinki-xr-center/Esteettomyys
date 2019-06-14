using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * UI panel script for handling the players panel in map UI.
 * </summary>
 */
public class MapPlayersPanel : MonoBehaviour
{
	public string multiplayerScene = "Multiplayer";
	public GameObject playerListPrefab;
	public Transform listParent;
	public Transform mapImage;

	private Mapper mapper;

	private void OnEnable()
	{
		mapper = FindObjectOfType<Mapper>();
		ClearList();
		PopulateList();
	}

	private void Awake()
	{
		SceneManager.sceneLoaded += SceneLoadedCallback;
		SceneManager.sceneUnloaded += SceneUnloadedCallback;
	}

	private void Start()
	{
		CheckIfInMultiplayer();
	}

	private void SceneLoadedCallback(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == multiplayerScene)
		{
			gameObject.SetActive(true);
		}
	}

	private void SceneUnloadedCallback(Scene scene)
	{
		if (scene.name == multiplayerScene)
		{
			gameObject.SetActive(false);
		}
	}

	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= SceneLoadedCallback;
		SceneManager.sceneUnloaded -= SceneUnloadedCallback;
	}

	/**
	 * <summary>
	 * Checks if Multiplayer scene is loaded and sets this panel active accordingly.
	 * </summary>
	 */
	private void CheckIfInMultiplayer()
	{
		if (SceneExtensions.GetAllLoadedScenes().Any(x => x.name == multiplayerScene))
		{
			gameObject.SetActive(true);
		}
		else
		{
			gameObject.SetActive(false);
		}
	}

	/**
	 * <summary>
	 * Populates the list with instantiated MapPlayerListItems, and sets their values.
	 * </summary>
	 */
	private void PopulateList()
	{
		if (mapper == null)
		{
			return;
		}

		foreach (var player in FindObjectsOfType<AvatarFollowPlayer>())
		{
			GameObject listItem = Instantiate(playerListPrefab, listParent);
			listItem.GetComponent<MapPlayerListItem>().SetValues(player, mapImage);
		}
	}

	/**
	 * <summary>
	 * Clears the current list.
	 * </summary>
	 */
	private void ClearList()
	{
		foreach (Transform child in listParent)
		{
			Destroy(child.gameObject);
		}
	}
}
