// Nuget Search JSON properties description:
// https://learn.microsoft.com/en-us/nuget/api/search-query-service-resource

namespace AutoTrust;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

public class NugetDownloadCount {
  public required int TotalHits { get; set; }
  public required List<NugetDownloadCountItem> Data { get; set; } = new List<NugetDownloadCountItem>();

  public static string GetNugetDownloadCountUrl(string packageName, bool prerelease = false) => $"https://azuresearch-usnc.nuget.org/query?q=packageid:{packageName.ToLower(System.Globalization.CultureInfo.InvariantCulture)}&prerelease={prerelease}";
  public static async Task<NugetDownloadCount?> GetNugetDownloadCount(HttpClient httpClient, string packageName, bool prerelease = false) {
    try {
      // Fetch package data
      var getNugetDownloadCount = await httpClient.GetFromJsonAsync<NugetDownloadCount>(GetNugetDownloadCountUrl(packageName, prerelease));
      return getNugetDownloadCount;
    }
    catch (HttpRequestException ex) {
      // Handle any exceptions thrown by the HTTP client.
      Console.WriteLine($"An HTTP error occurred: {ex.Message}");
    }
    catch (JsonException ex) {
      // Handle any exceptions thrown during JSON deserialization.
      Console.WriteLine($"A JSON error occurred: {ex.Message}");
    }
    return null;
  }

  public string ToString(string packageVersion) {
    if (this.TotalHits == 1) {
      return this.Data[0].ToString(packageVersion);
    }
    else {
      return "Warning: More than one package fits the Id!";
    }
  }
}

public class NugetDownloadCountItem {
  [JsonPropertyName("@id")]
  public required string Id { get; set; }
  [JsonPropertyName("@type")]
  public string Type { get; set; } = string.Empty;
  public string Registration { get; set; } = string.Empty;
  [JsonPropertyName("Id")]
  public required string PackageName { get; set; }
  public required string Version { get; set; }
  public string Description { get; set; } = string.Empty;
  public string Summary { get; set; } = string.Empty;
  public string Title { get; set; } = string.Empty;
  public string IconUrl { get; set; } = string.Empty;
  public string LicenseUrl { get; set; } = string.Empty;
  public string ProjectUrl { get; set; } = string.Empty;
  public List<string> Tags { get; set; } = new List<string>();
  [JsonPropertyName("authors")]
  [JsonConverter(typeof(SingleOrArrayConverter<string>))]
  public List<string> Authors { get; set; } = new List<string>();
  public List<string> Owners { get; set; } = new List<string>();
  public long TotalDownloads { get; set; }
  public bool Verified { get; set; }
  public required List<NugetDownloadCountPackageType> PackageTypes { get; set; }
  public required List<NugetDownloadCountVersion> Versions { get; set; }

  public string ToString(string packageVersion) {
    var returnString = "";

    returnString += $"Total downloads for package: {this.TotalDownloads}\n";

    for (var i = 0; i < this.Versions?.Count; i++) {
      if (this.Versions[i]?.Version == packageVersion) {
        returnString += $"Total downloads for version {this.Versions[i]?.Version}: {this.Versions[i]?.Downloads}\n";
        if (this.Versions[i]?.Downloads != 0) {
          returnString += $"Total downloads/Version downloads: {this.TotalDownloads / this.Versions[i]?.Downloads}";
          return returnString;
        }
        else {
          returnString += "Warning: No downloads for this package!";
          return returnString;
        }
      }
    }
    returnString += "Warning: Did not find download statistics for package!";
    return returnString;
  }
}

public class NugetDownloadCountPackageType {
  public required string Name { get; set; }
}

public class NugetDownloadCountVersion {
  public required string Version { get; set; }
  public required long Downloads { get; set; }
  [JsonPropertyName("@id")]
  public required string Id { get; set; }
}
