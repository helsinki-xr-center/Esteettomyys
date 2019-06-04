using Newtonsoft.Json;
using UnityEngine;

namespace SaveSystem
{
	internal static class SaveSerializer
	{
		public static string Serialize<T>(T value)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings();
			settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			return JsonConvert.SerializeObject(value, settings);
		}

		public static T Deserialize<T>(string serialized)
		{
			return JsonConvert.DeserializeObject<T>(serialized);
		}
	}
}