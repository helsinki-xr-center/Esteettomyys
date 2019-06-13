using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Sets the material of a Renderer to be the <see cref="Mapper.replaceMaterial"/> for rendering the map. Will be added automatically by <see cref="Mapper"/>.
 * </summary>
 */
public class ReplaceMaterialOnCameraRender : MonoBehaviour
{
	public Camera targetCamera;
	public Mapper mapper;

	private Material originalMaterial;
	private new Renderer renderer;

	private void Start()
	{
		renderer = GetComponent<Renderer>();
		if(renderer != null)
		{
			originalMaterial = renderer.sharedMaterial;
		}
	}

	/**
	 * <summary>
	 * Sets the material of a Renderer to be the <see cref="Mapper.replaceMaterial"/>.
	 * </summary>
	 */
	public void ReplaceMaterial()
	{
		if(mapper.replaceMaterial != null && renderer != null)
		{
			renderer.material = mapper.replaceMaterial;
		}
	}

	/**
	 * <summary>
	 * Resets the material back to the original.
	 * </summary>
	 */
	public void ResetMaterial()
	{
		if (mapper.replaceMaterial != null && renderer != null)
		{
			renderer.material = originalMaterial;
		}
	}
}
