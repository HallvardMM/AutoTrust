using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Runtime.Serialization;


namespace AutoTrust
{
  public class NugetPackage
  {
    public string? CatalogEntry { get; set; } // Interesting
    public bool Listed { get; set; } // Interesting
    public string? PackageContent { get; set; } // Interesting
    public string? Published { get; set; } // Interesting
    public string? Registration { get; set; }

    public static string GetManifestUrlFromPackageContentUrl(string PackageContentUrl)
    {
      return PackageContentUrl.Replace(".nupkg", ".nuspec");
    }

    public static string GetNugetPackageUrl(string packageName, string packageVersion)
    {
      return ($"https://api.nuget.org/v3/registration5-semver1/{packageName.ToLower()}/{packageVersion.ToLower()}.json");
    }
  }

  
}