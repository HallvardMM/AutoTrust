// CA1852 Type 'Program' can be sealed because it has no subtypes in its containing assembly and is not externally visible
// Warning related to: https://github.com/dotnet/runtime/issues/78270
#pragma warning disable CA1852
using AutoTrust;

var httpClient = new HttpClient();

// Heads up: add and update are used similarly in dotnet
// dotnet add package <PACKAGE_NAME> 
// dotnet add package <PACKAGE_NAME> -v <VERSION> 


(var packageName, var packageVersion, var packageVersionSetByUser, var prerelease) = CliInputHandler.HandleInput(args);
if (packageName is "" && packageVersion is "" && packageVersionSetByUser is false && prerelease is false) {
  return;
}
if (packageVersion is "") {
  var latestVersion = await NugetPackageVersion.GetLatestVersion(httpClient, packageName, prerelease);
  if (latestVersion != null) {
    packageVersion = latestVersion;
  }
  else {
    Console.WriteLine("Error: Package version not found!");
    return;
  }
};

var dataHandler = new DataHandler(httpClient, packageName, packageVersion);
// Need to call fetchData to fetch the dataHandler object data
await dataHandler.FetchData();
Age.Validate(dataHandler);
Popularity.Validate(dataHandler);
KnownVulnerabilities.Validate(dataHandler);
Deprecated.Validate(dataHandler);
DeprecatedDependencies.Validate(dataHandler);
InitScript.Validate(dataHandler);
DirectTransitiveDependencies.Validate(dataHandler);

Console.WriteLine($"Nuget website for package: https://www.nuget.org/packages/{packageName.ToLower(System.Globalization.CultureInfo.CurrentCulture)}/{packageVersion.ToLower(System.Globalization.CultureInfo.CurrentCulture)}");

Console.WriteLine("Do you still want to add this package? (y/n)");

var addPackageQuery = Console.ReadLine()!.Trim();

if (Constants.PositiveResponse.Any(addPackageQuery.Contains)) {
  if (packageVersionSetByUser) {
    RunProcess.DotnetProcess(args);
  }
  else {
    if (prerelease) {
      RunProcess.DotnetProcess(args);
    }
    else {
      RunProcess.DotnetProcess(args.Append("-v").Append(packageVersion).ToArray());
    }
  }
}
else {
  Console.WriteLine("Package not added!");
}
