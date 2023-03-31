namespace AutoTrust;

public class InitScript : ITrustCriteria {
  public static string Title => "Init Script";

  public static (string, Status, string[]) Validate(DataHandler dataHandler) {
    var passedCriteria = new List<string>();
    if (dataHandler.DependencyTree is not null) {
      foreach (var package in dataHandler.DependencyTree) {
        if (package.Value.HasInitScript && package.Value.Depth == 0) {
          passedCriteria.Add($"Main package {package.Key} has an init script");
          return ($"Package has an init script!", Status.Fail, passedCriteria.ToArray());
        }
        else if (package.Value.HasInitScript && package.Value.Depth > 0) {
          passedCriteria.Add($"Package {package.Key} with depth of {package.Value.Depth} has an init script");
          return ($"Package in dependency tree contains an init script!", Status.Error, passedCriteria.ToArray());
        }
        passedCriteria.Add($"No init script found in package {package.Key} with depth of {package.Value.Depth}");
      }
    }
    passedCriteria.Add($"No init script found in package or dependencies down to dependency depth {DependencyTreeBuilder.MAXDEPTH}");

    return ($"No init script found in package or dependencies down to dependency depth {DependencyTreeBuilder.MAXDEPTH}", Status.Pass, passedCriteria.ToArray());
  }
}
