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
	bool hasObj;

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

	private void OnEnable()
	{
		Pointer.SelectedObjectEvent += EnableObjectScreen;
	}

	private void OnDisable()
	{
		Pointer.SelectedObjectEvent -= EnableObjectScreen;
	}

	private void Update()
	{

		transform.position = new Vector3(trackPos.position.x, trackPos.position.y, trackPos.position.z);

	}

	public void EnableObjectScreen(bool hasObj, Transform obj)
	{

		if (hasObj && !contentPages[4].gameObject.activeSelf)
		{
			ActivateIndex(4, false);
			hasObj = true;
		}
		if(!hasObj)
		{
			contentPages[4].gameObject.SetActive(false);
			hasObj = false;
		}

	}

	public void ActivateIndex(int index, bool deactivateAll)
	{
		if (deactivateAll)
		{
			for (int i = 0; i < contentPages.Length; i++)
			{
				contentPages[i].gameObject.SetActive(false);
			}
		}
		else
		{
			for (int i = 0; i < contentPages.Length; i++)
			{
				if (i != index)
				{
					contentPages[i].gameObject.SetActive(false);
				}
				else
				{
					contentPages[i].gameObject.SetActive(true);
				}

			}
		}
	}

	void OpenFilterTab()
	{
		if (!contentPages[0].gameObject.activeSelf)
		{
			ActivateIndex(0, false);
		}
		else if (contentPages[0].gameObject.activeSelf && hasObj)
		{
			ActivateIndex(4, false);
		}
		else
		{
			ActivateIndex(0, true);
		}

	}

	void OpenOptionsTab()
	{
		if (!contentPages[1].gameObject.activeSelf)
		{
			ActivateIndex(1, false);
		}
		else if (contentPages[1].gameObject.activeSelf && hasObj)
		{
			ActivateIndex(4, false);
		}
		else
		{
			ActivateIndex(1, true);
		}
	}

	void OpenMapTab()
	{
		
		if (!contentPages[2].gameObject.activeSelf)
		{
			ActivateIndex(2, false);
		}
		else if (contentPages[2].gameObject.activeSelf && hasObj)
		{
			ActivateIndex(4, false);
		}
		else
		{
			ActivateIndex(2, true);
		}
	}

	void OpenExitTab()
	{
		
		if (!contentPages[3].gameObject.activeSelf)
		{
			ActivateIndex(3, false);
		}
		else if (contentPages[3].gameObject.activeSelf && hasObj)
		{	
			ActivateIndex(4, false);
		}
		else
		{
			ActivateIndex(3, true);
		}
	}
}
