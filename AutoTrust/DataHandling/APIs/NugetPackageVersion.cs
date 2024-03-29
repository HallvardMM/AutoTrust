namespace AutoTrust;
using System.Net.Http.Json;
using System.Text.Json;

public class NugetPackageVersion {
  public List<string> Versions { get; set; } = new List<string>();

  public static string GetVersionsUrl(string packageName) => $"https://api.nuget.org/v3-flatcontainer/{packageName.ToLowerInvariant()}/index.json";

  public static async Task<(string?, string?)> GetLatestVersion(HttpClient httpClient, string packageName, bool prerelease, bool isDiagnostic) {
    try {

      // Fetch all versions data
      var allVersionsForPackageObject = await httpClient.GetFromJsonAsync<NugetPackageVersion>
        (GetVersionsUrl(packageName));


      if (allVersionsForPackageObject?.Versions != null) {

        if (prerelease) {
          if (isDiagnostic) {
            Console.WriteLine($"\rFound versions for {packageName} (including prerelease versions) from {GetVersionsUrl(packageName)}");
          }
          return (FilterOldestStableVersion(allVersionsForPackageObject.Versions), FilterLatestVersion(allVersionsForPackageObject.Versions));
        }
        if (isDiagnostic) {
          Console.WriteLine($"\rFound versions for {packageName} from {GetVersionsUrl(packageName)}");
        }
        return (FilterOldestStableVersion(allVersionsForPackageObject.Versions), FilterLatestStableVersion(allVersionsForPackageObject.Versions));
      }
    }
    catch (HttpRequestException ex) {
      // Handle any exceptions thrown by the HTTP client.
      if (isDiagnostic) {
        Console.WriteLine($"Error: An HTTP error occurred from {GetVersionsUrl(packageName)}: {ex.Message}");
      }
    }
    catch (JsonException ex) {
      // Handle any exceptions thrown during JSON deserialization.
      if (isDiagnostic) {
        Console.WriteLine($"Error: A JSON error occurred from {GetVersionsUrl(packageName)}: {ex.Message}");
      }
    }
    return (null, null);
  }

  public static string? FilterLatestStableVersion(List<string> versions) {
    for (var i = versions.Count - 1; i >= 0; i--) {
      if (!versions[i].Contains('-') && !versions[i].Contains("alpha") && !versions[i].Contains("beta")) {
        return versions[i];
      }
    }
    return null;
  }

  public static string FilterLatestVersion(List<string> versions) => versions.Last();

  public static string? FilterOldestStableVersion(List<string> versions) {
    foreach (var version in versions) {
      if (version.First() != '0' && !version.Contains('-') && !version.Contains("alpha") && !version.Contains("beta")) {
        return version;
      }
    }
    return null;
  }

  public override string ToString() => $"[{string.Join(", ", this.Versions)}]";

}
