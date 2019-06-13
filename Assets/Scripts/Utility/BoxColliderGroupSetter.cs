using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @Author : Veli-Matti Vuoti
/// 
/// Sizes one Boxcollider for fences with z axis as lengthwise axis
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class BoxColliderGroupSetter : MonoBehaviour
{
	[SerializeField] MeshRenderer[] childs;
	BoxCollider col;
	public bool editOnStart;
	public MeshShape meshShape;
	float totalLength;
	float height;
	float width;
	public int differentObjectAmount;
	[Tooltip("IF FENCES HAVE EMPTY SPACE BETWEEN THEM")] public float emptySpace;
	[Tooltip("Length for stairstep")]public float stepLength;

	private void OnValidate()
	{
		if (!editOnStart)
		{
			switch (meshShape)
			{
				case MeshShape.Stairs:
					Stairs();
					break;
				case MeshShape.Fence:
					Fence();
					break;
				case MeshShape.Slope:
					break;
				default:
					break;
			}
		}
	}

	private void Start()
	{
		if (editOnStart)
		{
			switch (meshShape)
			{
				case MeshShape.Stairs:
					Stairs();
					break;
				case MeshShape.Fence:
					Fence();
					break;
				case MeshShape.Slope:
					break;
				default:
					break;
			}
		}
	}

	/// <summary>
	/// Fences have z axis as length and x as width y is height
	/// </summary>
	public void Fence()
	{

		col = GetComponent<BoxCollider>();
		totalLength = 0;
		height = 0;

		if (differentObjectAmount > 0)
		{
			childs = new MeshRenderer[transform.childCount];
			if (transform.gameObject.GetComponent<MeshRenderer>())
			{
				childs[0] = transform.gameObject.GetComponent<MeshRenderer>();
			}

			for (int i = 0; i < transform.childCount - differentObjectAmount; i++)
			{
				childs[i + 1] = transform.GetChild(i).GetComponent<MeshRenderer>();
			}
		}
		else
		{
			childs = new MeshRenderer[transform.childCount + 1];
			if (transform.gameObject.GetComponent<MeshRenderer>())
			{
				childs[0] = transform.gameObject.GetComponent<MeshRenderer>();
			}

			for (int i = 0; i < transform.childCount; i++)
			{
				childs[i + 1] = transform.GetChild(i).GetComponent<MeshRenderer>();
			}
		}

		foreach (var item in childs)
		{
			if (item != null)
			{
				height = item.bounds.size.y;
				width = item.bounds.size.x;
			}
		}

		for (int i = 0; i < childs.Length; i++)
		{
			if (childs[i] == null)
			{
				continue;
			}
			totalLength += childs[i].bounds.size.z + emptySpace;
		}

		col.center = childs[childs.Length / 2].transform.position - transform.position;
		col.size = new Vector3(col.size.x, height, totalLength );

	}

	/// <summary>
	/// Stairs have z axis as width and x as length y is height
	/// </summary>
	public void Stairs()
	{
		
	}
}
