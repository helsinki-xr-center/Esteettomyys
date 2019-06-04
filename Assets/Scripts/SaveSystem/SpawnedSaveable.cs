using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
	public sealed class SpawnedSaveable : MonoBehaviour
	{
		public string resourcePath;
		public bool saveParent = false;
		public UnityEngine.Object[] saveables;

		internal SpawnedObjectSaveData GetSaveData()
		{
			SpawnedObjectSaveData data = new SpawnedObjectSaveData()
			{
				resourcePath = resourcePath,
				saveParent = saveParent,
				position = transform.position,
				rotation = transform.eulerAngles,
				localScale = transform.localScale,
				saveables = new SaveableSaveData[saveables.Length]
			};

			if (saveParent)
			{
				data.parent = transform.parent;
			}

			for (int i = 0; i < saveables.Length; i++)
			{
				SaveWriter writer = new SaveWriter();
				(saveables[i] as ISaveable).Save(writer);
				data.saveables[i] = writer.GetSaveData();
			}

			return data;
		}

		internal void LoadSaveData(SpawnedObjectSaveData saveData)
		{
			if (saveData.saveParent)
			{
				transform.SetParent(saveData.parent.GetTransform(gameObject.scene));
			}
			transform.position = saveData.position;
			transform.eulerAngles = saveData.rotation;
			transform.localScale = saveData.localScale;

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
