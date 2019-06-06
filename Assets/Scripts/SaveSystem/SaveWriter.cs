using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SaveSystem
{
	public class SaveWriter
	{
		private Dictionary<string, string> data = new Dictionary<string, string>();

		public void Write<T>(string key, T value)
		{
			string val = SaveSerializer.Serialize(value);
			WriteString(key, val);
		}

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
