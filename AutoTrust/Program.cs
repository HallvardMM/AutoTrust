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
    
    var versionsObject = await httpClient.GetFromJsonAsync<NugetPackageVersion>
      (NugetPackageVersion.GetVersionsUrl(packageName));

    string stableVersion = NugetPackageVersion.GetLatestStableVersion(versionsObject?.Versions);
    
    if (VERBOSE)
    {
      Console.WriteLine("All versions found: " + versionsObject.ToString());
      Console.WriteLine("Latest stable version: " + stableVersion);
    }
    
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

    Console.WriteLine(nugetPackage.ToString());





    // Download the package to the local machine

    var downloadPackage = false; // This is set to false while working on the project

    if (downloadPackage) { 
    NugetPackageDownload.DownloadNugetPackage(httpClient, nugetPackage, packageName, packageVersion);
    }


    // Get the package catalog entry with a lot of data such as potential vulnerabilities

    if (nugetPackage?.CatalogEntry != null)
    {    
      var nugetCatalogEntry = await httpClient.GetFromJsonAsync<NugetCatalogEntry>(nugetPackage.CatalogEntry);
      if(nugetCatalogEntry != null)
      {
        Console.WriteLine(nugetCatalogEntry.ToString());
      }
    }

    // Create a web client to download the XML file
    WebClient client = new WebClient();
    Stream stream = client.OpenRead(NugetPackageManifest.GetNugetPackageManifestUrl(packageName, packageVersion));

    // Deserialize the XML file into a NuGetPackage object
    XmlSerializer serializer = new XmlSerializer(typeof(NugetPackageManifest));
    NugetPackageManifest package = (NugetPackageManifest)serializer.Deserialize(stream);

    Console.WriteLine(package.ToString());
    

    //TODO: Move this logic into seperate file
    if (package.Metadata.Repository?.Url?.ToLower().Contains("github.com") ?? false)
    {
      // Github returns forbidden. Needs headers: {'User-Agent': 'request'}
      var githubApiUrl = GithubPackage.GetGithubApiUrl(package.Metadata.Repository.Url);

      Console.WriteLine(githubApiUrl);
      httpClient.DefaultRequestHeaders.Add("User-Agent", "request");
      var githubData = await httpClient.GetFromJsonAsync<GithubPackage>(githubApiUrl);
      if (githubData != null)
      {
        Console.WriteLine(githubData.ToString());
      }
      
      var githubIssuesUrl = githubApiUrl.Replace("repos/", "search/issues?q=repo:") + "+type:issue+state:open&per_page=1";
      Console.WriteLine(githubIssuesUrl);
      httpClient.DefaultRequestHeaders.Add("User-Agent", "request");
      GithubIssues githubIssueData = await httpClient.GetFromJsonAsync<GithubIssues>(githubIssuesUrl);
      if (githubIssueData != null)
      {
        Console.WriteLine(githubIssueData.ToString());
        Console.WriteLine($"Open PRs: {githubData.OpenIssuesCount - githubIssueData.TotalCount}");
      }
    }
    else if(package.Metadata.ProjectUrl?.ToLower().Contains("github.com") ?? false)
    {
      var githubApiUrl = GithubPackage.GetGithubApiUrl(package.Metadata.ProjectUrl);

      Console.WriteLine(githubApiUrl);
      httpClient.DefaultRequestHeaders.Add("User-Agent", "request");
      var githubData = await httpClient.GetFromJsonAsync<GithubPackage>(githubApiUrl);
      if (githubData != null)
      {
        Console.WriteLine(githubData.ToString());
      }
      var githubIssuesUrl = githubApiUrl.Replace("repos/", "search/issues?q=repo:") + "+type:issue+state:open&per_page=1";
      httpClient.DefaultRequestHeaders.Add("User-Agent", "request");
      GithubIssues githubIssueData = await httpClient.GetFromJsonAsync<GithubIssues>(githubIssuesUrl);
      if (githubIssueData != null)
      {
        Console.WriteLine(githubIssueData.ToString());
        Console.WriteLine($"Open PRs: {githubData.OpenIssuesCount - githubIssueData.TotalCount}\n");
      }
    }
    

    var nugetDownloadCount = await httpClient.GetFromJsonAsync<NugetDownloadCount>(NugetDownloadCount.GetNugetDownloadCountUrl(packageName, packageVersion));

    Console.WriteLine(nugetDownloadCount.ToString(packageVersion));
    
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