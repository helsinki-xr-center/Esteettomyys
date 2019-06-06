using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilterTab : MonoBehaviour
{

	[SerializeField]Button[] buttons;
	[SerializeField] Transform[] contentPages;

	private void Start()
	{
		buttons = transform.GetChild(0).GetComponentsInChildren<Button>();
		contentPages = new Transform[transform.childCount - 1];
		buttons[0].onClick.AddListener(() => OpenColorFilters());
		buttons[1].onClick.AddListener(() => OpenSightFilters());
		buttons[2].onClick.AddListener(() => OpenOtherFilters());

		for (int i = 1; i < transform.childCount; i++)
		{
			contentPages[i - 1] = transform.GetChild(i);
		}

	}

	void OpenColorFilters()
	{
		contentPages[0].gameObject.SetActive(true);
		contentPages[1].gameObject.SetActive(false);
		contentPages[2].gameObject.SetActive(false);
	}

	void OpenSightFilters()
	{

		contentPages[1].gameObject.SetActive(true);
		contentPages[0].gameObject.SetActive(false);
		contentPages[2].gameObject.SetActive(false);
	}

	void OpenOtherFilters()
	{
		contentPages[2].gameObject.SetActive(true);
		contentPages[0].gameObject.SetActive(false);
		contentPages[1].gameObject.SetActive(false);
	}

	
}
