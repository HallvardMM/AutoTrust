// CA1852 Type 'Program' can be sealed because it has no subtypes in its containing assembly and is not externally visible
// Warning related to: https://github.com/dotnet/runtime/issues/78270
#pragma warning disable CA1852
using AutoTrust;

var httpClient = new HttpClient();

// Heads up: add and update are used similarly in dotnet
// dotnet add package <PACKAGE_NAME> 
// dotnet add package <PACKAGE_NAME> -v <VERSION> 
var (packageName, packageVersion, packageVersionSetByUser, isPrerelease, isVerbose) = CliInputHandler.HandleInput(args);
if (packageName is "" && packageVersion is "" && !packageVersionSetByUser && !isPrerelease && !isVerbose) {
  return;
}

var dataHandler = new DataHandler(httpClient, packageName, packageVersion, isPrerelease);
// Need to call fetchData to fetch the dataHandler object data
await dataHandler.FetchData();
Age.Validate(dataHandler, isVerbose);
Popularity.Validate(dataHandler, isVerbose);
KnownVulnerabilities.Validate(dataHandler, isVerbose);
Deprecated.Validate(dataHandler, isVerbose);
DeprecatedDependencies.Validate(dataHandler, isVerbose);
InitScript.Validate(dataHandler, isVerbose);
DirectTransitiveDependencies.Validate(dataHandler, isVerbose);
Documentation.Validate(dataHandler, isVerbose);
License.Validate(dataHandler, isVerbose);
WidespreadUse.Validate(dataHandler, isVerbose);

var nugetUrl = $"https://www.nuget.org/packages/{packageName.ToLowerInvariant()}/{packageVersion.ToLowerInvariant()}";
Console.WriteLine($"NuGet website for package: {nugetUrl}");

Console.WriteLine("Do you still want to add this package? (y/n)");

var addPackageQuery = Console.ReadLine()!.Trim();

// Remove verbosity flag from args as dotnet does not accept it
args = args.Except(Constants.VerbosityFlags).ToArray();

if (Constants.PositiveResponse.Any(addPackageQuery.Contains)) {
  if (packageVersionSetByUser) {
    RunProcess.DotnetProcess(args);
  }
  else {
    if (isPrerelease) {
      RunProcess.DotnetProcess(args);
    }
    else {
      RunProcess.DotnetProcess(args.Append("-v").Append(dataHandler.PackageVersion).ToArray());
    }
  }
}
else {
  Console.WriteLine("Package not added!");
}
