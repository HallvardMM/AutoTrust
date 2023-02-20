using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;


namespace AutoTrust
{
  public class APINugetPackageVersion
  {
    public List<string>? Versions { get; set; }

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