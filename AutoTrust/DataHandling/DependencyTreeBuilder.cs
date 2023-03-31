namespace AutoTrust;

public class DependencyTreeBuilder {
  public const int MAXDEPTH = 2;

  public static async Task<System.Collections.Concurrent.ConcurrentDictionary<string, DependencyNode>> GetDependencyTree(
    DataHandler dataHandler,
    System.Collections.Concurrent.ConcurrentDictionary<string, DependencyNode> currentTree,
    string name,
    string range,
    bool isDiagnostic,
    string parentName = "",
    int depth = MAXDEPTH) {
    if (depth == MAXDEPTH) {
      // Need to create the root node
      var rootNode = new DependencyNode {
        Depth = MAXDEPTH - depth,
        Name = name,
        ParentName = parentName,
        Frameworks = new HashSet<string>(),
        HasInitScript = false,
        IsDeprecated = false
      };
      currentTree.TryAdd(name, rootNode);
    }
    // Need to have a dictionary of the dependencies that are not in the tree yet to iterate over
    var dependenciesToCheck = new Dictionary<string, string>();

    // Get the package
    var nugetPackage = await NugetPackage.GetNugetPackage(dataHandler.HttpClient, name, GetFirstPackageVersion(range), isDiagnostic);
    if (nugetPackage?.CatalogEntry != null) {
      var nugetCatalogEntry = await NugetCatalogEntry.GetNugetCatalogEntry(dataHandler.HttpClient, nugetPackage.CatalogEntry, isDiagnostic);
      // Check if package is deprecated
      if (nugetCatalogEntry?.Deprecation is not null) {
        currentTree[name] = new DependencyNode {
          Depth = currentTree[name].Depth,
          Name = currentTree[name].Name,
          ParentName = currentTree[name].ParentName,
          Frameworks = currentTree[name].Frameworks,
          HasInitScript = currentTree[name].HasInitScript,
          IsDeprecated = true
        };
      }
      // Check for init script
      if (nugetCatalogEntry?.PackageEntries is not null) {
        if (CheckForInitScript(nugetCatalogEntry.PackageEntries)) {
          // Package has init script
          currentTree[name] = new DependencyNode {
            Depth = currentTree[name].Depth,
            Name = currentTree[name].Name,
            ParentName = currentTree[name].ParentName,
            Frameworks = currentTree[name].Frameworks,
            HasInitScript = true,
            IsDeprecated = currentTree[name].IsDeprecated
          };
        }
      }
      // Add dependencies for next iteration if not at max depth
      if (nugetCatalogEntry?.DependencyGroups != null && depth > 0) {
        foreach (var dependencyGroup in nugetCatalogEntry.DependencyGroups) {
          foreach (var dependency in dependencyGroup.Dependencies) {
            if (!dependenciesToCheck.ContainsKey(dependency.PackageName)) {
              // If the dependency is not already in the dictionary, add it
              dependenciesToCheck.Add(dependency.PackageName, dependency.Range);
            }
            if (currentTree.TryGetValue(dependency.PackageName, out var value)) {
              // If the dependency is already in the tree, add the framework to the list of frameworks
              value.Frameworks.Add(dependencyGroup.TargetFramework);
            }
            else {
              // Add the dependency to the tree if it is not already in the tree
              var dependencyNode = new DependencyNode {
                Depth = MAXDEPTH - depth + 1,
                Name = dependency.PackageName,
                ParentName = name,
                Frameworks = new HashSet<string> { dependencyGroup.TargetFramework },
                HasInitScript = false,
                IsDeprecated = false
              };
              currentTree.TryAdd(dependency.PackageName, dependencyNode);
            }
          }
        }

      }
    }

    var tasks = new List<Task>();
    // Check dependencies that have not been checked yet
    foreach (var entry in dependenciesToCheck) {
      // key = package name, value = version range
      var localDependencyNode = currentTree[entry.Key];
      if (depth > 0) {
        // Step down the dependency tree
        tasks.Add(
          Task.Run(async () => await GetDependencyTree(dataHandler, currentTree, name: entry.Key, range: entry.Value, isDiagnostic, parentName: name, depth: depth - 1)));
      }
    }
    var t = Task.WhenAll(tasks.ToArray());
    try {
      await t;
    }
    catch {
      Console.WriteLine("ERROR, COULD NOT GET DEPENDENCY TREE");
    }
    // If we get here, we have no more dependencies to check and jump back up the tree
    return currentTree;
  }

  public static bool CheckForInitScript(List<PackageEntries> catalogEntries) {
    var scriptFileNames = new List<string> { "init.ps1", "install.ps1", "uninstall.ps1" };
    foreach (var entry in catalogEntries) {
      if (scriptFileNames.Contains(entry.Name.ToLowerInvariant())) {
        return true;
      }
    }
    return false;
  }

  public static string GetFirstPackageVersion(string version) {
    if (version.Contains('(')) {
      version = version.Replace("(", "");
    }
    if (version.Contains('[')) {
      version = version.Replace("[", "");
    }
    if (version.Contains(')')) {
      version = version.Replace(")", "");
    }
    if (version.Contains(']')) {
      version = version.Replace("]", "");
    }
    if (version.Contains(',')) {
      version = version.Split(",")[0];
    }
    return version;
  }
}
