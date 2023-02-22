using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace AutoTrust
{
  public class NugetPackage
  {
    // https://learn.microsoft.com/en-us/nuget/api/registration-base-url-resource#registration-leaf
    [JsonPropertyName("@id")]
    public string? Id { get; set; }
    [JsonPropertyName("@type")]
    public List<string>? Type { get; set; }
    public string? CatalogEntry { get; set; }
    public bool Listed { get; set; } 
    public string? PackageContent { get; set; } 
    public DateTimeOffset? Published { get; set; } 
    public string? Registration { get; set; }

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