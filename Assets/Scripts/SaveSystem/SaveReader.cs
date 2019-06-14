using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
	/**
	 * Author: Nomi Lakkala
	 * 
	 * <summary>
	 * Reads data from the save.
	 * </summary>
	 */
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

		/**
		 * <summary>
		 * Reads a value from the save file.
		 * </summary>
		 */
		public T Read<T>(string key)
		{
			if(!data.TryGetValue(key, out string s))
			{
				return default;
			}
			return SaveSerializer.Deserialize<T>(s);
		}

		/**
		 * <summary>
		 * Reads a string from the save file. More efficient than using <see cref="Read{string}"/>
		 * </summary>
		 */
		public string ReadString(string key)
		{
			string s = data[key];
			return s;
		}
	}
}
