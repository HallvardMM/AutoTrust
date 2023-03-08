using System.Text.Json.Serialization;


namespace AutoTrust
{
  public class NugetPackage
  {
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

    public static string GetNugetPackageUrl(string packageName, string packageVersion)
    {
      return ($"https://api.nuget.org/v3/registration5-semver1/{packageName.ToLower()}/{packageVersion.ToLower()}.json");
    }

    public override string ToString()
    {
      string returnString = "";
      if (!Listed)
      {
        returnString += "Warning: Package not listed!\n";
      }
      if (Published != null)
      {
        returnString += $"Latest version of package published: {Published.ToString()}";
      }
      else
      {
        returnString += "Warning: No published date found!";
      }

      return returnString;
    }
  }
}