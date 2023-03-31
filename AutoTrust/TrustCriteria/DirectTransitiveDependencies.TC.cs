namespace AutoTrust;

public class DirectTransitiveDependencies : ITrustCriteria {
  public static string Title => "Direct and Transitive Dependencies";

  private static readonly int MaxDirectDependencies = 20;
  private static readonly int MaxTransitiveDependencies = 50;

  public static (string, Status) Validate(DataHandler dataHandler) {
    if (dataHandler.DependencyTree is not null) {
      var directDependencies = dataHandler.DependencyTree.Values.Where(x => x.Depth == 1).Count();
      var transitiveDependencies = dataHandler.DependencyTree.Values.Where(x => x.Depth > 1).Count();
      // Check if both direct and transitive dependencies are within the threshold
      if (directDependencies > MaxDirectDependencies && transitiveDependencies > MaxTransitiveDependencies) {
        return ($"Package has {directDependencies} direct dependencies and {transitiveDependencies} transitive dependencies down to dependency depth {DependencyTreeBuilder.MAXDEPTH}!", Status.Error);
      }
      // Check if direct dependencies are within the threshold
      else if (directDependencies > MaxDirectDependencies) {
        return ($"Package has {directDependencies} direct dependencies!", Status.Error);
      }
      // Check if transitive dependencies are within the threshold
      else if (transitiveDependencies > MaxTransitiveDependencies) {
        return ($"Package has {transitiveDependencies} transitive dependencies down to dependency depth {DependencyTreeBuilder.MAXDEPTH}!", Status.Error);
      }
    }

    return ($"Not a concerning amount of direct or transitive dependencies down to dependency depth {DependencyTreeBuilder.MAXDEPTH}", Status.Pass);
  }
}
