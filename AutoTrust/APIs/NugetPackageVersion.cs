namespace AutoTrust;
using System.Net.Http.Json;
using System.Text.Json;

public class NugetPackageVersion
{
  public List<string> Versions { get; set; } = new List<string>();

  public static string GetVersionsUrl(string packageName) => $"https://api.nuget.org/v3-flatcontainer/{packageName.ToLower(System.Globalization.CultureInfo.CurrentCulture)}/index.json";

  public static async Task<string?> GetLatestStableVersion(HttpClient httpClient, string packageName)
  {
	try
	{

	  // Fetch all versions data
	  var allVersionsForPackageObject = await httpClient.GetFromJsonAsync<NugetPackageVersion>
		(GetVersionsUrl(packageName));
	  if (allVersionsForPackageObject?.Versions != null)
	  {
		return FilterLatestStableVersion(allVersionsForPackageObject.Versions);
	  }
	}
	catch (HttpRequestException ex)
	{
	  // Handle any exceptions thrown by the HTTP client.
	  Console.WriteLine($"An HTTP error occurred: {ex.Message}");
	}
	catch (JsonException ex)
	{
	  // Handle any exceptions thrown during JSON deserialization.
	  Console.WriteLine($"A JSON error occurred: {ex.Message}");
	}
	return null;
  }

  public static string? FilterLatestStableVersion(List<string> versions)
  {
	for (var i = versions.Count - 1; i >= 0; i--)
	{
	  if (!versions[i].Contains('-'))
	  {
		return versions[i];
	  }
	}
	return null; // TODO: Should maybe return an error?
  }

  public override string ToString() => $"[{string.Join(", ", this.Versions)}]";

}
