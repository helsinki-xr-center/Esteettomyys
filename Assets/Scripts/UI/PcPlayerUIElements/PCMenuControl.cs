using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// 
/// Handle The PC Menu Tabs
/// </summary>
public class PCMenuControl : MonoBehaviour
{

	Transform fullMenu;
	Transform partMenu;
	[SerializeField] Button[] menuTabButtons;
	[SerializeField] Transform[] contentPages;

	public delegate void MenuOpenDelegate(bool status);
	public static event MenuOpenDelegate OnMenuOpenEvent;

	private void Start()
	{
		fullMenu = transform.GetChild(0);
		partMenu = transform.GetChild(1);

		menuTabButtons = new Button[transform.GetChild(0).transform.GetChild(0).transform.childCount];
		contentPages = new Transform[transform.GetChild(0).transform.childCount - 1];
		Debug.Log(transform.GetChild(0).transform.childCount - 1);

		for (int i = 0; i < transform.GetChild(0).transform.childCount; i++)
		{
			if (i == 0)
			{
				continue;
			}

			contentPages[i -1] = transform.GetChild(0).transform.GetChild(i).transform;

		}

		for (int i = 0; i < transform.GetChild(0).transform.GetChild(0).transform.childCount; i++)
		{
			menuTabButtons[i] = transform.GetChild(0).transform.GetChild(0).transform.GetChild(i).GetComponent<Button>();
		}

		menuTabButtons[0].onClick.AddListener(() => OpenFilterContent());
		menuTabButtons[1].onClick.AddListener(() => OpenOptionsContent());
		menuTabButtons[2].onClick.AddListener(() => OpenMapContent());
		menuTabButtons[3].onClick.AddListener(() => OpenExitContent());

	}

	private void OnEnable()
	{
		PcPlayer.OpenMenuEvent += OpenMenu;
		PcPlayer.OnDeselectObjectEvent += OnDeselectObject;
	}

	private void OnDisable()
	{
		PcPlayer.OpenMenuEvent -= OpenMenu;
		PcPlayer.OnDeselectObjectEvent -= OnDeselectObject;
	}

	void OpenFilterContent()
	{
		OpenTab(0, true);
	}

	void OpenOptionsContent()
	{
		OpenTab(1, true);
	}

	void OpenMapContent()
	{
		OpenTab(2, true);
	}

	void OpenExitContent()
	{
		OpenTab(3, true);
	}

	void OpenTab(int index, bool open)
	{
		if (open)
		{
			for (int i = 0; i < contentPages.Length; i++)
			{
				if (i == index && !contentPages[i].transform.gameObject.activeSelf)
				{
					contentPages[i].transform.gameObject.SetActive(true);

					continue;
				}
							
				contentPages[i].transform.gameObject.SetActive(false);
				
			}
		}
		else
		{
			for (int i = 0; i < contentPages.Length; i++)
			{

				contentPages[i].transform.gameObject.SetActive(false);

			}
		}
	}

	public void OpenMenu(bool hasObj)
	{
		if(hasObj)
		{
			if(!fullMenu.gameObject.activeSelf)
			{
				if (!partMenu.gameObject.activeSelf)
				{
					partMenu.gameObject.SetActive(true);
					OnMenuOpenEvent?.Invoke(true);
				}
				else
				{
					partMenu.gameObject.SetActive(false);
					OnMenuOpenEvent?.Invoke(false);
				}
			}
			else
			{
				fullMenu.gameObject.SetActive(false);
				OnMenuOpenEvent?.Invoke(false);
			}
		}
		else
		{
			
			if(!partMenu.gameObject.activeSelf)
			{
				if(!fullMenu.gameObject.activeSelf)
				{
					fullMenu.gameObject.SetActive(true);
					OnMenuOpenEvent?.Invoke(true);
				}
				else
				{
					fullMenu.gameObject.SetActive(false);
					OnMenuOpenEvent?.Invoke(false);
				}
			}
			else
			{
				partMenu.gameObject.SetActive(false);
				OnMenuOpenEvent?.Invoke(false);
			}
		}

	}

	public void OnDeselectObject(GameObject obj)
	{
		if (partMenu.gameObject.activeSelf)
		{
			partMenu.gameObject.SetActive(false);
		}
	}

}
