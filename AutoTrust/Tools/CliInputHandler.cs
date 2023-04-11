namespace AutoTrust;

public class CliInputHandler {
  private static readonly string HelperText = @"
AutoTrust extension:
  Runs prior to 'dotnet add [<PROJECT>] package <PACKAGE_NAME> [options]' to provide information about the package to be added.
  Prompts user (y/n) after displaying information if they want to continue with the 'dotnet add' command.

Options:
  -?, -h, --help  Show help and usage information
  -ve, --verbosity <d|detailed|diag|diagnostic|n|normal>  Set the verbosity level. Allowed values are n[ormal], d[etailed], and diag[nostic]
Dotnet add information:";

  public static (string, string, bool, bool, bool, bool)? HandleInput(string[] args) {
    var query = args.AsQueryable();

    if (query.ElementAtOrDefault(0) == "add" && query.Contains("package")) {
      var packageNameIndex = query.ToList().IndexOf("package") + 1;
      return RunDotnetAddPackage(args, query.ElementAtOrDefault(packageNameIndex));
    }
    RunProcess.DotnetProcess(args);
    return null;
  }

  public static (string, string, bool, bool, bool, bool)? RunDotnetAddPackage(string[] args, string? packageName) {
    if (packageName is null) {
      Console.WriteLine("Error: Package name not provided!");
      //Package name not provided run dotnet process for basic error handling
      RunProcess.DotnetProcess(args);
      return null;
    }

    if (Constants.HelpFlags.Any(args.Contains)) {
      // Show command line help and not run the AutoTrust program 
      Console.WriteLine(HelperText);
      // Run the dotnet process to get original help information
      RunProcess.DotnetProcess(args);
      return null;
    }

    var packageVersion = "";
    var packageVersionSetByUser = false;

    var indexOfVersionFlag = Array.FindIndex(args, arg => Constants.VersionFlags.Contains(arg));
    if (indexOfVersionFlag != -1) {
      packageVersion = args[indexOfVersionFlag + 1];
      packageVersionSetByUser = true;
    }

    var verbosityLevel = "";
    var verbosityDetailed = false;
    var verbosityDiagnostic = false;

    var indexOfVerbosityFlag = Array.FindIndex(args, arg => Constants.VerbosityFlags.Contains(arg));
    if (indexOfVerbosityFlag != -1) {
      verbosityLevel = args[indexOfVerbosityFlag + 1];
      if (verbosityLevel is "d" or "detailed") {
        verbosityDetailed = true;
      }
      else if (verbosityLevel is "diag" or "diagnostic") {
        verbosityDiagnostic = true;
        verbosityDetailed = true;
      }
    }

    var prerelease = Constants.PrereleaseFlag.Any(args.Contains) || packageVersion.Contains('-');

    return (packageName, packageVersion, packageVersionSetByUser,
              prerelease, verbosityDetailed, verbosityDiagnostic);
  }

}
