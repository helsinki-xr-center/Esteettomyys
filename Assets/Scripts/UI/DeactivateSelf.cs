using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Intended for animation events to be able to deactivate themselves. Deactivates the gameobject attached to this script.
 * </summary>
 */
public class DeactivateSelf : MonoBehaviour
{
	public void Deactivate() => gameObject.SetActive(false);
}
