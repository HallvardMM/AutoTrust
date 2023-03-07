using AutoTrust;
using System;
using System.IO;
using System.Net;
using System.Net.Http.Json;
using System.Diagnostics;
using System.Text.Json;
using System.Xml.Serialization;


string[] POSITIVE_RESPONSE = { "y", "yes", "Yes", "YES" };
string[] NEGATIVE_RESPONSE = { "n", "no", "No", "NO" };

bool VERBOSE = false;

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


    if (query.ElementAtOrDefault(3) == "-v" || query.ElementAtOrDefault(3) == "--version")
    {
      packageVersion = query.ElementAtOrDefault(4);
    }
    else
    {
      string? latestVersion = await NugetPackageVersion.GetLatestStableVersion(httpClient, packageName);
      if (latestVersion != null)
      {
        packageVersion = latestVersion;
      }
      else
      {
        Console.WriteLine("Error: Package version not found!");
        return;
      }
    }

    NugetPackage? nugetPackage = await NugetPackage.GetNugetPackage(httpClient, packageName, packageVersion);
    
    if (nugetPackage != null)
    {
      Console.WriteLine(nugetPackage.ToString());
    }
    else
    {
      Console.WriteLine($"Error: Package {packageName} with version {packageVersion} not found!");
    }

    // Download the package to the local machine

    var downloadPackage = false; // This is set to false while working on the project

    if (downloadPackage) { 
    NugetPackageDownload.DownloadNugetPackage(httpClient, nugetPackage, packageName, packageVersion);
    }


    // Get the package catalog entry with a lot of data such as potential vulnerabilities

    if (nugetPackage?.CatalogEntry != null)
    {
      NugetCatalogEntry? nugetCatalogEntry = await NugetCatalogEntry.GetNugetCatalogEntry(httpClient,nugetPackage.CatalogEntry);
      if(nugetCatalogEntry != null)
      {
        Console.WriteLine(nugetCatalogEntry.ToString());
        packageName = nugetCatalogEntry.PackageName;
      }
      else
      {
        Console.WriteLine($"Error: Package catalog entry for {packageName} with version {packageVersion} not found!");
      }
    }

    NugetPackageManifest? packageManifest = await NugetPackageManifest.GetNugetPackageManifest(httpClient, packageName, packageVersion);

    if (packageManifest != null)
    {
      Console.WriteLine(packageManifest.ToString());
    }
    else
    {
      Console.WriteLine($"Error: Package manifest for {packageName} with version {packageVersion} not found!");
    }

    string repositoryUrl = "";

    if (packageManifest.Metadata.Repository?.Url?.ToLower().Contains("github.com") ?? false)
    {
      repositoryUrl = packageManifest.Metadata.Repository.Url;
    }
    else if (packageManifest.Metadata.ProjectUrl?.ToLower().Contains("github.com") ?? false)
    {
      repositoryUrl = packageManifest.Metadata.ProjectUrl;
    }

    if (repositoryUrl != "")
    {
      GithubPackage? githubData = await GithubPackage.GetGithubPackage(httpClient, repositoryUrl);
      if (githubData != null)
      {
        Console.WriteLine(githubData.ToString());
      }
      else
      {
        Console.WriteLine($"Error: Package manifest for {packageName} with version {packageVersion} not found!");
      }

      GithubIssues? githubIssueData = await GithubIssues.GetGithubIssues(httpClient, repositoryUrl);
      if (githubIssueData != null)
      {
        Console.WriteLine(githubIssueData.ToString());
        Console.WriteLine($"Open PRs: {githubData.OpenIssuesCount - githubIssueData.TotalCount}");
      }
      else
      {
        Console.WriteLine($"Error: Github issues data not found for {packageName} with version {packageVersion}!");
      }
    }
    else
    {
      Console.WriteLine($"Error: No GitHub repository found for {packageName} with version {packageVersion}!");
    }

    NugetDownloadCount? nugetDownloadCount = await NugetDownloadCount.GetNugetDownloadCount(httpClient, packageName, packageVersion);

    if (nugetDownloadCount != null)
    {
      Console.WriteLine(nugetDownloadCount.ToString(packageVersion));
    }
    else
    {
      Console.WriteLine($"Error: NuGet download count not found for {packageName} with version {packageVersion}!");
    }

    OSVData? osvData = await OSVData.GetOSVData(httpClient, packageName, packageVersion);
    
    if (osvData != null)
    {
      Console.WriteLine(osvData.ToString());
    }
    else
    {
      Console.WriteLine($"Error: OSV data not found for {packageName} with version {packageVersion}!");
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
        dotnetProcess.StartInfo.FileName = "dotnet";

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
      dotnetProcess.StartInfo.FileName = "dotnet";
      dotnetProcess.StartInfo.Arguments = string.Join(" ", query.ToArray());
      //TODO: Remove only for debug
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