using System.Text.Json;
using System.Net.Http.Json;

namespace AutoTrust
{
  public class NugetPackageVersion
  {
    public List<string> Versions { get; set; } = new List<string>();

    public static string GetVersionsUrl(string packageName)
    {
     return ($"https://api.nuget.org/v3-flatcontainer/{packageName.ToLower()}/index.json");
    }

    public async static Task<string?> GetLatestStableVersion(HttpClient httpClient, string packageName)
    {
      try
      {

        // Fetch all versions data
        NugetPackageVersion? allVersionsForPackageObject = await httpClient.GetFromJsonAsync<NugetPackageVersion>
          (NugetPackageVersion.GetVersionsUrl(packageName));
        if (allVersionsForPackageObject?.Versions != null) {
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
      for (int i = versions.Count - 1; i >= 0; i--)
      {
        if (!versions[i].Contains("-"))
        {
          return versions[i];
        }
      }
      return null; // TODO: Should maybe return an error?
    }

    public override string ToString()
    {
      return $"[{string.Join(", ", Versions)}]";
    }

  }

}