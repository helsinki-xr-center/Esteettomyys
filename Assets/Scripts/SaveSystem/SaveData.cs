﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveSystem
{
	[System.Serializable]
	public class SaveData
	{
		public string saveName;
		public DateTime timestamp;
		public SceneSaveData[] savedScenes;

		public string AsString()
		{
			return SaveSerializer.Serialize(this);
		}

		public string AsStringCompressed()
		{
			return StringCompress.Compress(AsString());
		}

		public static SaveData FromString(string saveData)
		{
			return SaveSerializer.Deserialize<SaveData>(saveData);
		}

		public static SaveData FromStringCompressed(string compressedData)
		{
			return FromString(StringCompress.Decompress(compressedData));
		}

		public static Stream AsStream() => throw new NotImplementedException();

		public static SaveData FromStream(Stream source) => throw new NotImplementedException();

		public static Stream AsStreamCompressed() => throw new NotImplementedException();

		public static SaveData FromStreamCompressed(Stream source) => throw new NotImplementedException();

	}

	[System.Serializable]
	public struct SceneSaveData
	{
		public string sceneName;
		public SceneObjectSaveData[] sceneData;
		public SpawnedObjectSaveData[] spawnedData;
	}

	[System.Serializable]
	public struct SceneObjectSaveData
	{
		public string saveableID;
		public bool saveTransform;
		public Vector3SaveData position;
		public Vector3SaveData rotation;
		public Vector3SaveData localScale;
		public SaveableSaveData[] saveables;
	}

	[System.Serializable]
	public struct SpawnedObjectSaveData
	{
		public int spawnIndex;
		public string objectName;
		public string resourcePath;
		public bool saveParent;
		public Vector3SaveData position;
		public Vector3SaveData rotation;
		public Vector3SaveData localScale;
		public ParentData parent;
		public SaveableSaveData[] saveables;
	}

	[System.Serializable]
	public struct SaveableSaveData
	{
		public KeyValuePair<string, string>[] serializedData;
	}

	[System.Serializable]
	public class ParentData
	{
		public string name;
		public string rootName;
		public int siblingIndex;
		public int rootSiblingIndex;
		public Vector3SaveData position;

		public ParentData(Transform source)
		{
			if (source == null)
			{
				name = string.Empty;
				siblingIndex = 0;
				rootName = string.Empty;
				rootSiblingIndex = 0;
				position = Vector3.zero;
			}
			else
			{
				name = source.name;
				siblingIndex = source.GetSiblingIndex();
				rootName = source.root.name;
				rootSiblingIndex = source.root.GetSiblingIndex();
				position = source.position;
			}

		}

		public Transform GetTransform(Scene scene)
		{
			if (string.IsNullOrEmpty(name)) // no parent
			{
				return null;
			}

			var data = this;
			var roots = scene.GetRootGameObjects();
			IEnumerable<Transform> matchingItems = Array.Empty<Transform>();

			if (rootName != name) //the parent is a child of one of the root objects
			{
				var matchingRoots = roots.Select(x => x.transform).Where(x => x.name == data.rootName);
				if(matchingRoots.Count() > 1) //more than one matching root, find by sibling index
				{
					matchingRoots = matchingRoots.Where(x => x.GetSiblingIndex() == data.rootSiblingIndex);
				}
				matchingItems = matchingRoots.SelectMany(x => x.GetChildrenRecursive().Where(y => y.name == data.name));

			}  //the parent is one of the root objects
			else
			{
				matchingItems = roots.Select(x => x.transform).Where(x => x.name == data.rootName);
			}


			if (matchingItems.Count() > 1) // more than one match. match by sibling index.
			{
				var matching = matchingItems.Where(x => x.GetSiblingIndex() == data.siblingIndex);
				if (matching.Count() == 1)
				{
					return matching.Single();
				}
				else //find closest position
				{
					return matching.MinBy(x => Vector3.Distance(data.position, x.position));
				}
			}
			else // found perfect match.
			{
				var matching = matchingItems.SingleOrDefault();
				if (matching != null)
				{
					return matching;
				}
			}

			//nothing worked. Just return null.
			return null;
		}

		public static implicit operator ParentData(Transform src) => src != null ? new ParentData(src) : null;
	}

	[System.Serializable]
	public struct Vector3SaveData
	{
		public float x, y, z;

		public static implicit operator Vector3(Vector3SaveData data)
		{
			return new Vector3(data.x, data.y, data.z);
		}

		public static implicit operator Vector3SaveData(Vector3 vec)
		{
			return new Vector3SaveData()
			{
				x = vec.x,
				y = vec.y,
				z = vec.z
			};
		}
	}
}
