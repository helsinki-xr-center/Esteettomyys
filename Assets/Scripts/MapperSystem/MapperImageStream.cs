using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Shows the image feed from <see cref="Mapper"/> on a RawImage. Also calls <see cref="Mapper.RenderFrame"/> 10 times a second.
 * </summary>
 */
[RequireComponent(typeof(RawImage))]
public class MapperImageStream : MonoBehaviour
{
	public Mapper mapper;

	private RawImage rawImage;


	private void Awake()
    {
		rawImage = GetComponent<RawImage>();
    }

	private void Start()
	{
		mapper = FindObjectOfType<Mapper>();
		if (mapper != null)
		{
			rawImage.texture = mapper.GetTexture();
		}
	}

	private void OnEnable()
	{
		mapper = FindObjectOfType<Mapper>();
		if(mapper != null)
		{
			rawImage.texture = mapper.GetTexture();
		}

		StartCoroutine(RenderMap());
	}

	/**
	 * <summary>
	 * Handles rendering the map. Calls <see cref="Mapper.RenderFrame"/> while active and enabled.
	 * </summary>
	 */
	private IEnumerator RenderMap()
	{
		if (mapper == null)
		{
			yield break;
		}

		while(isActiveAndEnabled)
		{
			mapper.RenderFrame();
			yield return new WaitForSecondsRealtime(0.1f);
		}
	}
}
