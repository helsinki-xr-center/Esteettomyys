using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace SaveSystem
{

	public class NoPropertiesContractResolver : DefaultContractResolver
	{
		public new static readonly NoPropertiesContractResolver Instance = new NoPropertiesContractResolver();

		protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
		{
			JsonProperty property = base.CreateProperty(member, memberSerialization);

			if (member.MemberType == MemberTypes.Property)
			{
				property.ShouldSerialize = (x) => false;
			}

			return property;
		}
	}
	internal static class SaveSerializer
	{
		private static JsonSerializerSettings settings { get; } = new JsonSerializerSettings() { ContractResolver = NoPropertiesContractResolver.Instance };

		public static string Serialize<T>(T value)
		{
			if (value is Component)
			{
				Debug.LogError($"Can't serialize type {typeof(T).Name}!");
				return "";
			}
			if(value is GameObject go)
			{
				if(value == null)
				{
					return JsonConvert.SerializeObject(new GameObjectReference(), settings);
				}
				GameObjectID id = go.GetComponent<GameObjectID>();
				if(id == null)
				{
					Debug.LogWarning("Trying to serialize a GameObject reference without GameObjectID component. Please add a GameObjectID to the GameObject in the scene.", go);
					return JsonConvert.SerializeObject(new GameObjectReference(), settings);
				}
				if(string.IsNullOrEmpty(id.id))
				{
					Debug.LogWarning("Trying to serialize a GameObject without an id. Is this an instantiated GameObject?", go);
					return JsonConvert.SerializeObject(new GameObjectReference(), settings);
				}

				GameObjectReference reference = new GameObjectReference() { id = id.id };

				return JsonConvert.SerializeObject(reference, settings);
			}

			return JsonConvert.SerializeObject(value, settings);
		}

		public static T Deserialize<T>(string serialized)
		{
			if (typeof(T).IsAssignableFrom(typeof(GameObject)))
			{
				object go;
				var reference = JsonConvert.DeserializeObject<GameObjectReference>(serialized, settings);
				go = reference.GetGameObject();
				return (T)go;
			}
			return JsonConvert.DeserializeObject<T>(serialized, settings);
		}


		public static void SerializeToStream(object value, Stream stream)
		{
			using (StreamWriter writer = new StreamWriter(stream))
			using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
			{
				JsonSerializer ser = new JsonSerializer() { ContractResolver = NoPropertiesContractResolver.Instance, Formatting = Formatting.Indented };
				ser.Serialize(jsonWriter, value);
				jsonWriter.Flush();
			}
		}

		public static T DeserializeFromStream<T>(Stream stream)
		{
			using (StreamReader reader = new StreamReader(stream))
			using (JsonTextReader jsonReader = new JsonTextReader(reader))
			{
				JsonSerializer ser = new JsonSerializer() { ContractResolver = NoPropertiesContractResolver.Instance };
				return ser.Deserialize<T>(jsonReader);
			}
		}
	}
}