using System;
using System.Xml.Serialization;
using System.Collections.Generic;

//TODO: Evaluate which fields are valuable and needed

namespace AutoTrust
{
  [XmlRoot(ElementName = "package", Namespace = "http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd")]
  public class NugetPackageManifest
  {
    [XmlElement(ElementName = "metadata")]
    public Metadata Metadata { get; set; }

    public static string GetNugetPackageManifestUrl(string packageName, string packageVersion)
    {
      return ($"https://api.nuget.org/v3-flatcontainer/{packageName.ToLower()}/{packageVersion.ToLower()}/{packageName.ToLower()}.nuspec");
    }
  }
  
  [XmlRoot(ElementName = "metadata", Namespace = "http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd")]
    public class Metadata
  {
    [XmlElement(ElementName = "id")]
    public string Id { get; set; }
    [XmlElement(ElementName = "version")]
    public string Version { get; set; }
    [XmlElement(ElementName = "title")]
    public string Title { get; set; }
    [XmlElement(ElementName = "authors")]
    public string Authors { get; set; }
    [XmlElement(ElementName = "license")]
    public License License { get; set; }
    [XmlElement(ElementName = "licenseUrl")]
    public string LicenseUrl { get; set; }
    [XmlElement(ElementName = "icon")]
    public string Icon { get; set; }
    [XmlElement(ElementName = "readme")]
    public string Readme { get; set; }
    [XmlElement(ElementName = "projectUrl")]
    public string ProjectUrl { get; set; }
    [XmlElement(ElementName = "iconUrl")]
    public string IconUrl { get; set; }
    [XmlElement(ElementName = "description")]
    public string Description { get; set; }
    [XmlElement(ElementName = "copyright")]
    public string Copyright { get; set; }
    [XmlElement(ElementName = "tags")]
    public string Tags { get; set; }
    [XmlElement(ElementName = "repository")]
    public Repository Repository { get; set; }
    [XmlElement(ElementName = "dependencies")]
    public Dependencies Dependencies { get; set; }
    [XmlAttribute(AttributeName = "minClientVersion")]
    public string MinClientVersion { get; set; }
  }

  public class License
  {
    [XmlAttribute(AttributeName = "type")]
    public string Type { get; set; }
    [XmlText]
    public string Value { get; set; }
  }

  public class Repository
  {
    [XmlAttribute(AttributeName = "type")]
    public string Type { get; set; }
    [XmlAttribute(AttributeName = "url")]
    public string Url { get; set; }
    [XmlAttribute(AttributeName = "commit")]
    public string Commit { get; set; }
  }

  public class Dependencies
  {
    [XmlElement(ElementName = "group")]
    public List<Group> Group { get; set; }
  }

  public class Group
  {
    [XmlAttribute(AttributeName = "targetFramework")]
    public string TargetFramework { get; set; }
    [XmlElement(ElementName = "dependency")]
    public List<Dependency> Dependency { get; set; }
  }

  public class Dependency
  {
    [XmlAttribute(AttributeName = "id")]
    public string Id { get; set; }
    [XmlAttribute(AttributeName = "version")]
    public string Version { get; set; }
    [XmlAttribute(AttributeName = "exclude")]
    public string Exclude { get; set; }
  }

}