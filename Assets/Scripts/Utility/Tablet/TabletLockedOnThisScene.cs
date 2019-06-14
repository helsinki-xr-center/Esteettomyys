using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletLockedOnThisScene : MonoBehaviour
{
	TabletStatePattern tablet;

	private IEnumerator Start()
	{
	
		yield return new WaitForSeconds(0.1f);
		tablet = FindObjectOfType<TabletStatePattern>();
		if (tablet != null)
		{
			tablet.gameObject.SetActive(false);
		}
	}

	private void OnDestroy()
	{
		if (tablet != null)
		{
			tablet.gameObject.SetActive(true);
		}
	}
}
