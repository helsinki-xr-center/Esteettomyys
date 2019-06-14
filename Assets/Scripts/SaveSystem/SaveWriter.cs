using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SaveSystem
{
	/**
	 * Author: Nomi Lakkala
	 * 
	 * <summary>
	 * Writes data to the save.
	 * </summary>
	 */
	public class SaveWriter
	{
		private Dictionary<string, string> data = new Dictionary<string, string>();

		/**
		 * <summary>
		 * Write a value to the save. Does not support <see cref="UnityEngine.MonoBehaviour"/> classes, but supports GameObjects that have a <see cref="GameObjectID"/>.
		 * </summary>
		 */
		public void Write<T>(string key, T value)
		{
			string val = SaveSerializer.Serialize(value);
			WriteString(key, val);
		}

		/**
		 * <summary>
		 * Writes a string to the save. Use this for saving strings!
		 * </summary>
		 */
		public void WriteString(string key, string value)
		{
			data.Add(key, value);
		}

		internal SaveableSaveData GetSaveData()
		{
			SaveableSaveData saveable = new SaveableSaveData()
			{
				serializedData = data.ToArray()
			};
			return saveable;
		}
	}

}
