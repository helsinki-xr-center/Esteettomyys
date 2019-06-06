using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class BoxColliderGroupSetter : MonoBehaviour
{
	[SerializeField]MeshRenderer[] childs;
	BoxCollider col;
	public bool isLastObjectIdentical;
	float totalLength;
	float height;

	private void Start()
	{
		col = GetComponent<BoxCollider>();
		if (!isLastObjectIdentical)
		{
			childs = new MeshRenderer[transform.childCount];
			childs[0] = transform.gameObject.GetComponent<MeshRenderer>();
			for (int i = 0; i < transform.childCount - 1; i++)
			{
				childs[i + 1] = transform.GetChild(i).GetComponent<MeshRenderer>();
			}
		}
		else
		{
			childs = new MeshRenderer[transform.childCount+1];
			childs[0] = transform.gameObject.GetComponent<MeshRenderer>();
			for (int i = 0; i < transform.childCount; i++)
			{
				childs[i + 1] = transform.GetChild(i).GetComponent<MeshRenderer>();
			}
		}

		CalculateCollider();
	}

	public void CalculateCollider()
	{

		height = childs[0].bounds.size.y;

		for (int i = 0; i < childs.Length; i++)
		{
			totalLength += childs[i].bounds.size.z;
		}

		col.center = Vector3.zero;
		col.size = Vector3.forward * totalLength;

	}
}
