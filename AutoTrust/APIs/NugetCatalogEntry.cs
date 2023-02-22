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

    public override string ToString()
    {
      string returnString = "";

      returnString += $"Id: {Id}\n";
      returnString += $"Type: [{string.Join(", ", Type)}]\n";
      returnString += $"PackageName: {PackageName}\n";
      returnString += $"Version: {Version}\n";
      returnString += $"Authors: [{string.Join(", ", Authors)}]\n";
      returnString += $"CommitId: {CommitId}\n";
      returnString += $"CommitTimeStamp: {CommitTimeStamp}\n";
      returnString += $"Copyright: {Copyright}\n";
      returnString += $"Created: {Created}\n";
      returnString += $"Description: {Description}\n";
      returnString += $"IconFile: {IconFile}\n";
      returnString += $"IconUrl: {IconUrl}\n";
      returnString += $"IsPrerelease: {IsPrerelease}\n";
      returnString += $"LastEdited: {LastEdited}\n";
      returnString += $"LicenseExpression: {LicenseExpression}\n";
      returnString += $"LicenseUrl: {LicenseUrl}\n";
      returnString += $"Listed: {Listed}\n";
      returnString += $"MinClientVersion: {MinClientVersion}\n";
      returnString += $"PackageHash: {PackageHash}\n";
      returnString += $"PackageHashAlgorithm: {PackageHashAlgorithm}\n";
      returnString += $"PackageSize: {PackageSize}\n";
      returnString += $"ProjectUrl: {ProjectUrl}\n";
      returnString += $"Published: {Published}\n";
      returnString += $"Repository: {Repository}\n";
      returnString += $"RequireLicenseAcceptance: {RequireLicenseAcceptance}\n";
      returnString += $"Title: {Title}\n";
      returnString += $"VerbatimVersion: {VerbatimVersion}\n";
      returnString += $"Tags: [{string.Join(", ", Tags)}]\n";
      if (Deprecation != null)
      {
        returnString += $"Deprecation:\n {Deprecation.ToString()}";
      }
      returnString += $"Language: {Language}\n";
      returnString += $"Summary: {Summary}\n";
      if(DependencyGroups != null)
      {
        returnString += $"Dependencies:\n";
        foreach (PackageDependencyGroup dependencyGroup in DependencyGroups)
        {
          returnString += dependencyGroup.ToString();
        }
      }
      if (PackageEntries != null) {
        returnString += $"Package entries:\n";
        foreach (PackageEntries packageEntry in PackageEntries)
        {
          returnString += packageEntry.ToString();
        } 
      }
      if(Vulnerabilities != null)
      {
        returnString += $"Vulnerabilities:\n";
        foreach (Vulnerabilities vulnerability in Vulnerabilities)
        {
          returnString += vulnerability.ToString();
        }
      }
      return returnString;
    }
  }

  public class PackageDependencyGroup
  {
    [JsonPropertyName("@id")]
    public string Id { get; set; }

    [JsonPropertyName("@type")]
    public string Type { get; set; }

    public List<PackageDependency> Dependencies { get; set; }

    public string TargetFramework { get; set; }

    public override string ToString()
    {

      string returnString = $"Target framework: {TargetFramework}\n";
      if(Dependencies != null)
      {
        foreach (PackageDependency dependency in Dependencies)
        {
          returnString += dependency.ToString();
        }
      }
      return returnString;
    }
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

    public override string ToString()
    {
      return $"Package name: {PackageName}, Range: {Range}\n";
    }
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

    // Write toString
    public override string ToString()
    {
      string returnString = $"Package name: {Name}\n";
      returnString += $"Package full name: {FullName}\n";
      returnString += $"Package length: {Length}\n";
      returnString += $"Package compressed length: {CompressedLength}\n";
      return returnString;
    }
  }

  public class Vulnerabilities
  {
    [JsonPropertyName("@id")]
    public string Id { get; set; }

    [JsonPropertyName("@type")]
    public string Type { get; set; }

    public string AdvisoryUrl { get; set; }

    public string Severity { get; set; }

    public override string ToString()
    {
      return $"Type: {Type}, Severity: {Severity}, Advisory: {AdvisoryUrl}\n";
    }


  }

  public class Deprecation
  {
    [JsonPropertyName("@id")]
    public string Id { get; set; }
    public string? Message { get; set; }
    public List<string> Reasons { get; set; }
    public AlternatePackage AlternatePackage { get; set; }
    
    public override string ToString()
    {
      string returnString = $"Message: {Message}, Reasons: [{string.Join(", ", Reasons)}]";
      if(AlternatePackage != null)
      {
        returnString += $", AlternatePackage: [{ AlternatePackage.ToString()}]\n";
      }
      return returnString;
    }
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
    public override string ToString()
    {
      return $"Name: {AlternatePackageName}, Range: {Range}";
    }
  }

}

