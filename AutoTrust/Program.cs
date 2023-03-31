// CA1852 Type 'Program' can be sealed because it has no subtypes in its containing assembly and is not externally visible
// Warning related to: https://github.com/dotnet/runtime/issues/78270
#pragma warning disable CA1852
using AutoTrust;

var httpClient = new HttpClient();

// Heads up: add and update are used similarly in dotnet
// dotnet add package <PACKAGE_NAME> 
// dotnet add package <PACKAGE_NAME> -v <VERSION> 
var input = CliInputHandler.HandleInput(args);
if (input is null) {
  return;
}
var (packageName, packageVersion, packageVersionSetByUser, isPrerelease, isVerbose, isDiagnostic) = input.Value;

var dataHandler = new DataHandler(httpClient, packageName, packageVersion, isPrerelease);
// Need to call fetchData to fetch the dataHandler object data
await dataHandler.FetchData(isDiagnostic);

// Dict format: {TC Title: (TC result message, Status, ranking)}
var trustCriteriaResult = new System.Collections.Concurrent.ConcurrentDictionary<string, (string, Status, int)>();

var tasks = new List<Task> {
  Task.Run(() => {
    var (message, status) = Age.Validate(dataHandler, isVerbose);
    trustCriteriaResult.TryAdd(Age.Title, (message, status, 1));
  }),
  Task.Run(() => {
    var (message, status) = Popularity.Validate(dataHandler, isVerbose);
    trustCriteriaResult.TryAdd(Popularity.Title, (message, status, 2));
  }),
  Task.Run(() => {
    var (message, status) = KnownVulnerabilities.Validate(dataHandler, isVerbose);
    trustCriteriaResult.TryAdd(KnownVulnerabilities.Title, (message, status, 3));
  }),
  Task.Run(() => {
    var (message, status) = Deprecated.Validate(dataHandler, isVerbose);
    trustCriteriaResult.TryAdd(Deprecated.Title, (message, status, 4));
  }),
  Task.Run(() => {
    var (message, status) = DeprecatedDependencies.Validate(dataHandler, isVerbose);
    trustCriteriaResult.TryAdd(DeprecatedDependencies.Title, (message, status, 5));
  }),
  Task.Run(() => {
    var (message, status) = InitScript.Validate(dataHandler, isVerbose);
    trustCriteriaResult.TryAdd(InitScript.Title, (message, status, 6));
  }),
  Task.Run(() => {
    var (message, status) = DirectTransitiveDependencies.Validate(dataHandler, isVerbose);
    trustCriteriaResult.TryAdd(DirectTransitiveDependencies.Title, (message, status, 7));
  }),
  Task.Run(() => {
    var (message, status) = Documentation.Validate(dataHandler, isVerbose);
    trustCriteriaResult.TryAdd(Documentation.Title, (message, status, 8));
  }),
  Task.Run(() => {
    var (message, status) = License.Validate(dataHandler, isVerbose);
    trustCriteriaResult.TryAdd(License.Title, (message, status, 9));
  }),
  Task.Run(() => {
    var (message, status) = WidespreadUse.Validate(dataHandler, isVerbose);
    trustCriteriaResult.TryAdd(WidespreadUse.Title, (message, status, 10));
  }),
};
var t = Task.WhenAll(tasks.ToArray());
try {
  await t;
}
catch { }

// Sort the trustCriteriaResult by status and ranking
// To change the status sorting order, change the order of the values in Status enum in ITrustCriteria.cs
var sortedTrustCriteriaResult = trustCriteriaResult.OrderBy(x => x.Value.Item2).ThenBy(y => y.Value.Item3).ToList();

foreach (var result in sortedTrustCriteriaResult) {
  PrettyPrint.PrintTCMessage(result.Value.Item1, result.Value.Item2);
}


var nugetUrl = $"https://www.nuget.org/packages/{packageName.ToLowerInvariant()}/{packageVersion.ToLowerInvariant()}";
Console.WriteLine($"NuGet website for package: {nugetUrl}");

Console.WriteLine("Do you still want to add this package? (y/n)");

var addPackageQuery = Console.ReadLine()!.Trim();

// Remove verbosity flag from args as dotnet add package does not accept it
var indexOfVerbosityFlag = Array.FindLastIndex(args, arg => Constants.VerbosityFlags.Contains(arg));
if (indexOfVerbosityFlag != -1) {
  args = args.Take(indexOfVerbosityFlag).Concat(args.Skip(indexOfVerbosityFlag + 2)).ToArray();
}

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
