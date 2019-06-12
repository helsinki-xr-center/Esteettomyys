using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
	public class SaveReader
	{
		private Dictionary<string, string> data;
		
		internal SaveReader(KeyValuePair<string, string>[] arr)
		{
			data = new Dictionary<string, string>(arr.Length);
			for (int i = 0; i < arr.Length; i++)
			{
				data.Add(arr[i].Key, arr[i].Value);
			}
		}

		public T Read<T>(string key)
		{
			if(!data.TryGetValue(key, out string s))
			{
				return default;
			}
			return SaveSerializer.Deserialize<T>(s);
		}

		public string ReadString(string key)
		{
			string s = data[key];
			return s;
		}
	}
}
