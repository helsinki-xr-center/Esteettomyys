using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * A helper class that finds the player and feeds its transform to mapper.
 * </summary>
 */
public class MapperFindPlayer : MonoBehaviour
{
	public Mapper mapper;

	public IEnumerator Start()
	{
		mapper = FindObjectOfType<Mapper>();
		while (mapper.playerTransform == null)
		{
			yield return new WaitForSeconds(1f);

			var playerPos = FindObjectOfType<PlayerPosition>();

			if (playerPos != null)
			{
				mapper.playerTransform = playerPos.transform;
				Destroy(this);
			}
		}
	}
}
