using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Usable static methods
/// </summary>
public static class ExtensionMethods
{

	/// <summary>
	/// Resets Color Back to the Original color
	/// </summary>
	/// <param name="obj">Object that color needs to be reset</param>
	/// <param name="original">Original color of the object</param>
	public static void MaterialResetColorChange(GameObject obj, Color original)
	{

		MeshRenderer[] meshRenderers = obj.GetComponents<MeshRenderer>();

		foreach (MeshRenderer mr in meshRenderers)
		{
			mr.material.color = original;
		}
	}

	/// <summary>
	/// Changes MeshRenderers Material Color;
	/// </summary>
	/// <param name="obj">Object that needs to have mesh material color Changed</param>
	/// <param name="newColor">Color to change to</param>
	public static void MaterialColorChange(GameObject obj, Color newColor)
	{

		MeshRenderer[] meshRenderers = obj.GetComponents<MeshRenderer>();

		foreach (MeshRenderer mr in meshRenderers)
		{
			mr.material.color = newColor;
		}
	}
}
