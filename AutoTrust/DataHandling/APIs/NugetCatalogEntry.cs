// JSON object properties description of Catalog leaf:
// https://learn.microsoft.com/en-us/nuget/api/catalog-resource#catalog-leaf

namespace AutoTrust;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

public class NugetCatalogEntry {
  [JsonPropertyName("@id")]
  public required string Id { get; set; }
  [JsonConverter(typeof(SingleOrArrayConverter<string>))]
  [JsonPropertyName("@type")]
  public required List<string> Type { get; set; }
  [JsonPropertyName("id")]
  public required string PackageName { get; set; }
  public required string Version { get; set; }
  [JsonPropertyName("authors")]
  [JsonConverter(typeof(SingleOrArrayConverter<string>))]
  public List<string> Authors { get; set; } = new List<string>();
  [JsonPropertyName("catalog:commitId")]
  public required string CommitId { get; set; }
  [JsonPropertyName("catalog:commitTimeStamp")]
  public required DateTimeOffset CommitTimeStamp { get; set; }
  public string Copyright { get; set; } = string.Empty;
  public DateTimeOffset Created { get; set; }
  public string Description { get; set; } = string.Empty;
  public string IconFile { get; set; } = string.Empty;
  public string IconUrl { get; set; } = string.Empty;
  public bool IsPrerelease { get; set; }
  public DateTimeOffset LastEdited { get; set; }
  public string LicenseExpression { get; set; } = string.Empty;
  public string LicenseUrl { get; set; } = string.Empty;
  public bool Listed { get; set; }
  public string MinClientVersion { get; set; } = string.Empty;
  public string PackageHash { get; set; } = string.Empty;
  public string PackageHashAlgorithm { get; set; } = string.Empty;
  public int PackageSize { get; set; }
  public string ProjectUrl { get; set; } = string.Empty;
  public required DateTimeOffset Published { get; set; }
  public string Repository { get; set; } = string.Empty;
  public bool RequireLicenseAcceptance { get; set; }
  public string Title { get; set; } = string.Empty;
  public string VerbatimVersion { get; set; } = string.Empty;
  public List<PackageDependencyGroup>? DependencyGroups { get; set; }
  public List<PackageEntries>? PackageEntries { get; set; }
  [JsonPropertyName("tags")]
  [JsonConverter(typeof(SingleOrArrayConverter<string>))]
  public List<string> Tags { get; set; } = new List<string>();
  public List<Vulnerabilities>? Vulnerabilities { get; set; }
  public Deprecation? Deprecation { get; set; }
  public string Language { get; set; } = string.Empty;
  public string Summary { get; set; } = string.Empty;


  public static async Task<NugetCatalogEntry?> GetNugetCatalogEntry(HttpClient httpClient, string catalogEntryUrl, bool isDiagnostic) {
    try {
      // Fetch package data
      var nugetCatalogEntry = await httpClient.GetFromJsonAsync<NugetCatalogEntry>(catalogEntryUrl);
      if (isDiagnostic) {
        Console.WriteLine($"Found catalog entry from {catalogEntryUrl}");
      }
      return nugetCatalogEntry;
    }
    catch (HttpRequestException ex) {
      // Handle any exceptions thrown by the HTTP client.
      if (isDiagnostic) {
        Console.WriteLine($"Error: An HTTP error occurred from {catalogEntryUrl}: {ex.Message}");
      }
    }
    catch (JsonException ex) {
      // Handle any exceptions thrown during JSON deserialization.
      if (isDiagnostic) {
        Console.WriteLine($"Error: A JSON error occurred from {catalogEntryUrl}: {ex.Message}");
      }
    }
    return null;
  }

  public override string ToString() {
    var returnString = "";

    returnString += $"Id: {this.Id}\n";
    returnString += $"Type: [{string.Join(", ", this.Type)}]\n";
    returnString += $"PackageName: {this.PackageName}\n";
    returnString += $"Version: {this.Version}\n";
    returnString += $"Authors: [{string.Join(", ", this.Authors)}]\n";
    returnString += $"CommitId: {this.CommitId}\n";
    returnString += $"CommitTimeStamp: {this.CommitTimeStamp}\n";
    returnString += $"Copyright: {this.Copyright}\n";
    returnString += $"Created: {this.Created}\n";
    returnString += $"Description: {this.Description}\n";
    returnString += $"IconFile: {this.IconFile}\n";
    returnString += $"IconUrl: {this.IconUrl}\n";
    returnString += $"IsPrerelease: {this.IsPrerelease}\n";
    returnString += $"LastEdited: {this.LastEdited}\n";
    returnString += $"LicenseExpression: {this.LicenseExpression}\n";
    returnString += $"LicenseUrl: {this.LicenseUrl}\n";
    returnString += $"Listed: {this.Listed}\n";
    returnString += $"MinClientVersion: {this.MinClientVersion}\n";
    returnString += $"PackageHash: {this.PackageHash}\n";
    returnString += $"PackageHashAlgorithm: {this.PackageHashAlgorithm}\n";
    returnString += $"PackageSize: {this.PackageSize}\n";
    returnString += $"ProjectUrl: {this.ProjectUrl}\n";
    returnString += $"Published: {this.Published}\n";
    returnString += $"Repository: {this.Repository}\n";
    returnString += $"RequireLicenseAcceptance: {this.RequireLicenseAcceptance}\n";
    returnString += $"Title: {this.Title}\n";
    returnString += $"VerbatimVersion: {this.VerbatimVersion}\n";
    returnString += $"Tags: [{string.Join(", ", this.Tags)}]\n";
    if (this.Deprecation != null) {
      returnString += $"Deprecation:\n {this.Deprecation}";
    }
    returnString += $"Language: {this.Language}\n";
    returnString += $"Summary: {this.Summary}\n";
    if (this.DependencyGroups != null) {
      returnString += $"Dependencies:\n";
      foreach (var dependencyGroup in this.DependencyGroups) {
        returnString += dependencyGroup.ToString();
      }
    }
    if (this.PackageEntries != null) {
      returnString += $"Package entries:\n";
      foreach (var packageEntry in this.PackageEntries) {
        returnString += packageEntry.ToString();
      }
    }
    if (this.Vulnerabilities != null) {
      returnString += $"Vulnerabilities:\n";
      foreach (var vulnerability in this.Vulnerabilities) {
        returnString += vulnerability.ToString();
      }
    }
    return returnString;
  }
}

public class PackageDependencyGroup {
  [JsonPropertyName("@id")]
  public string Id { get; set; } = string.Empty;

  [JsonPropertyName("@type")]
  public string Type { get; set; } = string.Empty;

  public List<PackageDependency> Dependencies { get; set; } = new List<PackageDependency>();

  public string TargetFramework { get; set; } = string.Empty;

  public override string ToString() {

    var returnString = $"Target framework: {this.TargetFramework}\n";
    if (this.Dependencies != null) {
      foreach (var dependency in this.Dependencies) {
        returnString += dependency.ToString();
      }
    }
    return returnString;
  }
}

public class PackageDependency {
  [JsonPropertyName("@id")]
  public string Id { get; set; } = string.Empty;

  [JsonPropertyName("@type")]
  public string Type { get; set; } = string.Empty;

  [JsonPropertyName("id")]
  public string PackageName { get; set; } = string.Empty;

  public string Range { get; set; } = string.Empty;

  public override string ToString() => $"Package name: {this.PackageName}, Range: {this.Range}\n";
}

public class PackageEntries {
  [JsonPropertyName("@id")]
  public string Id { get; set; } = string.Empty;

  [JsonPropertyName("@type")]
  public string Type { get; set; } = string.Empty;

  public int CompressedLength { get; set; }

  public string FullName { get; set; } = string.Empty;

  public int Length { get; set; }

  public string Name { get; set; } = string.Empty;

  // Write toString
  public override string ToString() {
    var returnString = $"Package name: {this.Name}\n";
    returnString += $"Package full name: {this.FullName}\n";
    returnString += $"Package length: {this.Length}\n";
    returnString += $"Package compressed length: {this.CompressedLength}\n";
    return returnString;
  }
}

public class Vulnerabilities {
  [JsonPropertyName("@id")]
  public string Id { get; set; } = string.Empty;

  [JsonPropertyName("@type")]
  public string Type { get; set; } = string.Empty;

  public required string AdvisoryUrl { get; set; }

  public required string Severity { get; set; }

  public override string ToString() => $"Type: {this.Type}, Severity: {this.Severity}, Advisory: {this.AdvisoryUrl}\n";


}

public class Deprecation {
  [JsonPropertyName("@id")]
  public required string Id { get; set; }
  public string Message { get; set; } = string.Empty;
  public required List<string> Reasons { get; set; }
  public AlternatePackage? AlternatePackage { get; set; }

  public override string ToString() {
    var returnString = $"Message: {this.Message}, Reasons: [{string.Join(", ", this.Reasons)}]";
    if (this.AlternatePackage != null) {
      returnString += $", AlternatePackage: [{this.AlternatePackage}]\n";
    }
    return returnString;
  }
}

public class AlternatePackage {
  [JsonPropertyName("@id")]
  public required string Id { get; set; }
  [JsonPropertyName("id")]
  public required string AlternatePackageName { get; set; }
  [JsonPropertyName("range")]
  [JsonConverter(typeof(StringOrObjectConverter<string>))]
  public string Range { get; set; } = string.Empty;
  public override string ToString() => $"Name: {this.AlternatePackageName}, Range: {this.Range}";
}

