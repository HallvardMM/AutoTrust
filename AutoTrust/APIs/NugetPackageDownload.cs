using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;


namespace AutoTrust
{
  public class NugetPackageDownload
  {

    public static async Task DownloadNugetPackage(HttpClient httpClient, NugetPackage nugetPackage, string packageName, string packageVersion)
    {
      var responseStream = await httpClient.GetStreamAsync(nugetPackage?.PackageContent);
      using var fileSystem = new FileStream($"./{packageName}.{packageVersion}.nupkg", FileMode.OpenOrCreate);
      await responseStream.CopyToAsync(fileSystem);
    }

  }

}