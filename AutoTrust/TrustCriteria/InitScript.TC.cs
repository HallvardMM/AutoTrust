namespace AutoTrust;

public class InitScript : ITrustCriteria {
  public static string Title => "Init Script";

  public static (string, Status) Validate(DataHandler dataHandler) {
    if (dataHandler.DependencyTree is not null) {
      foreach (var package in dataHandler.DependencyTree) {
        if (package.Value.HasInitScript && package.Value.Depth == 0) {
          return ($"Package has an init script!", Status.Fail);
        }
        else if (package.Value.HasInitScript && package.Value.Depth > 0) {
          return ($"Package in dependency tree contains an init script!", Status.Error);
        }
      }
    }

    return ($"No init script found in package or dependencies down to dependency depth {DependencyTreeBuilder.MAXDEPTH}", Status.Pass);
  }
}
