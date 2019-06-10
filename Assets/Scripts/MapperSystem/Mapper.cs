using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * A script for rendering the map. Calculates the bounds of every visible object in the map and fits the orthgraphic view to the bounds.
 * </summary>
 */
[RequireComponent(typeof(Camera))]
public class Mapper : MonoBehaviour
{
	public int renderTextureResolution = 512;
	public Material replaceMaterial = null;

	private new Camera camera;
	private RenderTexture texture;
	private CullingGroup cullingGroup;
	private Bounds bounds;
	private ReplaceMaterialOnCameraRender[] replaceMaterials;

	private void Awake()
	{
		camera = GetComponent<Camera>();
		texture = new RenderTexture(renderTextureResolution, renderTextureResolution, 24);

		camera.targetTexture = texture;
		camera.enabled = false;
	}

	private void Start()
	{
		CalculateBoundsAndAddReplaceMaterials();
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += SceneLoadedCallback;
		SceneManager.sceneUnloaded += SceneUnloadedCallback;
		CalculateBoundsAndAddReplaceMaterials();
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= SceneLoadedCallback;
		SceneManager.sceneUnloaded -= SceneUnloadedCallback;
	}

	/**
	 * <summary>
	 * Renders the camera view. The camera is not rendered automatically so this should be called when a new view is required from the map.
	 * </summary>
	 */
	public void RenderFrame()
	{
		if (replaceMaterial != null)
		{
			foreach (var mat in replaceMaterials)
			{
				mat.ReplaceMaterial();
			}
		}

		camera.Render();

		if (replaceMaterial != null)
		{
			foreach (var mat in replaceMaterials)
			{
				mat.ResetMaterial();
			}
		}
	}

	/**
	 * <summary>
	 * Gets the render texture that the map will be rendered to.
	 * </summary>
	 */
	public RenderTexture GetTexture()
	{
		return texture;
	}

	private void Update()
	{
		DebugDrawBounds(bounds);
	}

	/**
	 * <summary>
	 * Transfroms a world position in XZ to a map coordinate XY in range of [0-1, 0-1].
	 * </summary>
	 */
	public Vector2 XZWorldToMapPositionXY(Vector3 worldPos)
	{

		float y = Mathf.InverseLerp(camera.transform.position.z - camera.orthographicSize, camera.transform.position.z + camera.orthographicSize, worldPos.z);
		float x = Mathf.InverseLerp(camera.transform.position.x - camera.orthographicSize, camera.transform.position.x + camera.orthographicSize, worldPos.x);

		return new Vector2(x, y);
	}


	/**
	 * <summary>
	 * Calculates the combined bounds of every visible object in currently loaded scenes. Also adds <see cref="ReplaceMaterialOnCameraRender"/> script to all visible Renderers.
	 * </summary>
	 */
	private void CalculateBoundsAndAddReplaceMaterials()
	{
		int includeLayers = camera.cullingMask;

		// linq query to find all Renderer bounds in all visible objects for the camera
		var scenes = SceneExtensions.GetAllLoadedScenes();
		var roots = scenes.SelectMany(x => x.GetRootGameObjects()).Select(x => x.transform);
		var visible = roots.SelectMany(x => x.EnumerateChildrenRecursive()).Where(x => ((1 << x.gameObject.layer) & includeLayers) != 0);
		var renderers = visible.Select(x => x.GetComponent<Renderer>()).Where(x => x != null);
		var allBounds = renderers.Select(x => x.bounds);

		if (allBounds.Count() == 0)
		{
			bounds = new Bounds(Vector3.zero, Vector3.one * 10);
		}
		else
		{
			// encapsulate all bounds into a single bounds
			bounds = allBounds.Aggregate((combined, next) => { combined.Encapsulate(next); return combined; });
		}

		var missingRenderers = renderers.Where(x => x.GetComponent<ReplaceMaterialOnCameraRender>() == null);
		foreach (var r in missingRenderers)
		{
			var replacer = r.gameObject.AddComponent<ReplaceMaterialOnCameraRender>();
			replacer.targetCamera = camera;
			replacer.mapper = this;
		}

		replaceMaterials = renderers.Select(x => x.GetComponent<ReplaceMaterialOnCameraRender>()).ToArray();

		float centerX = (bounds.min.x + bounds.max.x) / 2;
		float centerZ = (bounds.min.z + bounds.max.z) / 2;
		float maxY = bounds.max.y;
		float lengthY = (bounds.max.y - bounds.min.y);
		float yAdd = 2;

		float maxWidth = Mathf.Max((bounds.max.x - bounds.min.x), (bounds.max.z - bounds.min.z));
		camera.transform.position = new Vector3(centerX, maxY + yAdd, centerZ);
		camera.orthographicSize = maxWidth / 2;
		camera.farClipPlane = lengthY * 2;
	}

	/**
	 * <summary>
	 * Draws bounds Editor window with lines.
	 * </summary>
	 */
	private void DebugDrawBounds(Bounds b, float delay = 0)
	{
		// bottom
		var p1 = new Vector3(b.min.x, b.min.y, b.min.z);
		var p2 = new Vector3(b.max.x, b.min.y, b.min.z);
		var p3 = new Vector3(b.max.x, b.min.y, b.max.z);
		var p4 = new Vector3(b.min.x, b.min.y, b.max.z);

		Debug.DrawLine(p1, p2, Color.blue, delay);
		Debug.DrawLine(p2, p3, Color.red, delay);
		Debug.DrawLine(p3, p4, Color.yellow, delay);
		Debug.DrawLine(p4, p1, Color.magenta, delay);

		// top
		var p5 = new Vector3(b.min.x, b.max.y, b.min.z);
		var p6 = new Vector3(b.max.x, b.max.y, b.min.z);
		var p7 = new Vector3(b.max.x, b.max.y, b.max.z);
		var p8 = new Vector3(b.min.x, b.max.y, b.max.z);

		Debug.DrawLine(p5, p6, Color.blue, delay);
		Debug.DrawLine(p6, p7, Color.red, delay);
		Debug.DrawLine(p7, p8, Color.yellow, delay);
		Debug.DrawLine(p8, p5, Color.magenta, delay);

		// sides
		Debug.DrawLine(p1, p5, Color.white, delay);
		Debug.DrawLine(p2, p6, Color.gray, delay);
		Debug.DrawLine(p3, p7, Color.green, delay);
		Debug.DrawLine(p4, p8, Color.cyan, delay);
	}

	/**
	 * <summary>
	 * Draws the desired texture directly to the map texture.
	 * </summary>
	 */
	private void DrawTexture(Vector2 position, Vector2 scale, Texture drawTexture)
	{
		RenderTexture.active = texture;
		GL.PushMatrix(); //Saves projection and modelview matrices.

		GL.LoadPixelMatrix(0, texture.width, texture.height, 0);
		Vector2 pixelScale = Vector2.Scale(scale, new Vector2(texture.width, texture.height));
		Vector2 pixelPos = Vector2.Scale(position, new Vector2(texture.width, texture.height));
		Graphics.DrawTexture(new Rect(pixelPos.x - pixelScale.x / 2, pixelPos.y - pixelScale.y / 2, pixelScale.x, pixelScale.y), drawTexture);

		GL.PopMatrix(); //Restores projection and modelview matrices.
		RenderTexture.active = null;
	}

	private void SceneLoadedCallback(Scene s, LoadSceneMode mode)
	{
		Invoke("CalculateBoundsAndAddReplaceMaterials", 1);
	}

	private void SceneUnloadedCallback(Scene s)
	{
		Invoke("CalculateBoundsAndAddReplaceMaterials", 1);
	}
}
