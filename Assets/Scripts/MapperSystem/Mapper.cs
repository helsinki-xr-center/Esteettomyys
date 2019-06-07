using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Camera))]
public class Mapper : MonoBehaviour
{
	public int renderTextureResolution = 512;
	public Texture playerTexture;
	public Texture otherPlayerTexture;

	public Transform playerTransform;
	public List<Transform> otherPlayerTransforms = new List<Transform>();

	private new Camera camera;
	private RenderTexture texture;
	private CullingGroup cullingGroup;
	private Bounds bounds;

	private void Awake()
	{
		camera = GetComponent<Camera>();
		texture = new RenderTexture(renderTextureResolution, renderTextureResolution, 24);

		camera.targetTexture = texture;
		camera.enabled = false;
	}

	private void Start()
	{
		CalculateBounds();
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += SceneLoadedCallback;
		SceneManager.sceneUnloaded += SceneUnloadedCallback;
		CalculateBounds();

	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= SceneLoadedCallback;
		SceneManager.sceneUnloaded -= SceneUnloadedCallback;
	}

	public void RenderFrame()
	{
		camera.Render();

		if (playerTransform != null)
		{
			Vector2 pos = XZWorldToMapPositionXY(playerTransform.position);
			DrawTexture(pos, new Vector2(0.05f, 0.05f), playerTexture);
		}

		foreach (var otherTransform in otherPlayerTransforms)
		{
			if (otherTransform == null)
				continue;

			Vector2 pos = XZWorldToMapPositionXY(otherTransform.position);
			DrawTexture(pos, new Vector2(0.05f, 0.05f), otherPlayerTexture);
		}
	}

	public RenderTexture GetTexture()
	{
		return texture;
	}

	private void Update()
	{
		DebugDrawBounds(bounds);
	}

	public Vector2 XZWorldToMapPositionXY(Vector3 worldPos)
	{

		float y = 1 - Mathf.InverseLerp(camera.transform.position.z - camera.orthographicSize, camera.transform.position.z + camera.orthographicSize, worldPos.z);
		float x = Mathf.InverseLerp(camera.transform.position.x - camera.orthographicSize, camera.transform.position.x + camera.orthographicSize, worldPos.x);

		return new Vector2(x, y);
	}

	private void CalculateBounds()
	{
		int includeLayers = camera.cullingMask;

		// linq query to find all Renderer bounds in all visible objects for the camera
		var scenes = SceneExtensions.GetAllLoadedScenes();
		var roots = scenes.SelectMany(x => x.GetRootGameObjects()).Select(x => x.transform);
		var visible = roots.SelectMany(x => x.EnumerateChildrenRecursive()).Where(x => ((1 << x.gameObject.layer) & includeLayers) != 0);
		var renderers = visible.Select(x => x.GetComponent<Renderer>()).Where(x => x != null);
		var allBounds = renderers.Select(x => x.bounds);

		if(allBounds.Count() == 0)
		{
			bounds = new Bounds(Vector3.zero, Vector3.one * 10);
		}else
		{
			// encapsulate all bounds into a single bounds
			bounds = allBounds.Aggregate((combined, next) => { combined.Encapsulate(next); return combined; });
		}
		
		

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
		Invoke("CalculateBounds", 1);
	}

	private void SceneUnloadedCallback(Scene s)
	{
		Invoke("CalculateBounds", 1);
	}
}
