using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerLockedOnThisScene : MonoBehaviour
{
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(0.1f);
		if (Pointer.instance != null)
		{
			Pointer.instance.lockLaserOn = true;
		}
	}

	private void OnDestroy()
	{
		if (Pointer.instance != null)
		{
			Pointer.instance.lockLaserOn = false;
		}
	}
}
