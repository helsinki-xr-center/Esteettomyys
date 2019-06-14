using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// 
/// Interactable Object class
/// </summary>
public class InteractableObject : MonoBehaviour
{
	//RuntimeColors
	Color hoveringColor;
	Color selectedColor;
	Color objColor;

	public ObjectType objectType;
	public InteractableObjectData objData;
	string objname;
	string objdesc;

	public Material hoverColor;
	[Tooltip("Set whether or not you want this interactible to highlight when hovering over it")]
	public bool highlightObject = true;
	public GameObject pointerHighlightHolder;
	public GameObject highlightedMesh;
	MeshRenderer highlightedMeshRenderer;

	public bool _isHovering;
	public bool _wasHovering;
	public bool _selected;
	bool colorChanged;

	private void OnEnable()
	{
		PcPlayer.mouseHoverIn += IsHovered;
		PcPlayer.mouseHoverOut += HoveredEnd;
		PcPlayer.mouseClick += IsClicked;
		Pointer.PointerHit += IsHovered;
		Pointer.PointerLeft += HoveredEnd;
		Pointer.PointerClick += IsClicked;
		PcPlayer.OnDeselectObjectEvent += OnDeselect;
		Pointer.SelectedObjectEvent += OnDeselectPointer;
	}

	private void OnDisable()
	{
		PcPlayer.mouseHoverIn -= IsHovered;
		PcPlayer.mouseHoverOut -= HoveredEnd;
		PcPlayer.mouseClick -= IsClicked;
		Pointer.PointerHit -= IsHovered;
		Pointer.PointerLeft -= HoveredEnd;
		Pointer.PointerClick -= IsClicked;
		PcPlayer.OnDeselectObjectEvent -= OnDeselect;
		Pointer.SelectedObjectEvent -= OnDeselectPointer;
	}

	public void Start()
	{
		if (hoverColor == null)
		{
			Debug.LogError("Add hovering material to obj");
		}

		ObjIndicatorColor();
	}

	/// <summary>
	/// Sets the colors for object
	/// </summary>
	private void ObjIndicatorColor()
	{

		if (objData != null)
		{
			objname = objData.objectName;
			objdesc = objData.objectDescription;
			objColor = objData.objColor;

			switch (GlobalValues.gameMode)
			{
				case GameMode.Tutorial:

					if (objectType == ObjectType.Critical)
					{

						hoveringColor = objData.criticalColor;
						selectedColor = objData.criticalColor;
					}
					else
					{

						hoveringColor = objData.standardColor;
						selectedColor = objData.standardColor;
					}

					break;
				case GameMode.Training:

					if (objectType == ObjectType.Critical)
					{
						hoveringColor = objData.criticalColor;
						selectedColor = objData.criticalColor;
					}
					else
					{
						hoveringColor = objData.standardColor;
						selectedColor = objData.standardColor;

					}
					break;
				case GameMode.Exam:
					if (objectType == ObjectType.Critical)
					{
						hoveringColor = objData.hoveringColor;
						selectedColor = objData.criticalColor;
					}
					else
					{
						hoveringColor = objData.hoveringColor;
						selectedColor = objData.standardColor;

					}
					break;
				default:
					break;
			}
		}
	}

	private void Update()
	{

		if (highlightObject)
		{
			//UpdateHighlightRenderersPointer();
			if (highlightedMesh != null)
			{
				highlightedMesh.transform.SetPositionAndRotation(transform.position, transform.rotation);

			}

			if (!_isHovering && pointerHighlightHolder != null && !_selected)
			{
				Destroy(pointerHighlightHolder);
			}
		}
	}

	public void CreateHighLightHolder()
	{
		pointerHighlightHolder = new GameObject("NewHighLightHolder");
		highlightedMesh = new GameObject("NewHighlightedMesh");
		highlightedMesh.transform.SetParent(pointerHighlightHolder.transform);
		highlightedMeshRenderer = highlightedMesh.AddComponent<MeshRenderer>();
		MeshFilter meshFilter = highlightedMesh.AddComponent<MeshFilter>();
		meshFilter.mesh = gameObject.GetComponent<MeshFilter>().mesh;
		highlightedMesh.transform.SetPositionAndRotation(transform.position, transform.rotation);
		highlightedMeshRenderer.material = hoverColor;

	}

	/// <summary>
	/// Sets color back on deselect
	/// </summary>
	/// <param name="obj"></param>
	public void OnDeselect(GameObject obj)
	{
		if (obj == gameObject)
		{
			_selected = false;
			//ExtensionMethods.MaterialColorChange(gameObject, objColor);
			Debug.Log("DESELECTED" + gameObject.name);
		}
	}

	public void OnDeselectPointer(bool havingObj, Transform obj)
	{
		if (obj == gameObject.transform)
		{
			if (havingObj)
			{

				if (highlightObject && pointerHighlightHolder != null && !_selected)
				{
					_isHovering = false;
					_selected = true;
					highlightedMeshRenderer.material.SetColor("g_vOutlineColor", selectedColor);
					//UpdateHighlightRenderersPointer();
					//ExtensionMethods.MaterialColorChange(gameObject, selectedColor);
					Debug.Log("SELECTED " + gameObject.name);
				}

			}
			else if (!havingObj)
			{

				if (highlightObject && pointerHighlightHolder != null && _selected)
				{
					_isHovering = false;
					_selected = false;
					//UpdateHighlightRenderersPointer();
					//ExtensionMethods.MaterialColorChange(gameObject, selectedColor);
					Debug.Log("DESELECTED OBJ  " + gameObject.name);
				}

			}
		}
	}

	/// <summary>
	/// Sets hovered color for object
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="hitData"></param>
	public void IsHovered(object sender, RayCastData hitData)
	{
		if (hitData.target.gameObject == transform.gameObject && !_isHovering && highlightObject && pointerHighlightHolder == null && !_selected)
		{
			_isHovering = true;
			CreateHighLightHolder();
			highlightedMeshRenderer.material.SetColor("g_vOutlineColor", hoveringColor);
			//UpdateHighlightRenderersPointer();
			//Debug.Log("IM HOVERED OVER" + gameObject.name);
			//ExtensionMethods.MaterialColorChange(gameObject, hoveringColor);

		}
		else if (hitData.target.gameObject != transform.gameObject && _isHovering && highlightObject && pointerHighlightHolder != null)
		{
			_isHovering = false;
			//UpdateHighlightRenderersPointer();
			if (!_selected)
			{
				Destroy(pointerHighlightHolder);
			}
		}
	}

	/// <summary>
	/// Resets color on hovering end
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="hitData"></param>
	public void HoveredEnd(object sender, RayCastData hitData)
	{
		if (hitData.target == transform )
		{

			//Debug.Log("IM HOVERED EXIT" + gameObject.name);
			if (sender.GetType() == typeof(PcPlayer))
			{
				PcPlayer pl = (PcPlayer)sender;

				if (pl.selectedObject != gameObject && highlightObject && pointerHighlightHolder != null)
				{
					//ExtensionMethods.MaterialColorChange(gameObject, objColor);
					//UpdateHighlightRenderersPointer();
					if (!_selected)
					{
						Destroy(pointerHighlightHolder);
					}
					_isHovering = false;
				}
			}
			else if (sender.GetType() == typeof(Pointer))
			{
				Pointer pl = (Pointer)sender;

				if (pl.selectedObj != gameObject && highlightObject && pointerHighlightHolder != null)
				{
					//ExtensionMethods.MaterialColorChange(gameObject, objColor);
					//UpdateHighlightRenderersPointer();
					if (!_selected)
					{
						Destroy(pointerHighlightHolder);
					}
					_isHovering = false;
				}
			}
		}
		else
		{
			if (sender.GetType() == typeof(PcPlayer))
			{
				PcPlayer pl = (PcPlayer)sender;

				if (pl.selectedObject != gameObject && highlightObject && pointerHighlightHolder != null)
				{
					//ExtensionMethods.MaterialColorChange(gameObject, objColor);
					//UpdateHighlightRenderersPointer();
					if (!_selected)
					{
						Destroy(pointerHighlightHolder);
					}
					_isHovering = false;
				}
			}
			else if (sender.GetType() == typeof(Pointer))
			{

				Pointer pl = (Pointer)sender;

				if (pl.selectedObj != gameObject && highlightObject && pointerHighlightHolder != null)
				{
					//ExtensionMethods.MaterialColorChange(gameObject, objColor);
					//UpdateHighlightRenderersPointer();
					if (!_selected)
					{
						Destroy(pointerHighlightHolder);
					}
					_isHovering = false;
				}
			}
		}
	}

	/// <summary>
	/// Click color
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="hitData"></param>
	public void IsClicked(object sender, RayCastData hitData)
	{
		if (hitData.target == transform)
		{
			//Debug.Log("IM CLICKED" + gameObject.name);
			if (sender.GetType() == typeof(PcPlayer))
			{

				if (hitData.target.gameObject == gameObject && highlightObject && pointerHighlightHolder != null && !_selected)
				{
					_selected = true;
					//ExtensionMethods.MaterialColorChange(gameObject, selectedColor);
					highlightedMeshRenderer.material.SetColor("g_vOutlineColor", selectedColor);
					Debug.Log("SELECTED " + gameObject.name);
				}
			}
		}
	}
}
