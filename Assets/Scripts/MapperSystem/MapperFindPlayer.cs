using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapperFindPlayer : MonoBehaviour
{
	public Mapper mapper;

	public IEnumerator Start()
	{
		mapper = FindObjectOfType<Mapper>();
		while (mapper.playerTransform == null)
		{
			yield return new WaitForSeconds(0.1f);

			var playerPos = FindObjectOfType<PlayerPosition>();

			if (playerPos != null)
			{
				mapper.playerTransform = playerPos.transform;
				Destroy(this);
			}
		}
	}
}
