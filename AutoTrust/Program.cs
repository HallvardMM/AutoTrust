// CA1852 Type 'Program' can be sealed because it has no subtypes in its containing assembly and is not externally visible
// Warning related to: https://github.com/dotnet/runtime/issues/78270
#pragma warning disable CA1852
using AutoTrust;

var httpClient = new HttpClient();
var totalTCSecurityScore = 0.0;
var totalPossibleTCSecurityScore = 0;
// Heads up: add and update are used similarly in dotnet
// dotnet add package <PACKAGE_NAME> 
// dotnet add package <PACKAGE_NAME> -v <VERSION> 
var input = CliInputHandler.HandleInput(args);
if (input is null) {
  return;
}
TerminalSpinner.Start();
var (packageName, packageVersion, packageVersionSetByUser, isPrerelease, isVerbose, isDiagnostic) = input.Value;

var dataHandler = new DataHandler(httpClient, packageName, packageVersion, isPrerelease);
// Need to call fetchData to fetch the dataHandler object data
await dataHandler.FetchData(isDiagnostic);

// Dict format: {TC Title: (TC result message, Status, ranking)}
var trustCriteriaResult = new System.Collections.Concurrent.ConcurrentDictionary<string, (string, Status, string[], int)>();

var tasks = new List<Task> {
  Task.Run(() => {
    var (message, status, additionalInfo) = Age.Validate(dataHandler);
    HelperFunctions.AddSecurityScoreOfTC(Age.TotalScoreImportance, status, ref totalTCSecurityScore, ref totalPossibleTCSecurityScore);
    trustCriteriaResult.TryAdd(Age.Title, (message, status, additionalInfo, Age.TotalScoreImportance));
  }),
  Task.Run(() => {
    var (message, status, additionalInfo) = Popularity.Validate(dataHandler);
    HelperFunctions.AddSecurityScoreOfTC(Popularity.TotalScoreImportance, status, ref totalTCSecurityScore, ref totalPossibleTCSecurityScore);
    trustCriteriaResult.TryAdd(Popularity.Title, (message, status, additionalInfo, Popularity.TotalScoreImportance));
  }),
  Task.Run(() => {
    var (message, status, additionalInfo) = KnownVulnerabilities.Validate(dataHandler);
    HelperFunctions.AddSecurityScoreOfTC(KnownVulnerabilities.TotalScoreImportance, status, ref totalTCSecurityScore, ref totalPossibleTCSecurityScore);
    trustCriteriaResult.TryAdd(KnownVulnerabilities.Title, (message, status, additionalInfo, KnownVulnerabilities.TotalScoreImportance));
  }),
  Task.Run(() => {
    var (message, status, additionalInfo) = Deprecated.Validate(dataHandler);
    HelperFunctions.AddSecurityScoreOfTC(Deprecated.TotalScoreImportance, status, ref totalTCSecurityScore, ref totalPossibleTCSecurityScore);
    trustCriteriaResult.TryAdd(Deprecated.Title, (message, status, additionalInfo, Deprecated.TotalScoreImportance));
  }),
  Task.Run(() => {
    var (message, status, additionalInfo) = DeprecatedDependencies.Validate(dataHandler);
    HelperFunctions.AddSecurityScoreOfTC(DeprecatedDependencies.TotalScoreImportance, status, ref totalTCSecurityScore, ref totalPossibleTCSecurityScore);
    trustCriteriaResult.TryAdd(DeprecatedDependencies.Title, (message, status, additionalInfo, DeprecatedDependencies.TotalScoreImportance));
  }),
  Task.Run(() => {
    var (message, status, additionalInfo) = InitScript.Validate(dataHandler);
    HelperFunctions.AddSecurityScoreOfTC(InitScript.TotalScoreImportance, status, ref totalTCSecurityScore, ref totalPossibleTCSecurityScore);
    trustCriteriaResult.TryAdd(InitScript.Title, (message, status, additionalInfo, InitScript.TotalScoreImportance));
  }),
  Task.Run(() => {
    var (message, status, additionalInfo) = Analyzers.Validate(dataHandler);
    HelperFunctions.AddSecurityScoreOfTC(Analyzers.TotalScoreImportance, status, ref totalTCSecurityScore, ref totalPossibleTCSecurityScore);
    trustCriteriaResult.TryAdd(Analyzers.Title, (message, status, additionalInfo, Analyzers.TotalScoreImportance));
  }),
  Task.Run(() => {
    var (message, status, additionalInfo) = DirectTransitiveDependencies.Validate(dataHandler);
    HelperFunctions.AddSecurityScoreOfTC(DirectTransitiveDependencies.TotalScoreImportance, status, ref totalTCSecurityScore, ref totalPossibleTCSecurityScore);
    trustCriteriaResult.TryAdd(DirectTransitiveDependencies.Title, (message, status, additionalInfo, DirectTransitiveDependencies.TotalScoreImportance));
  }),
  Task.Run(() => {
    var (message, status, additionalInfo) = Documentation.Validate(dataHandler);
    HelperFunctions.AddSecurityScoreOfTC(Documentation.TotalScoreImportance, status, ref totalTCSecurityScore, ref totalPossibleTCSecurityScore);
    trustCriteriaResult.TryAdd(Documentation.Title, (message, status, additionalInfo, Documentation.TotalScoreImportance));
  }),
  Task.Run(() => {
    var (message, status, additionalInfo) = License.Validate(dataHandler);
    HelperFunctions.AddSecurityScoreOfTC(License.TotalScoreImportance, status, ref totalTCSecurityScore, ref totalPossibleTCSecurityScore);
    trustCriteriaResult.TryAdd(License.Title, (message, status, additionalInfo, License.TotalScoreImportance));
  }),
  Task.Run(() => {
    var (message, status, additionalInfo) = WidespreadUse.Validate(dataHandler);
    HelperFunctions.AddSecurityScoreOfTC(WidespreadUse.TotalScoreImportance, status, ref totalTCSecurityScore, ref totalPossibleTCSecurityScore);
    trustCriteriaResult.TryAdd(WidespreadUse.Title, (message, status, additionalInfo, WidespreadUse.TotalScoreImportance));
  }),
  Task.Run(() => {
    var (message, status, additionalInfo) = Contributors.Validate(dataHandler);
    HelperFunctions.AddSecurityScoreOfTC(Contributors.TotalScoreImportance, status, ref totalTCSecurityScore, ref totalPossibleTCSecurityScore);
    trustCriteriaResult.TryAdd(Contributors.Title, (message, status, additionalInfo, Contributors.TotalScoreImportance));
  }),
  Task.Run(() => {
    var (message, status, additionalInfo) = VerifiedPrefix.Validate(dataHandler);
    HelperFunctions.AddSecurityScoreOfTC(VerifiedPrefix.TotalScoreImportance, status, ref totalTCSecurityScore, ref totalPossibleTCSecurityScore);
    trustCriteriaResult.TryAdd(VerifiedPrefix.Title, (message, status, additionalInfo, VerifiedPrefix.TotalScoreImportance));
  }),
  Task.Run(() => {
    var (message, status, additionalInfo) = OpenIssues.Validate(dataHandler);
    HelperFunctions.AddSecurityScoreOfTC(OpenIssues.TotalScoreImportance, status, ref totalTCSecurityScore, ref totalPossibleTCSecurityScore);
    trustCriteriaResult.TryAdd(OpenIssues.Title, (message, status, additionalInfo, OpenIssues.TotalScoreImportance));
  }),
  Task.Run(() => {
    var (message, status, additionalInfo) = OpenPullRequests.Validate(dataHandler);
    HelperFunctions.AddSecurityScoreOfTC(OpenPullRequests.TotalScoreImportance, status, ref totalTCSecurityScore, ref totalPossibleTCSecurityScore);
    trustCriteriaResult.TryAdd(OpenPullRequests.Title, (message, status, additionalInfo, OpenPullRequests.TotalScoreImportance));
  }),
};
TerminalSpinner.Stop();
var t = Task.WhenAll(tasks.ToArray());
try {
  await t;
}
catch { }


// Sort the trustCriteriaResult by status and ranking
// To change the status sorting order, change the order of the values in Status enum in ITrustCriteria.cs
var sortedTrustCriteriaResult = trustCriteriaResult.OrderBy(x => x.Value.Item2).ThenBy(y => y.Value.Item4).ToList();

foreach (var result in sortedTrustCriteriaResult) {
  PrettyPrint.PrintTCMessage(result.Value.Item1, result.Value.Item2, result.Value.Item3, isVerbose);
}

PrettyPrint.SecurityScorePrint(HelperFunctions.CalculateNumberOfStars(totalTCSecurityScore, totalPossibleTCSecurityScore));

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
