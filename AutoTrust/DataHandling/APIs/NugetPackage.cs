namespace AutoTrust;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

public class NugetPackage {
  // https://learn.microsoft.com/en-us/nuget/api/registration-base-url-resource#registration-leaf
  [JsonPropertyName("@id")]
  public required string Id { get; set; }
  [JsonPropertyName("@type")]
  public List<string> Type { get; set; } = new List<string>();
  public string CatalogEntry { get; set; } = string.Empty;
  public bool Listed { get; set; }
  public string PackageContent { get; set; } = string.Empty;
  public DateTimeOffset? Published { get; set; }
  public string Registration { get; set; } = string.Empty;

  public static string GetNugetPackageUrl(string packageName, string packageVersion) => $"https://api.nuget.org/v3/registration5-semver1/{packageName.ToLower(System.Globalization.CultureInfo.CurrentCulture)}/{packageVersion.ToLower(System.Globalization.CultureInfo.CurrentCulture)}.json";

  public static async Task<NugetPackage?> GetNugetPackage(HttpClient httpClient, string packageName, string packageVersion) {
    try {
      // Fetch package data
      var nugetPackage = await httpClient.GetFromJsonAsync<NugetPackage>
          (GetNugetPackageUrl(packageName, packageVersion));
      return nugetPackage;
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

  public override string ToString() {
    var returnString = "";
    if (!this.Listed) {
      returnString += "Warning: Package not listed!\n";
    }
    if (this.Published != null) {
      returnString += $"Latest version of package published: {this.Published}";
    }
    else {
      returnString += "Warning: No published date found!";
    }

    return returnString;
  }
}
