using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
	[AddComponentMenu("Saving/Scene Saveable")]
	[RequireComponent(typeof(GameObjectID))]
	public sealed class SceneSaveable : MonoBehaviour
	{
		public bool loadAfterStart = true;
		public bool saveTransform = false;
		public UnityEngine.Object[] saveables;

		private GameObjectID objectID;

		public string saveableID { get { return objectID.id; } }

		private void Awake()
		{
			objectID = GetComponent<GameObjectID>();
		}

		internal SceneObjectSaveData GetSaveData()
		{
			SceneObjectSaveData data = new SceneObjectSaveData()
			{
				saveableID = saveableID,
				saveTransform = saveTransform,
				position = transform.position,
				rotation = transform.eulerAngles,
				localScale = transform.localScale,
				saveables = new SaveableSaveData[saveables.Length]
			};

			for (int i = 0; i < saveables.Length; i++)
			{
				SaveWriter writer = new SaveWriter();
				(saveables[i] as ISaveable).Save(writer);
				data.saveables[i] = writer.GetSaveData();
			}

			return data;
		}

		internal void LoadSaveData(SceneObjectSaveData saveData)
		{
			if (saveData.saveableID != saveableID)
			{
				Debug.LogError("Saveable ID's don't match", this);
				return;
			}

			if(saveData.saveTransform)
			{
				transform.position = saveData.position;
				transform.eulerAngles = saveData.rotation;
				transform.localScale = saveData.localScale;
			}

			if (loadAfterStart)
			{
				StartCoroutine(DelayLoading(saveData));
			}
			else
			{
				LoadSaveablesData(saveData);
			}

			
		}

		private IEnumerator DelayLoading(SceneObjectSaveData saveData)
		{
			yield return null;
			LoadSaveablesData(saveData);
		}

		private void LoadSaveablesData(SceneObjectSaveData saveData)
		{
			if (saveData.saveables.Length != saveables.Length)
			{
				Debug.LogWarning("Saveable array length doesn't match with save data.", this);
				return;
			}

			for (int i = 0; i < System.Math.Min(saveables.Length, saveData.saveables.Length); i++)
			{
				SaveReader reader = new SaveReader(saveData.saveables[i].serializedData);
				(saveables[i] as ISaveable).Load(reader);
			}
		}
	}
}