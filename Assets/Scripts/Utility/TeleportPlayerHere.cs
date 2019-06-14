using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * On start, sends a <see cref="TeleportMessage"/> with the position of this GameObject.
 * </summary>
 */
public class TeleportPlayerHere : MonoBehaviour
{
    void Start()
    {
		new TeleportMessage(transform.position).Deliver();
    }
}
