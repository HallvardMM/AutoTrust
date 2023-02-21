using AutoTrust;
using System;
using System.Net.Http.Json;
using System.Diagnostics;

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

    if (nugetPackage?.Listed == true && nugetPackage?.CatalogEntry != null)
    {
      Console.WriteLine($"Lastest stable version of package published: {nugetPackage?.Published}");
      
      string ManifestUrl = NugetPackage.GetManifestUrlFromPackageContentUrl(nugetPackage.CatalogEntry);
      var nugetCatalogEntry = await httpClient.GetFromJsonAsync<NugetCatalogEntry>(nugetPackage.CatalogEntry);

      Console.WriteLine($"Author(s) on NuGet: {string.Join(",", nugetCatalogEntry?.Authors)}");
    }
    else
    {
      throw new Exception("Package is not listed!");
    }


    Console.WriteLine($"Nuget site for package: https://www.nuget.org/packages/{packageName.ToLower()}/{packageVersion.ToLower()}");

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