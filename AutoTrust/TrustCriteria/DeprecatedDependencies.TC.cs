namespace AutoTrust;

public class DeprecatedDependencies : ITrustCriteria {
  public static string Title => "Deprecated dependencies";
  public static int TotalScoreImportance => 10;

  public static (string, Status, string[]) Validate(DataHandler dataHandler) {
    var verbosityInfo = new List<string>();

    var deprecatedPackagesList = new Dictionary<string, string>();
    if (dataHandler.DependencyTree is not null) {
      foreach (var package in dataHandler.DependencyTree) {
        if (package.Value.IsDeprecated && package.Value.Depth != 0) {
          var packagePath = "";
          var currentPackageName = package.Key;
          verbosityInfo.Add($"Deprecated dependency: Package '{currentPackageName}' on dependency depth {package.Value.Depth}");
          while (true) {
            if (dataHandler.DependencyTree[currentPackageName].Depth == 0) {
              packagePath = packagePath.Insert(0, currentPackageName);
            }
            else {
              packagePath = packagePath.Insert(0, "/" + currentPackageName);
            }
            if (dataHandler.DependencyTree[currentPackageName].ParentName == "") {
              break;
            }
            currentPackageName = dataHandler.DependencyTree[currentPackageName].ParentName;
          }
          deprecatedPackagesList.Add(package.Key, packagePath);
        }
        else {
          verbosityInfo.Add($"Not deprecated dependency: Package '{package.Key}' on dependency depth {package.Value.Depth}");
        }
      }
    }
    if (deprecatedPackagesList.Count > 0) {
      var deprecatedDependencyReturnMessage = "";
      foreach (var entry in deprecatedPackagesList) {
        deprecatedDependencyReturnMessage += $"{Environment.NewLine}  - The deprecated package '{entry.Key}' is found in the dependency tree with package path: '{entry.Value}'";
      }
      verbosityInfo.Add($"Registered deprecated dependencies found on Nuget with depth of {DependencyTreeBuilder.MAXDEPTH}");
      return ("Package has a deprecated dependency in its dependency tree!" + deprecatedDependencyReturnMessage, Status.Fail, verbosityInfo.ToArray());
    }

    verbosityInfo.Add($"No registered deprecated dependencies found on Nuget with depth of {DependencyTreeBuilder.MAXDEPTH}");
    // Does not have any deprecated dependencies
    return ($"Package does not depend on deprecated packages down to dependency depth {DependencyTreeBuilder.MAXDEPTH}", Status.Pass, verbosityInfo.ToArray());
  }
}
