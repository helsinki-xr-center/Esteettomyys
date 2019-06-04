using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveSystem
{
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
	public struct ParentData
	{
		public string name;
		public string rootName;
		public int siblingIndex;

		public ParentData(Transform source)
		{
			if (source == null)
			{
				name = string.Empty;
				siblingIndex = 0;
				rootName = string.Empty;
			}
			else
			{
				name = source.name;
				siblingIndex = source.GetSiblingIndex();
				rootName = source.root.name;
			}

		}

		public Transform GetTransform(Scene scene)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}

			var data = this;
			var roots = scene.GetRootGameObjects();
			if (rootName != name)
			{
				var matchingRoots = roots.Select(x => x.transform).Where(x => x.name == data.rootName);
				foreach (Transform root in matchingRoots)
				{
					Transform found = root.Find(name);
					if (found != null)
					{
						return found;
					}
				}
			}
			else
			{
				var matchingRoots = roots.Select(x => x.transform).Where(x => x.name == data.rootName);
				if (matchingRoots.Count() > 1)
				{
					var matching = matchingRoots.SingleOrDefault(x => x.GetSiblingIndex() == data.siblingIndex);
					if (matching != null)
					{
						return matching;
					}
				}
				else
				{
					var matching = matchingRoots.SingleOrDefault();
					if (matching != null)
					{
						return matching;
					}
				}
			}
			//Didn't work somehow. Just return null.
			return null;
		}

		public static implicit operator ParentData(Transform src) => new ParentData(src);
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
