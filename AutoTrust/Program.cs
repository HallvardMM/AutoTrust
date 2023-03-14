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
  httpClient.Dispose();
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

  DataHandler dataHandler = new DataHandler(packageName, packageVersion);
  // Need to call fetchData to fetch the dataHandler object data
  await dataHandler.fetchData();
  Popularity.validate(dataHandler);
  
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
