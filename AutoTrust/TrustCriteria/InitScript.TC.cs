namespace AutoTrust;

public class InitScript : ITrustCriteria {
  public static string Title => "Init Script";
  public static int TotalScoreImportance => 8;

  public static (string, Status, string[]) Validate(DataHandler dataHandler) {
    var verbosityInfo = new List<string>();
    if (dataHandler.DependencyTree is not null) {
      foreach (var package in dataHandler.DependencyTree) {
        if (package.Value.HasInitScript && package.Value.Depth == 0) {
          verbosityInfo.Add($"Init script in main package {package.Key}");
          return ($"Package has an init script!", Status.Fail, verbosityInfo.ToArray());
        }
        else if (package.Value.HasInitScript && package.Value.Depth > 0) {
          verbosityInfo.Add($"Init script in: Package {package.Key} with depth of {package.Value.Depth}");
          return ($"Package in dependency tree contains an init script!", Status.Error, verbosityInfo.ToArray());
        }
        verbosityInfo.Add($"No init script in: Package {package.Key} with depth of {package.Value.Depth}");
      }
    }
    verbosityInfo.Add($"No init script found in package or dependencies down to dependency depth {DependencyTreeBuilder.MAXDEPTH}");

    return ($"No init script found in package or dependencies down to dependency depth {DependencyTreeBuilder.MAXDEPTH}", Status.Pass, verbosityInfo.ToArray());
  }
}
