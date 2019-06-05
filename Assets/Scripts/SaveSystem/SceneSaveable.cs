using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
	public sealed class SceneSaveable : MonoBehaviour
	{
		public string saveableID;
		public bool saveTransform = false;
		public UnityEngine.Object[] saveables;

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