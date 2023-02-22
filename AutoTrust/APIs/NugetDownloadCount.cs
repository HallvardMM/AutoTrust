using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace AutoTrust
{
  public class NugetDownloadCount
  {
    public int? TotalHits { get; set; }
    public List<NugetDownloadCountItem>? Data { get; set; }

    public static string GetNugetDownloadCountUrl(string packageName, string packageVersion)
    {
      return ($"https://azuresearch-usnc.nuget.org/query?q=packageid:{packageName.ToLower()}");
    }
  }

  public class NugetDownloadCountItem
  {
    [JsonPropertyName("@id")]
    public string? Id { get; set; }
    [JsonPropertyName("@type")]
    public string? Type { get; set; }
    public string? Registration { get; set; }
    [JsonPropertyName("Id")]
    public string? PackageName { get; set; }
    public string? Version { get; set; }
    public string? Description { get; set; }
    public string? Summary { get; set; }
    public string? Title { get; set; }
    public string? IconUrl { get; set; }
    public string? LicenseUrl { get; set; }
    public string? ProjectUrl { get; set; }
    public List<string>? Tags { get; set; }
    [JsonPropertyName("authors")]
    [JsonConverter(typeof(SingleOrArrayConverter<string>))]
    public List<string>? Authors { get; set; }
    public List<string>? Owners { get; set; }
    public long? TotalDownloads { get; set; }
    public bool? Verified { get; set; }
    public List<NugetDownloadCountPackageType>? PackageTypes { get; set; }
    public List<NugetDownloadCountVersion>? Versions { get; set; }
  }

  public class NugetDownloadCountPackageType
  {
    public string? Name { get; set; }
  }

  public class NugetDownloadCountVersion
  {
    public string? Version { get; set; }
    public long? Downloads { get; set; }
    [JsonPropertyName("@id")]
    public string? Id { get; set; }
  }

}