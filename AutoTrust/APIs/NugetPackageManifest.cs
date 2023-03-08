using System.Xml;
using System.Xml.Serialization;

//TODO: Evaluate which properties are valuable and needed
// Documentation for required properties: https://learn.microsoft.com/en-us/nuget/reference/nuspec

namespace AutoTrust
{
  [XmlRoot(ElementName = "package", Namespace = "http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd")]
  public class NugetPackageManifest
  {
    [XmlElement(ElementName = "metadata")]
    public required Metadata Metadata { get; set; }

    public async static Task<NugetPackageManifest?> GetNugetPackageManifest(HttpClient httpClient, string packageName, string packageVersion)
    {
      try
      {
        Stream stream = await httpClient.GetStreamAsync(NugetPackageManifest.GetNugetPackageManifestUrl(packageName, packageVersion));

        // Deserialize the XML file into a NuGetPackage object
        XmlSerializer serializer = new XmlSerializer(typeof(NugetPackageManifest));
        NugetPackageManifest? packageManifest = (NugetPackageManifest?)serializer.Deserialize(stream);
        return packageManifest;
      }
      catch (HttpRequestException ex)
      {
        // Handle any exceptions thrown by the HTTP client.
        Console.WriteLine($"An HTTP error occurred: {ex.Message}");
      }
      catch (InvalidOperationException ex)
      {
        // Handle any exceptions thrown during XML deserialization.
        Console.WriteLine($"An XML error occurred: {ex.Message}");
      }
      return null;
    }

    public static string GetNugetPackageManifestUrl(string packageName, string packageVersion)
    {
      return ($"https://api.nuget.org/v3-flatcontainer/{packageName.ToLower()}/{packageVersion.ToLower()}/{packageName.ToLower()}.nuspec");
    }
    
    public override string ToString()
    {
      return Metadata.ToString();
    }
  }
  
  [XmlRoot(ElementName = "metadata", Namespace = "http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd")]
    public class Metadata
  {
    [XmlElement(ElementName = "id")]
    public required string Id { get; set; }
    [XmlElement(ElementName = "version")]
    public required string Version { get; set; } 
    [XmlElement(ElementName = "title")]
    public string Title { get; set; } = string.Empty;
    [XmlElement(ElementName = "authors")]
    public required string Authors { get; set; }
    [XmlElement(ElementName = "license")]
    public License? License { get; set; }
    [XmlElement(ElementName = "licenseUrl")]
    public string LicenseUrl { get; set; } = string.Empty;
    [XmlElement(ElementName = "icon")]
    public string Icon { get; set; } = string.Empty;
    [XmlElement(ElementName = "readme")]
    public string Readme { get; set; } = string.Empty;
    [XmlElement(ElementName = "projectUrl")]
    public string ProjectUrl { get; set; } = string.Empty;
    [XmlElement(ElementName = "iconUrl")]
    public string IconUrl { get; set; } = string.Empty;
    [XmlElement(ElementName = "description")]
    public required string Description { get; set; } 
    [XmlElement(ElementName = "copyright")]
    public string Copyright { get; set; } = string.Empty;
    [XmlElement(ElementName = "tags")]
    public string Tags { get; set; } = string.Empty;
    [XmlElement(ElementName = "repository")]
    public Repository? Repository { get; set; }
    [XmlElement(ElementName = "dependencies")]
    public Dependencies? Dependencies { get; set; }
    [XmlAttribute(AttributeName = "minClientVersion")]
    public string MinClientVersion { get; set; } = string.Empty;

    public override string ToString()
    {
      string returnString = "";

      returnString += $"Package Id: {Id}\n";
      returnString += $"Package Version: {Version}\n";
      returnString += $"Package Title: {Title}\n";
      returnString += $"Package Authors: {Authors}\n";
      returnString += $"Package License: {License}\n";
      returnString += $"Package LicenseUrl: {LicenseUrl}\n";
      returnString += $"Package Icon: {Icon}\n";
      returnString += $"Package Readme: {Readme}\n";
      returnString += $"Package ProjectUrl: {ProjectUrl}\n";
      returnString += $"Package IconUrl: {IconUrl}\n";
      returnString += $"Package Description: {Description}\n";
      returnString += $"Package Copyright: {Copyright}\n";
      returnString += $"Package Tags: {Tags}\n";
      returnString += $"Package MinClientVersion: {MinClientVersion}\n";
      if (Repository != null)
      {
        returnString += $"Package Repository: {Repository.Url}\n";
        if (Repository.Commit != null)
        {
          returnString += $"Package Commit: {Repository.Url}/commit/{Repository.Commit}\n";
        }
      }
      if (Dependencies != null) {
        foreach (var group in Dependencies.Group)
        {
          returnString += $"Package Dependency Target Framework: {group.TargetFramework}\n";
          foreach (var dependency in group.Dependency)
          {
            returnString += $"Dependency: {dependency.Id}, Version: {dependency.Version}, Exclude: {dependency.Exclude}\n";
          }
        }
      }
      return returnString;
    }
  }

  public class License
  {
    [XmlAttribute(AttributeName = "type")]
    public string Type { get; set; } = string.Empty;
    [XmlText]
    public string Value { get; set; } = string.Empty;
  }

  public class Repository
  {
    [XmlAttribute(AttributeName = "type")]
    public string Type { get; set; } = string.Empty;
    [XmlAttribute(AttributeName = "url")]
    public string Url { get; set; } = string.Empty;
    [XmlAttribute(AttributeName = "branch")]
    public string Branch { get; set; } = string.Empty;
    [XmlAttribute(AttributeName = "commit")]
    public string Commit { get; set; } = string.Empty;
  }

  public class Dependencies 
  {
    //TODO: Dependencies don't have to be in a group, so we need to add support for that as well
    [XmlElement(ElementName = "group")]
    public List<Group> Group { get; set; } = new List<Group>();
  }

  public class Group
  {
    [XmlAttribute(AttributeName = "targetFramework")]
    public string TargetFramework { get; set; } = string.Empty;
    [XmlElement(ElementName = "dependency")]
    public List<Dependency> Dependency { get; set; }  = new List<Dependency>();
  }

  public class Dependency
  {
    [XmlAttribute(AttributeName = "id")]
    public string Id { get; set; } = string.Empty;
    [XmlAttribute(AttributeName = "version")]
    public string Version { get; set; } = string.Empty;
    [XmlAttribute(AttributeName = "exclude")]
    public string Exclude { get; set; } = string.Empty;
  }

}