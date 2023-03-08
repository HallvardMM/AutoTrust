namespace AutoTrust;
using System.Text.Json;
using System.Text.Json.Serialization;

public class StringOrObjectConverter<T> : JsonConverter<T>
{
  public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
	using var document = JsonDocument.ParseValue(ref reader);

	if (document.RootElement.ValueKind == JsonValueKind.String)
	{
	  return JsonSerializer.Deserialize<T>($"\"{document.RootElement.GetString()}\"", options);
	}
	else
	{
	  return JsonSerializer.Deserialize<T>(document.RootElement.GetRawText(), options);
	}
  }

  public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
  {
	if (value is string)
	{
	  writer.WriteStringValue(value as string);
	}
	else
	{
	  JsonSerializer.Serialize(writer, value, options);
	}
  }
}
