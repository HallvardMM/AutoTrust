using AutoTrust;
using System;
using System.IO;
using System.Net;
using System.Net.Http.Json;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;

string[] POSITIVE_RESPONSE = { "y", "yes", "Yes", "YES" };
string[] NEGATIVE_RESPONSE = { "n", "no", "No", "NO" };

var httpClient = new HttpClient();

// Heads up: add and update are used similarly in dotnet
// dotnet add package <PACKAGE_NAME> 
// dotnet add package <PACKAGE_NAME> -v <VERSION> 


var query = args.AsQueryable();

if (query.ElementAtOrDefault(0) == "add" & query.ElementAtOrDefault(1) == "package")
{
  // Fetch metadata about the package from the NuGet API, GitHub API, and security databases
  try
  {
    var packageName = query.ElementAtOrDefault(2);


    // Version handeling
    string packageVersion = null;
    string stableVersion = null;

    var versionsObject = await httpClient.GetFromJsonAsync<NugetPackageVersion>
      (NugetPackageVersion.GetVersionsUrl(packageName));


      stableVersion = NugetPackageVersion.GetLatestStableVersion(versionsObject?.Versions);
      Console.WriteLine("Latest stable version: " + stableVersion);

    if (query.ElementAtOrDefault(3) == "-v" || query.ElementAtOrDefault(3) == "--version")
    {
      packageVersion = query.ElementAtOrDefault(4);
    }
    else
    {
      packageVersion = stableVersion;
    }



    // Fetch package data
    var nugetPackage = await httpClient.GetFromJsonAsync<NugetPackage>
          (NugetPackage.GetNugetPackageUrl(packageName,packageVersion));

    var downloadPackage = false;
    
    if (downloadPackage) { 
    NugetPackageDownload.DownloadNugetPackage(httpClient, nugetPackage, packageName, packageVersion);
    }
    
    if (nugetPackage?.Listed == true && nugetPackage?.CatalogEntry != null)
    {
      Console.WriteLine($"Lastest stable version of package published: {nugetPackage?.Published}");
    
      var nugetCatalogEntry = await httpClient.GetFromJsonAsync<NugetCatalogEntry>(nugetPackage.CatalogEntry);

      Console.WriteLine($"Author(s) on NuGet: {string.Join(",", nugetCatalogEntry?.Authors)}");

      if (nugetCatalogEntry?.Deprecation != null)
      {
        Console.WriteLine($"Package is deprecated: {string.Join(",", nugetCatalogEntry?.Deprecation.Reasons)}");
        if (nugetCatalogEntry?.Deprecation.AlternatePackage != null)
        {
          Console.WriteLine($"Alternative package: {nugetCatalogEntry?.Deprecation.AlternatePackage.AlternatePackageName}");
          if (nugetCatalogEntry?.Deprecation.AlternatePackage.Range != null)
          {
            Console.WriteLine($"Alternative package version: {nugetCatalogEntry?.Deprecation.AlternatePackage.Range}");
          }
        }
      }
    }
    else
    {
      Console.WriteLine("Warning: Package is not listed!");
    }
    // Create a web client to download the XML file
    WebClient client = new WebClient();
    Stream stream = client.OpenRead(NugetPackageManifest.GetNugetPackageManifestUrl(packageName, packageVersion));

    // Deserialize the XML file into a NuGetPackage object
    XmlSerializer serializer = new XmlSerializer(typeof(NugetPackageManifest));
    NugetPackageManifest package = (NugetPackageManifest)serializer.Deserialize(stream);

    Console.WriteLine($"Id: {package.Metadata.Id}");
    Console.WriteLine($"Version: {package.Metadata.Version}");
    Console.WriteLine($"Title: {package.Metadata.Title}");
    Console.WriteLine($"Authors: {package.Metadata.Authors}");
    Console.WriteLine($"License: {package.Metadata.License}");
    Console.WriteLine($"LicenseUrl: {package.Metadata.LicenseUrl}");
    Console.WriteLine($"Icon: {package.Metadata.Icon}");
    Console.WriteLine($"Readme: {package.Metadata.Readme}");
    Console.WriteLine($"ProjectUrl: {package.Metadata.ProjectUrl}");
    Console.WriteLine($"IconUrl: {package.Metadata.IconUrl}");
    Console.WriteLine($"Description: {package.Metadata.Description}");
    Console.WriteLine($"Copyright: {package.Metadata.Copyright}");
    Console.WriteLine($"Tags: {package.Metadata.Tags}");
    Console.WriteLine($"Repository: {package.Metadata.Repository?.Type}, {package.Metadata.Repository?.Url}, {package.Metadata.Repository?.Commit}");





    var nugetDownloadCount = await httpClient.GetFromJsonAsync<NugetDownloadCount>(NugetDownloadCount.GetNugetDownloadCountUrl(packageName, packageVersion));

    if (nugetDownloadCount?.TotalHits == 1)
    {
      Console.WriteLine($"Total downloads for package: {nugetDownloadCount?.Data[0].TotalDownloads}");
      for (int i = 0; i < nugetDownloadCount?.Data[0].Versions.Count; i++)
      {
        if (nugetDownloadCount?.Data[0].Versions[i].Version == packageVersion)
        {
          Console.WriteLine($"Total downloads for version {nugetDownloadCount?.Data[0].Versions[i].Version}: {nugetDownloadCount?.Data[0].Versions[i].Downloads}");
          Console.WriteLine($"Total downloads/Total downloads for version: {nugetDownloadCount?.Data[0].TotalDownloads/nugetDownloadCount?.Data[0].Versions[i].Downloads}");
        }
      }
    }
    else
    {
      Console.WriteLine("Warning: More than one package fits the Id!");
    }
    
    

    Console.WriteLine($"Nuget website for package: https://www.nuget.org/packages/{packageName.ToLower()}/{packageVersion.ToLower()}");

    Console.WriteLine("Do you still want to add this package? (y/n)");

    var addPackageQuery = Console.ReadLine()!.Trim();

    if (POSITIVE_RESPONSE.Any(addPackageQuery.Contains))
    {
      using (Process dotnetProcess = new Process())
      {
        dotnetProcess.StartInfo.UseShellExecute = false;
        dotnetProcess.StartInfo.CreateNoWindow = true;
        dotnetProcess.StartInfo.RedirectStandardInput = true;
        dotnetProcess.StartInfo.RedirectStandardOutput = true;
        dotnetProcess.StartInfo.FileName = "dotnet.exe";

        if (packageVersion == "latest")
        {
          dotnetProcess.StartInfo.Arguments = "add package " + packageName;
        }
        else
        {
          {
            dotnetProcess.StartInfo.Arguments = "add package " + packageName + " -v " + packageVersion;
          }
          dotnetProcess.Start();
          dotnetProcess.StandardInput.Flush();
          dotnetProcess.StandardInput.Close();
          dotnetProcess.WaitForExit();
          Console.WriteLine(dotnetProcess.StandardOutput.ReadToEnd());
        }
      }
    }

  }
  catch (Exception e)
  {
    Console.WriteLine(e.Message);
  }
}
else
{
  try
  {
    using (Process dotnetProcess = new Process())
    {
      dotnetProcess.StartInfo.UseShellExecute = false;
      dotnetProcess.StartInfo.CreateNoWindow = true;
      dotnetProcess.StartInfo.RedirectStandardInput = true;
      dotnetProcess.StartInfo.RedirectStandardOutput = true;
      dotnetProcess.StartInfo.FileName = "dotnet.exe";
      dotnetProcess.StartInfo.Arguments = string.Join(" ", query.ToArray());
      Console.WriteLine("This is ran: " + dotnetProcess.StartInfo.FileName + " " + dotnetProcess.StartInfo.Arguments);
      dotnetProcess.Start();
      dotnetProcess.StandardInput.Flush();
      dotnetProcess.StandardInput.Close();
      dotnetProcess.WaitForExit();
      Console.WriteLine(dotnetProcess.StandardOutput.ReadToEnd());
    }
  }
  catch (Exception e)
  {
    Console.WriteLine(e.Message);
  }
}