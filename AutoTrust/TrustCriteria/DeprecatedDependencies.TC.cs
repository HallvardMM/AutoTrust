namespace AutoTrust;

public class DeprecatedDependencies : ITrustCriteria {
  public static string Title => "Deprecated Dependencies";

  public static (string, Status) Validate(DataHandler dataHandler) {
    var deprecatedPackagesList = new Dictionary<string, string>();
    if (dataHandler.DependencyTree is not null) {
      foreach (var package in dataHandler.DependencyTree) {
        if (package.Value.IsDeprecated && package.Value.Depth != 0) {
          var packagePath = "";
          var currentPackageName = package.Key;
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
      }
    }
    if (deprecatedPackagesList.Count > 0) {
      var deprecatedDependencyReturnMessage = "";
      foreach (var entry in deprecatedPackagesList) {
        deprecatedDependencyReturnMessage += $"{Environment.NewLine}  - The deprecated package '{entry.Key}' is found in the dependency tree with package path: '{entry.Value}'";
      }
      return ("Package has a deprecated dependency in its dependency tree!" + deprecatedDependencyReturnMessage, Status.Fail);
    }

    // Does not have any deprecated dependencies
    return ($"Package does not depend on deprecated packages down to dependency depth {DependencyTreeBuilder.MAXDEPTH}", Status.Pass);
  }
}
