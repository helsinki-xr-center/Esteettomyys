using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayerHere : MonoBehaviour
{
    void Start()
    {
		new TeleportMessage(transform.position).Deliver();
    }
}
