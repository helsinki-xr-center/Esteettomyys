using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapperFindOtherPlayers : MonoBehaviour
{
	public Mapper mapper;
    void Start()
    {
		StartCoroutine(LookForPlayers());
	}

    private IEnumerator LookForPlayers()
	{
		mapper = FindObjectOfType<Mapper>();
		float startTime = Time.time;
		while (true)
		{
			yield return new WaitForSeconds(5f);

			var players = FindObjectsOfType<AvatarFollowPlayer>();
			
			foreach(var player in players)
			{
				if (player.IsLocal())
					continue;
				if (mapper.otherPlayerTransforms.Contains(player.transform))
					continue;
				mapper.otherPlayerTransforms.Add(player.transform);
			}
		}
	}
}
