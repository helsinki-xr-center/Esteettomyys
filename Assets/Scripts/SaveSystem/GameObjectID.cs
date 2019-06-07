using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EasyHumanIds;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif


namespace SaveSystem
{
	[ExecuteInEditMode]
	[AddComponentMenu("Saving/GameObjectID")]
	public class GameObjectID : MonoBehaviour
	{
		private static System.Random r = new System.Random();
		private static Dictionary<string, GameObjectID> allIds = new Dictionary<string, GameObjectID>();
		public string id;

		private void Awake()
		{
			if (!string.IsNullOrEmpty(id))
			{
				if (!allIds.ContainsKey(id))
				{
					allIds.Add(id, this);
				}
			}
		}

#if UNITY_EDITOR
		// Update is called once per frame
		void Update()
		{
			if (Application.isPlaying)
				return;

			CheckID();
		}

		void OnDestroy()
		{
			if (!String.IsNullOrEmpty(id))
			{
				allIds.Remove(id);
			}
		}

		private void CheckID()
		{
			if (gameObject.scene == null)
			{
				id = String.Empty;
				return;
			}
			if (String.IsNullOrEmpty(id))
			{
				id = GenerateNewId();
				if(allIds.ContainsKey(id))
				{
					id = null;
					return;
				}
				else
				{
					allIds[id] = this;
				}
			}
			if(allIds.ContainsKey(id) && allIds[id] != this)
			{
				id = null;
				return;
			}
			if(!allIds.ContainsKey(id))
			{
				allIds[id] = this;
			}	
		}

		private string GenerateNewId()
		{
			return gameObject.scene.name + "-" + HumanIds.Generate(r);
			//return gameObject.scene.name + "_" + Guid.NewGuid(); //more traditional guids. or if EasyHumanIds ever breaks
		}
#endif

		public static GameObject GetObjectByID(string id)
		{
			if(!allIds.ContainsKey(id))
			{
				Debug.LogWarning($"No GameObject with key {id} found!");
				return null;
			}

			return allIds[id].gameObject;
		}

		
	}
}