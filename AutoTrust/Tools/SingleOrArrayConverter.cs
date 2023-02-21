using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;


// Inspired by: https://blog.bitscry.com/2017/08/31/single-or-array-json-converter/ 
namespace AutoTrust
{
	public class SingleOrArrayConverter<T> : JsonConverter<List<T>>
	{
		public override bool CanConvert(Type objectType)
		{
			return (objectType == typeof(List<T>));
		}

		public override List<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			using JsonDocument document = JsonDocument.ParseValue(ref reader);

			if (document.RootElement.ValueKind == JsonValueKind.Array)
			{
				return JsonSerializer.Deserialize<List<T>>(document.RootElement.GetRawText(), options);
			}

			return new List<T> { JsonSerializer.Deserialize<T>(document.RootElement.GetRawText(), options) };
		}

		public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options)
		{
			if (value.Count == 1)
			{
				JsonSerializer.Serialize(writer, value[0], options);
			}
			else
			{
				JsonSerializer.Serialize(writer, value, options);
			}
		}
	}
}