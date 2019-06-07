using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutoTeleporter : MonoBehaviour
{
	private void OnEnable()
	{
		TeleportMessage.AddListener(OnTeleport);
	}

	private void OnDisable()
	{
		TeleportMessage.RemoveListener(OnTeleport);
	}

	private void OnTeleport(TeleportMessage tel)
	{
		transform.position = tel.targetLocation;
	}
}



/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Message for forcibly teleporting the player to a certain location.
 * For example:
 * <example>
 * <code>
 * new TeleportMessage(Vector3.zero).Deliver();
 * </code>
 * </example>
 * </summary>
 */
public class TeleportMessage
{

	public delegate void TeleportEventDelegate(TeleportMessage message);
	private static event TeleportEventDelegate StaticEvent;

	public Vector3 targetLocation;

	public TeleportMessage(Vector3 location)
	{
		this.targetLocation = location;
	}

	/**
	 * <summary>Delivers this event to the consumers.</summary>
	 */
	public void Deliver()
	{
		StaticEvent?.Invoke(this);
	}

	/**
	 * <summary>Adds a consumer for this message type. Remember to call <see cref="RemoveListener"/> before the consumer is destroyed.</summary>
	 */
	public static void AddListener(TeleportEventDelegate del) => StaticEvent += del;
	/**
	 * <summary>Removes a consumer from this message type.</summary>
	 */
	public static void RemoveListener(TeleportEventDelegate del) => StaticEvent -= del;
}