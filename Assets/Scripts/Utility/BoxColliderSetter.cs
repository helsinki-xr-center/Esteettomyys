﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @Author : Veli-Matti Vuoti
/// 
/// Sets Collider around on object on editor or at game start, 
/// Might be usefull
/// </summary>
[RequireComponent (typeof(BoxCollider), typeof(MeshRenderer))]
public class BoxColliderSetter : MonoBehaviour
{

	BoxCollider col;
	MeshRenderer mr;
	public bool updateInEditor;

	private void OnValidate()
	{
		if (updateInEditor)
		{
			col = GetComponent<BoxCollider>();
			mr = GetComponent<MeshRenderer>();

			col.center = mr.bounds.center - gameObject.transform.position;
			col.size = mr.bounds.size;
		}
	}

	private void Start()
	{
		if (!updateInEditor)
		{
			col = GetComponent<BoxCollider>();
			mr = GetComponent<MeshRenderer>();

			col.center = mr.bounds.center - gameObject.transform.position;
			col.size = mr.bounds.size;
		}
	}


}
