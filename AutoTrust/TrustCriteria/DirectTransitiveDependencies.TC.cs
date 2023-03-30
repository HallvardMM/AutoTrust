namespace AutoTrust;

public class DirectTransitiveDependencies : ITrustCriteria {
  public string Title => "Direct and Transitive Dependencies";

  private static readonly int MaxDirectDependencies = 20;
  private static readonly int MaxTransitiveDependencies = 50;

  public static Status Validate(DataHandler dataHandler) {
    if (dataHandler.DependencyTree is not null) {
      var directDependencies = dataHandler.DependencyTree.Values.Where(x => x.Depth == 1).Count();
      var transitiveDependencies = dataHandler.DependencyTree.Values.Where(x => x.Depth > 1).Count();
      // Check if both direct and transitive dependencies are within the threshold
      if (directDependencies > MaxDirectDependencies && transitiveDependencies > MaxTransitiveDependencies) {
        PrettyPrint.WarningPrint($"Package has {directDependencies} direct dependencies and {transitiveDependencies} transitive dependencies down to tree depth {DependencyTreeBuilder.MAXDEPTH}!");
        return Status.Error;
      }
      // Check if direct dependencies are within the threshold
      else if (directDependencies > MaxDirectDependencies) {
        PrettyPrint.WarningPrint($"Package has {directDependencies} direct dependencies!");
        return Status.Error;
      }
      // Check if transitive dependencies are within the threshold
      else if (transitiveDependencies > MaxTransitiveDependencies) {
        PrettyPrint.WarningPrint($"Package has {transitiveDependencies} transitive dependencies down to tree depth {DependencyTreeBuilder.MAXDEPTH}!");
        return Status.Error;
      }
    }

    PrettyPrint.SuccessPrint($"Not a concerning amount of direct or transitive dependencies down to tree depth {DependencyTreeBuilder.MAXDEPTH}");
    return Status.Pass;
  }
}
