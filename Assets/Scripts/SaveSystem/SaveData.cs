using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
		public List<SceneSaveData> savedScenes;
		public Dictionary<string, object> customData = new Dictionary<string, object>();


		public void AddCustomData(string key, object data)
		{
			if (data == null)
			{
				return;
			}
			customData.Add(key, data);
		}

		public T LoadCustomDataClass<T>(string key) where T : class
		{
			bool success = customData.TryGetValue(key, out object val);
			var data = (T)val;
			return data;
		}

		public T? LoadCustomDataStruct<T>(string key) where T : struct
		{
			bool success = customData.TryGetValue(key, out object val);
			T? data = success ? (T?)val : null;
			return data;
		}

		/**
		 * <summary>
		 * Serializes this save data to a string. Also see <seealso cref="FromString"/>
		 * </summary>
		 */
		public string AsString()
		{
			return SaveSerializer.Serialize(this);
		}

		/**
		 * <summary>
		 * Serializes this save data to a compressed string. Also see <seealso cref="FromStringCompressed"/>
		 * </summary>
		 */
		public string AsStringCompressed()
		{
			return StringCompress.Compress(AsString());
		}

		/**
		 * <summary>
		 * Deserializes a string as SaveData. Also see <seealso cref="AsString"/>
		 * </summary>
		 */
		public static SaveData FromString(string saveData)
		{
			return SaveSerializer.Deserialize<SaveData>(saveData);
		}

		/**
		 * <summary>
		 * Deserializes a compressed string as SaveData. Also see <seealso cref="AsStringCompressed"/>
		 * </summary>
		 */
		public static SaveData FromStringCompressed(string compressedData)
		{
			return FromString(StringCompress.Decompress(compressedData));
		}

		/**
		 * <summary>
		 * Serializes this save data to a stream. Also see <seealso cref="FromStream"/>
		 * </summary>
		 */
		public void WriteToStream(Stream stream)
		{
			SaveSerializer.SerializeToStream(this, stream);
		}

		/**
		 * <summary>
		 * Deserializes a stream as SaveData. Also see <seealso cref="WriteToStream"/>
		 * </summary>
		 */
		public static SaveData FromStream(Stream source)
		{
			return SaveSerializer.DeserializeFromStream<SaveData>(source);
		}

		/**
		 * <summary>
		 * Serializes this save data to a stream as compressed binary data. Also see <seealso cref="FromStreamCompressed"/>
		 * </summary>
		 */
		public void WriteToStreamCompressed(Stream stream)
		{
			using (var gs = StringCompress.GetCompressionStream(stream))
			{
				WriteToStream(gs);
			}
		}

		/**
		 * <summary>
		 * Deserializes a compressed binary stream as SaveData. Also see <seealso cref="WriteToStreamCompressed"/>
		 * </summary>
		 */
		public static SaveData FromStreamCompressed(Stream source)
		{
			using (var gs = StringCompress.GetDecompressionStream(source))
			{
				return FromStream(gs);
			}
		}

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
		public Vector3 position;
		public Vector3 rotation;
		public Vector3 localScale;
		public SaveableSaveData[] saveables;
	}

	[System.Serializable]
	public struct SpawnedObjectSaveData
	{
		public int spawnIndex;
		public string objectName;
		public string resourcePath;
		public bool saveParent;
		public Vector3 position;
		public Vector3 rotation;
		public Vector3 localScale;
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
		public Vector3 position;

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
	internal class GameObjectReference
	{
		public string id = null;

		public GameObject GetGameObject()
		{
			if(string.IsNullOrEmpty(id))
			{
				return null;
			}
			else
			{
				return GameObjectID.GetObjectByID(id);
			}
		}
	}

}
