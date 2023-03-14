namespace AutoTrust;

public class CliInputHandler {
  private static readonly string HelperText = "AutoTrust extension: \n" +
      "  Runs prior to 'dotnet add [<PROJECT>] package <PACKAGE_NAME> [options]' to provide information about the package to be added.\n" +
      "  Prompts user (y/n) after displaying information if they want to continue with the 'dotnet add' command.\n" +
      "Dotnet add: \n";



  public static (string, string, bool, bool) HandleInput(string[] args) {
    var query = args.AsQueryable();

    if (query.ElementAtOrDefault(0) == "add") {
      if (query.ElementAtOrDefault(1) == "package") {
        // Project file not specified
        return RunDotnetAddPackage(args, query.ElementAtOrDefault(2));

      }
      else if (query.ElementAtOrDefault(2) == "package") {
        // Project file specified
        return RunDotnetAddPackage(args, query.ElementAtOrDefault(3));
      }
    }
    RunProcess.DotnetProcess(args);
    return ("", "", false, false);
  }

  public static (string, string, bool, bool) RunDotnetAddPackage(string[] args, string? packageName) {
    if (packageName is null) {
      Console.WriteLine("Error: Package name not provided!");
      //Package name not provided run dotnet process for basic error handling
      RunProcess.DotnetProcess(args);
      return ("", "", false, false);
    }

    if (Constants.HelpFlags.Any(args.Contains)) {
      // Show command line help and not run the AutoTrust program 
      Console.WriteLine(HelperText);
      // Run the dotnet process to get original help information
      RunProcess.DotnetProcess(args);
      return ("", "", false, false);
    }

    var packageVersion = "";
    var packageVersionSetByUser = false;

    for (var i = 0; i < args.Length; i++) {
      if (Constants.VersionFlags.Any(args[i].Contains)) {
        packageVersion = args[i + 1];
        packageVersionSetByUser = true;
      }
    }

    var prerelease = Constants.PrereleaseFlag.Any(args.Contains) || packageVersion.Contains('-');

    return (packageName, packageVersion, packageVersionSetByUser, prerelease);
  }


}

