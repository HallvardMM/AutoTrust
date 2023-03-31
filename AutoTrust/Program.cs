// CA1852 Type 'Program' can be sealed because it has no subtypes in its containing assembly and is not externally visible
// Warning related to: https://github.com/dotnet/runtime/issues/78270
#pragma warning disable CA1852
using AutoTrust;

var httpClient = new HttpClient();

// Heads up: add and update are used similarly in dotnet
// dotnet add package <PACKAGE_NAME> 
// dotnet add package <PACKAGE_NAME> -v <VERSION> 
(var packageName, var packageVersion, var packageVersionSetByUser, var isPrerelease) = CliInputHandler.HandleInput(args);
if (packageName is "" && packageVersion is "" && packageVersionSetByUser is false && isPrerelease is false) {
  return;
}

var dataHandler = new DataHandler(httpClient, packageName, packageVersion, isPrerelease);
// Need to call fetchData to fetch the dataHandler object data
await dataHandler.FetchData();

// Dict format: {TC Title: (TC result message, Status)}
var trustCriteriaResult = new System.Collections.Concurrent.ConcurrentDictionary<string, (string, Status)>();

var tasks = new List<Task> {
  Task.Run(() => { trustCriteriaResult.TryAdd(Age.Title, Age.Validate(dataHandler)); }),
  Task.Run(() => { trustCriteriaResult.TryAdd(Popularity.Title, Popularity.Validate(dataHandler)); }),
  Task.Run(() => { trustCriteriaResult.TryAdd(KnownVulnerabilities.Title, KnownVulnerabilities.Validate(dataHandler)); }),
  Task.Run(() => { trustCriteriaResult.TryAdd(Deprecated.Title, Deprecated.Validate(dataHandler)); }),
  Task.Run(() => { trustCriteriaResult.TryAdd(DeprecatedDependencies.Title, DeprecatedDependencies.Validate(dataHandler)); }),
  Task.Run(() => { trustCriteriaResult.TryAdd(InitScript.Title, InitScript.Validate(dataHandler)); }),
  Task.Run(() => { trustCriteriaResult.TryAdd(DirectTransitiveDependencies.Title, DirectTransitiveDependencies.Validate(dataHandler)); }),
  Task.Run(() => { trustCriteriaResult.TryAdd(Documentation.Title, Documentation.Validate(dataHandler)); }),
  Task.Run(() => { trustCriteriaResult.TryAdd(License.Title, License.Validate(dataHandler)); }),
  Task.Run(() => { trustCriteriaResult.TryAdd(WidespreadUse.Title, WidespreadUse.Validate(dataHandler)); }),
};
var t = Task.WhenAll(tasks.ToArray());
try {
  await t;
}
catch { }

// To change the sorting order, change the order of the values in Status enum in ITrustCriteria.cs
var sortedTrustCriteriaResult = trustCriteriaResult.OrderBy(x => x.Value.Item2).ToList();

foreach (var result in sortedTrustCriteriaResult) {
  PrettyPrint.PrintTCMessage(result.Value.Item1, result.Value.Item2);
}


Console.WriteLine($"Nuget website for package: https://www.nuget.org/packages/{packageName.ToLower(System.Globalization.CultureInfo.InvariantCulture)}/{packageVersion.ToLower(System.Globalization.CultureInfo.InvariantCulture)}");

Console.WriteLine("Do you still want to add this package? (y/n)");

var addPackageQuery = Console.ReadLine()!.Trim();

if (Constants.PositiveResponse.Any(addPackageQuery.Contains)) {
  if (packageVersionSetByUser) {
    RunProcess.DotnetProcess(args);
  }
  else {
    if (isPrerelease) {
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
