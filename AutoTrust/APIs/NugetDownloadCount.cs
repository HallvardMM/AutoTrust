using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;


namespace AutoTrust
{
  public class NugetDownloadCount
  {
    public int? TotalHits { get; set; }
    public List<NugetDownloadCountItem>? Data { get; set; }
  }

  public class NugetDownloadCountItem
  {
    public string? Registration { get; set; }
    public string? Id { get; set; }
    public string? Version { get; set; }
    public string? Description { get; set; }
    public string? Summary { get; set; }
    public string? Title { get; set; }
    public string? IconUrl { get; set; }
    public string? LicenseUrl { get; set; }
    public string? ProjectUrl { get; set; }
    public List<string>? Tags { get; set; }
    public List<string>? Authors { get; set; }
    public List<string>? Owners { get; set; }
    public int? TotalDownloads { get; set; }
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
    public int? Downloads { get; set; }
    public string? Id { get; set; }
  }

}