using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCMenuControl : MonoBehaviour
{

	Transform fullMenu;
	Transform partMenu;

	private void Start()
	{
		fullMenu = transform.GetChild(0);
		partMenu = transform.GetChild(1);
	}

	private void OnEnable()
	{
		PcPlayer.OpenMenuEvent += OpenMenu;
	}

	private void OnDisable()
	{
		PcPlayer.OpenMenuEvent -= OpenMenu;
	}

	public void OpenMenu(bool hasObj)
	{

		if(hasObj && !partMenu.gameObject.activeSelf)
		{
			partMenu.gameObject.SetActive(true);
		}
		else if (hasObj && partMenu.gameObject.activeSelf)
		{
			partMenu.gameObject.SetActive(false);
		}
		else if(!hasObj && !fullMenu.gameObject.activeSelf)
		{
			fullMenu.gameObject.SetActive(true);
		}
		else if(!hasObj && fullMenu.gameObject.activeSelf)
		{
			fullMenu.gameObject.SetActive(false);
		}
		
	}

}
