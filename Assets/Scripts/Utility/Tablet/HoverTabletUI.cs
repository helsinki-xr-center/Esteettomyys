using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Automatically Gets the tab buttons and adds listeners to them.
/// </summary>
public class HoverTabletUI : MonoBehaviour
{
	[SerializeField] Button[] buttons;
	[SerializeField] Transform[] contentPages;
	Transform trackPos;

	private void Start()
	{
		buttons = new Button[transform.GetChild(0).transform.childCount];
		contentPages = new Transform[transform.GetChild(1).transform.childCount];
		trackPos = transform.parent.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform;

		for (int i = 0; i < transform.GetChild(0).transform.childCount; i++)
		{
			buttons[i] = transform.GetChild(0).transform.GetChild(i).gameObject.GetComponent<Button>();
		}

		for (int i = 0; i < transform.GetChild(1).transform.childCount; i++)
		{
			contentPages[i] = transform.GetChild(1).transform.GetChild(i).transform;
		}

		buttons[0].onClick.AddListener(() => OpenFilterTab());
		buttons[1].onClick.AddListener(() => OpenOptionsTab());
		buttons[2].onClick.AddListener(() => OpenMapTab());
		buttons[3].onClick.AddListener(() => OpenExitTab());
	}

	private void Update()
	{

		transform.position = new Vector3(trackPos.position.x, trackPos.position.y, trackPos.position.z);

	}

	void OpenFilterTab()
	{
		if (!contentPages[0].gameObject.activeSelf)
		{
			contentPages[0].gameObject.SetActive(true);
		}
		else
		{
			contentPages[0].gameObject.SetActive(false);
		}
		contentPages[1].gameObject.SetActive(false);
		contentPages[2].gameObject.SetActive(false);
		contentPages[3].gameObject.SetActive(false);

	}

	void OpenOptionsTab()
	{
		contentPages[0].gameObject.SetActive(false);
		if (!contentPages[1].gameObject.activeSelf)
		{
			contentPages[1].gameObject.SetActive(true);
		}
		else
		{
			contentPages[1].gameObject.SetActive(false);
		}
		contentPages[2].gameObject.SetActive(false);
		contentPages[3].gameObject.SetActive(false);
	}

	void OpenMapTab()
	{
		contentPages[0].gameObject.SetActive(false);
		contentPages[1].gameObject.SetActive(false);
		if (!contentPages[2].gameObject.activeSelf)
		{
			contentPages[2].gameObject.SetActive(true);
		}
		else
		{
			contentPages[2].gameObject.SetActive(false);
		}
		contentPages[3].gameObject.SetActive(false);
	}

	void OpenExitTab()
	{
		contentPages[0].gameObject.SetActive(false);
		contentPages[1].gameObject.SetActive(false);
		contentPages[2].gameObject.SetActive(false);
		if (!contentPages[3].gameObject.activeSelf)
		{
			contentPages[3].gameObject.SetActive(true);
		}
		else
		{
			contentPages[3].gameObject.SetActive(false);
		}
	}
}
