namespace AutoTrust;

public class InitScript : ITrustCriteria {
  public string Title => "Init Script";

  public static Status Validate(DataHandler dataHandler, bool isVerbose) {
    if (dataHandler.DependencyTree is not null) {
      foreach (var package in dataHandler.DependencyTree) {
        if (package.Value.HasInitScript && package.Value.Depth == 0) {
          PrettyPrint.FailPrint($"Package has an init script!");
          return Status.Fail;
        }
        else if (package.Value.HasInitScript && package.Value.Depth > 0) {
          PrettyPrint.WarningPrint($"Package in dependency tree contains an init script!");
          return Status.Error;
        }
      }
    }

    PrettyPrint.SuccessPrint($"No init script found in package or dependencies down to dependency depth {DependencyTreeBuilder.MAXDEPTH}");
    return Status.Pass;
  }
}
