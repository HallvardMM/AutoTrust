// Inspired by: https://blog.bitscry.com/2017/08/31/single-or-array-json-converter/ 
namespace AutoTrust;
using System.Text.Json;
using System.Text.Json.Serialization;

public class SingleOrArrayConverter<T> : JsonConverter<List<T>>
{
  public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(List<T>);

  public override List<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
	using var document = JsonDocument.ParseValue(ref reader);

	if (document.RootElement.ValueKind == JsonValueKind.Array)
	{
	  var deserializeList = JsonSerializer.Deserialize<List<T>>(document.RootElement.GetRawText(), options);
	  return deserializeList ?? new List<T>();
	}
	var deserialized = JsonSerializer.Deserialize<T>(document.RootElement.GetRawText(), options);
	return deserialized is not null ? new List<T> { deserialized } : new List<T>();
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
