namespace AutoTrust;

public class Analyzers : ITrustCriteria {
  public static string Title => "Analyzers";
  public static int TotalScoreImportance => 3;

  public static (string, Status, string[]) Validate(DataHandler dataHandler) {
    var verbosityInfo = new List<string>();
    if (dataHandler.DependencyTree is not null) {
      foreach (var package in dataHandler.DependencyTree) {
        if (package.Value.HasAnalyzers && package.Value.Depth == 0) {
          verbosityInfo.Add($"Analyzers found in main package {package.Key}");
          return ($"Package is using an analyzer!", Status.Fail, verbosityInfo.ToArray());
        }
        else if (package.Value.HasAnalyzers && package.Value.Depth > 0) {
          verbosityInfo.Add($"Analyzer found in: Package {package.Key} with depth of {package.Value.Depth}");
          return ($"Package in dependency tree contains an analyzer!", Status.Error, verbosityInfo.ToArray());
        }
        verbosityInfo.Add($"No analyzer in: Package {package.Key} with depth of {package.Value.Depth}");
      }
    }
    verbosityInfo.Add($"No analyzer found in package or dependencies down to dependency depth {DependencyTreeBuilder.MAXDEPTH}");

    return ($"No analyzer found in package or dependencies down to dependency depth {DependencyTreeBuilder.MAXDEPTH}", Status.Pass, verbosityInfo.ToArray());
  }
}
