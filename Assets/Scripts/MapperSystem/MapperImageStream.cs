using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class MapperImageStream : MonoBehaviour
{
	public Mapper mapper;

	private RawImage rawImage;
    // Start is called before the first frame update
    void Awake()
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
