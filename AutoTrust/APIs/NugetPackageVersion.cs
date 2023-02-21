using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;


namespace AutoTrust
{
  public class NugetPackageVersion
  {
    public List<string>? Versions { get; set; }

    public static string GetVersionsUrl(string packageName)
    {
     return ($"https://api.nuget.org/v3-flatcontainer/{packageName.ToLower()}/index.json");
    }

    public static string GetLatestStableVersion(List<string> versions)
    {
      for (int i = versions.Count - 1; i >= 0; i--)
      {
        if (!versions[i].Contains("-"))
        {
          return versions[i];
        }
      }
      return null;
    }

  }

}