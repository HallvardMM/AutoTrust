using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutoTrust
{
  public class NugetCatalogEntry
  {
    // https://learn.microsoft.com/en-us/nuget/api/catalog-resource#catalog-leaf
    [JsonPropertyName("@id")]
    public string? Id { get; set; }
    [JsonConverter(typeof(SingleOrArrayConverter<string>))]
    [JsonPropertyName("@type")]
    public List<string>? Type { get; set; }
    [JsonPropertyName("id")]
    public string PackageName { get; set; }
    public string Version { get; set; }
    [JsonPropertyName("authors")]
    [JsonConverter(typeof(SingleOrArrayConverter<string>))]
    public List<string>? Authors { get; set; } 
    [JsonPropertyName("catalog:commitId")]
    public string? CommitId { get; set; }
    [JsonPropertyName("catalog:commitTimeStamp")]
    public DateTimeOffset? CommitTimeStamp { get; set; }
    public string? Copyright { get; set; }
    public DateTimeOffset? Created { get; set; }
    public string? Description { get; set; }
    public string? IconFile { get; set; }
    public string? IconUrl { get; set; }
    public bool? IsPrerelease { get; set; }
    public DateTimeOffset? LastEdited { get; set; }
    public string? LicenseExpression { get; set; }
    public string? LicenseUrl { get; set; }
    public bool? Listed { get; set; }
    public string? MinClientVersion { get; set; }
    public string? PackageHash { get; set; }
    public string? PackageHashAlgorithm { get; set; }
    public int? PackageSize { get; set; }
    public string? ProjectUrl { get; set; }
    public DateTimeOffset? Published { get; set; }
    public string? Repository { get; set; }
    public bool? RequireLicenseAcceptance { get; set; }
    public string? Title { get; set; }
    public string? VerbatimVersion { get; set; }
    public List<PackageDependencyGroup>? DependencyGroups { get; set; }
    public List<PackageEntries>? PackageEntries { get; set; }
    [JsonPropertyName("tags")]
    [JsonConverter(typeof(SingleOrArrayConverter<string>))]
    public List<string>? Tags { get; set; }
    public List<Vulnerabilities>? Vulnerabilities { get; set; }
    public Deprecation? Deprecation { get; set; }
    public string? Language { get; set; }
    public string? Summary { get; set; }
  }

  public class PackageDependencyGroup
  {
    [JsonPropertyName("@id")]
    public string Id { get; set; }

    [JsonPropertyName("@type")]
    public string Type { get; set; }

    public List<PackageDependency> Dependencies { get; set; }

    public string TargetFramework { get; set; }
  }

  public class PackageDependency
  {
    [JsonPropertyName("@id")]
    public string Id { get; set; }

    [JsonPropertyName("@type")]
    public string Type { get; set; }

    [JsonPropertyName("id")]
    public string PackageName { get; set; }

    public string Range { get; set; }
  }

  public class PackageEntries
  {
    [JsonPropertyName("@id")]
    public string Id { get; set; }

    [JsonPropertyName("@type")]
    public string Type { get; set; }

    public int CompressedLength { get; set; }

    public string FullName { get; set; }

    public int Length { get; set; }

    public string Name { get; set; }
  }

  public class Vulnerabilities
  {
    [JsonPropertyName("@id")]
    public string Id { get; set; }

    [JsonPropertyName("@type")]
    public string Type { get; set; }

    public string AdvisoryUrl { get; set; }

    public string Severity { get; set; }
  }

  public class Deprecation
  {
    [JsonPropertyName("@id")]
    public string Id { get; set; }
    public string? Message { get; set; }
    public List<string> Reasons { get; set; }
    public AlternatePackage AlternatePackage { get; set; }  
  }

  public class AlternatePackage
  {
    [JsonPropertyName("@id")]
    public string Id { get; set; }
    [JsonPropertyName("id")]
    public string AlternatePackageName { get; set; }
    [JsonPropertyName("range")]
    [JsonConverter(typeof(StringOrObjectConverter<string>))]
    public string? Range { get; set; } //TODO: Can be an object or "*" this needs more testing
    
  }

}

