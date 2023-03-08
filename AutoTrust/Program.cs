// CA1852 Type 'Program' can be sealed because it has no subtypes in its containing assembly and is not externally visible
// Warning related to: https://github.com/dotnet/runtime/issues/78270
#pragma warning disable CA1852
using AutoTrust;

string[] positive_response = { "y", "yes", "Yes", "YES" };
string[] negative_response = { "n", "no", "No", "NO" };

var httpClient = new HttpClient();

// Heads up: add and update are used similarly in dotnet
// dotnet add package <PACKAGE_NAME> 
// dotnet add package <PACKAGE_NAME> -v <VERSION> 

var query = args.AsQueryable();

if (query.ElementAtOrDefault(0) == "add" & query.ElementAtOrDefault(1) == "package")
{
  // Fetch metadata about the package from the NuGet API, GitHub API, and security databases
  var packageName = query.ElementAtOrDefault(2);
  if (packageName is null)
  {
	Console.WriteLine("Error: Package name not provided!");
	return;
  }

  // Version handling
  string? packageVersion;

  if (query.ElementAtOrDefault(3) is "-v" or "--version")
  {
	packageVersion = query.ElementAtOrDefault(4);
  }
  else
  {
	var latestVersion = await NugetPackageVersion.GetLatestStableVersion(httpClient, packageName);
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
  if (packageVersion is null)
  {
	Console.WriteLine("Error: Package version not found!");
	return;
  }

  var nugetPackage = await NugetPackage.GetNugetPackage(httpClient, packageName, packageVersion);

  if (nugetPackage is not null)
  {
	Console.WriteLine(nugetPackage.ToString());
  }
  else
  {
	Console.WriteLine($"Error: Package {packageName} with version {packageVersion} not found!");
  }

  // Download the package to the local machine

  var downloadPackage = false; // This is set to false while working on the project

  if (downloadPackage && nugetPackage is not null)
  {
	await NugetPackageDownload.DownloadNugetPackage(httpClient, nugetPackage, packageName, packageVersion);
  }


  // Get the package catalog entry with a lot of data such as potential vulnerabilities

  if (nugetPackage?.CatalogEntry != null)
  {
	var nugetCatalogEntry = await NugetCatalogEntry.GetNugetCatalogEntry(httpClient, nugetPackage.CatalogEntry);
	if (nugetCatalogEntry != null)
	{
	  Console.WriteLine(nugetCatalogEntry.ToString());
	  packageName = nugetCatalogEntry.PackageName;
	}
	else
	{
	  Console.WriteLine($"Error: Package catalog entry for {packageName} with version {packageVersion} not found!");
	}
  }

  var packageManifest = await NugetPackageManifest.GetNugetPackageManifest(httpClient, packageName, packageVersion);

  if (packageManifest != null)
  {
	Console.WriteLine(packageManifest.ToString());
  }
  else
  {
	Console.WriteLine($"Error: Package manifest for {packageName} with version {packageVersion} not found!");
  }

  var repositoryUrl = "";

  if (packageManifest?.Metadata.Repository?.Url?.ToLower(System.Globalization.CultureInfo.CurrentCulture).Contains("github.com") ?? false)
  {
	repositoryUrl = packageManifest.Metadata.Repository.Url;
  }
  else if (packageManifest?.Metadata.ProjectUrl?.ToLower(System.Globalization.CultureInfo.CurrentCulture).Contains("github.com") ?? false)
  {
	repositoryUrl = packageManifest.Metadata.ProjectUrl;
  }

  if (repositoryUrl != "")
  {
	var githubData = await GithubPackage.GetGithubPackage(httpClient, repositoryUrl);
	if (githubData != null)
	{
	  Console.WriteLine(githubData.ToString());
	}
	else
	{
	  Console.WriteLine($"Error: Package manifest for {packageName} with version {packageVersion} not found!");
	}

	var githubIssueData = await GithubIssues.GetGithubIssues(httpClient, repositoryUrl);
	if (githubIssueData != null)
	{
	  Console.WriteLine(githubIssueData.ToString());
	  Console.WriteLine($"Open PRs: {githubData?.OpenIssuesCount - githubIssueData.TotalCount}");
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

  var nugetDownloadCount = await NugetDownloadCount.GetNugetDownloadCount(httpClient, packageName);

  if (nugetDownloadCount != null)
  {
	Console.WriteLine(nugetDownloadCount.ToString(packageVersion));
  }
  else
  {
	Console.WriteLine($"Error: NuGet download count not found for {packageName} with version {packageVersion}!");
  }

  var osvData = await OSVData.GetOSVData(httpClient, packageName, packageVersion);

  if (osvData != null)
  {
	Console.WriteLine(osvData.ToString());
  }
  else
  {
	Console.WriteLine($"Error: OSV data not found for {packageName} with version {packageVersion}!");
  }

  Console.WriteLine($"Nuget website for package: https://www.nuget.org/packages/{packageName.ToLower(System.Globalization.CultureInfo.CurrentCulture)}/{packageVersion.ToLower(System.Globalization.CultureInfo.CurrentCulture)}");

  Console.WriteLine("Do you still want to add this package? (y/n)");

  var addPackageQuery = Console.ReadLine()!.Trim();

  if (positive_response.Any(addPackageQuery.Contains))
  {
	RunProcess.ProcessExecution("add package " + packageName + " -v " + packageVersion);
  }
}
else
{
  RunProcess.ProcessExecution(string.Join(" ", query.ToArray()));
}
