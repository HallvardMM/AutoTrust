namespace AutoTrust;

public class CliInputHandler {
  private static readonly string HelperText = "AutoTrust extension: \n" +
      "  Runs prior to 'dotnet add [<PROJECT>] package <PACKAGE_NAME> [options]' to provide information about the package to be added.\n" +
      "  Prompts user (y/n) after displaying information if they want to continue with the 'dotnet add' command.\n" +
      "Dotnet add: \n";

  private static readonly string[] HelpFlags = { "-h", "-?", "--help" };
  private static readonly string[] VersionFlags = { "-v", "--version" };
  private static readonly string[] PrereleaseFlag = { "--prerelease" };
  private static readonly string[] PositiveResponse = { "y", "yes", "Yes", "YES" };

  //TODO: Should we support some flags like --verbose or --no-check (this defeats some of the AutoTrust purpose)?

  public static async Task HandleInput(string[] args, HttpClient httpClient) {
    var query = args.AsQueryable();

    if (query.ElementAtOrDefault(0) == "add") {
      if (query.ElementAtOrDefault(1) == "package") {
        // Project file not specified
        await RunDotnetAddPackage(args, httpClient, query.ElementAtOrDefault(2));
        return;
      }
      else if (query.ElementAtOrDefault(2) == "package") {
        // Project file specified
        await RunDotnetAddPackage(args, httpClient, query.ElementAtOrDefault(3));
        return;
      }
    }
    RunDotnetProcess(args);
  }

  public static async Task RunDotnetAddPackage(string[] args, HttpClient httpClient, string? packageName) {
    if (packageName is null) {
      Console.WriteLine("Error: Package name not provided!");
      //Package name not provided run dotnet process for basic error handling
      RunDotnetProcess(args);
      return;
    }

    if (HelpFlags.Any(args.Contains)) {
      // Show command line help and not run the AutoTrust program 
      Console.WriteLine(HelperText);
      // Run the dotnet process to get original help information
      RunDotnetProcess(args);
      return;
    }

    await RunProgram(args, httpClient, packageName);
  }

  public static void RunDotnetProcess(string[] args) => RunProcess.ProcessExecution(string.Join(" ", args.ToArray()));

  public static async Task RunProgram(string[] args, HttpClient httpClient, string packageName) {

    string packageVersion = "";
    var packageVersionSetByUser = false;

    for (var i = 0; i < args.Length; i++) {
      if (VersionFlags.Any(args[i].Contains)) {
        packageVersion = args[i + 1];
        packageVersionSetByUser = true;
      }
    }

    var prerelease = (PrereleaseFlag.Any(args.Contains) || packageVersion.Contains('-'));

    // Version handling
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
    await dataHandler.fetchData();
    Popularity.validate(dataHandler);

    Console.WriteLine($"Nuget website for package: https://www.nuget.org/packages/{packageName.ToLower(System.Globalization.CultureInfo.CurrentCulture)}/{packageVersion.ToLower(System.Globalization.CultureInfo.CurrentCulture)}");

    Console.WriteLine("Do you still want to add this package? (y/n)");

    var addPackageQuery = Console.ReadLine()!.Trim();

    if (PositiveResponse.Any(addPackageQuery.Contains)) {
      if (packageVersionSetByUser) {
        RunDotnetProcess(args);
      }
      else {
        if (PrereleaseFlag.Any(args.Contains)) {
          RunDotnetProcess(args);
        }
        else {
          RunDotnetProcess(args.Append("-v").Append(packageVersion).ToArray());
        }
      }
    }
    else {
      Console.WriteLine("Package not added!");
    }
  }
}

