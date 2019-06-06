using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace SaveSystem
{

	public class ShouldSerializeContractResolver : DefaultContractResolver
	{
		public new static readonly ShouldSerializeContractResolver Instance = new ShouldSerializeContractResolver();

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
		private static JsonSerializerSettings settings { get; } = new JsonSerializerSettings() { ContractResolver = ShouldSerializeContractResolver.Instance };

		public static string Serialize<T>(T value)
		{
			if (value is Component)
			{
				Debug.LogError($"Can't serialize type {typeof(T).Name}!");
				return "";
			}

			return JsonConvert.SerializeObject(value, settings);
		}

		public static T Deserialize<T>(string serialized)
		{
			return JsonConvert.DeserializeObject<T>(serialized, settings);
		}


		public static void SerializeToStream(object value, Stream stream)
		{
			using (StreamWriter writer = new StreamWriter(stream))
			using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
			{
				JsonSerializer ser = new JsonSerializer() { ContractResolver = ShouldSerializeContractResolver.Instance, Formatting = Formatting.Indented };
				ser.Serialize(jsonWriter, value);
				jsonWriter.Flush();
			}
		}

		public static T DeserializeFromStream<T>(Stream stream)
		{
			using (StreamReader reader = new StreamReader(stream))
			using (JsonTextReader jsonReader = new JsonTextReader(reader))
			{
				JsonSerializer ser = new JsonSerializer() { ContractResolver = ShouldSerializeContractResolver.Instance };
				return ser.Deserialize<T>(jsonReader);
			}
		}
	}
}