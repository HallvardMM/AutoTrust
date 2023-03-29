namespace AutoTrust;

public struct DependencyNode {
  public int depth;
  public string name;
  public string parentName;
  public bool hasInitScript;
  public bool isDeprecated;
  public HashSet<string> frameworks;
}

public class DependencyTreeBuilder {
  public const int MAXDEPTH = 2;

  public static async Task<System.Collections.Concurrent.ConcurrentDictionary<string, DependencyNode>> GetDependencyTree(DataHandler dataHandler, System.Collections.Concurrent.ConcurrentDictionary<string, DependencyNode> currentTree, string name, string range, string parentName = "", int depth = MAXDEPTH) {
    if (depth == MAXDEPTH) {
      // Need to create the root node
      var rootNode = new DependencyNode {
        depth = MAXDEPTH - depth,
        name = name,
        parentName = parentName,
        frameworks = new HashSet<string>(),
        hasInitScript = false, // TODO: UPDATE THIS
        isDeprecated = false // TODO: UPDATE THIS
      };
      currentTree.TryAdd(name, rootNode);
    }

    var dependenciesToCheck = new Dictionary<string, string>();

    var nugetPackage = await NugetPackage.GetNugetPackage(dataHandler.HttpClient, name, GetFirstPackageVersion(range));
    if (nugetPackage?.CatalogEntry != null) {
      var nugetCatalogEntry = await NugetCatalogEntry.GetNugetCatalogEntry(dataHandler.HttpClient, nugetPackage.CatalogEntry);
      foreach (var dependencyGroup in nugetCatalogEntry.DependencyGroups) {
        foreach (var dependency in dependencyGroup.Dependencies) {
          if (!dependenciesToCheck.ContainsKey(dependency.PackageName)) {
            dependenciesToCheck.Add(dependency.PackageName, dependency.Range);
          }
          if (currentTree.TryGetValue(dependency.PackageName, out var value)) {
            value.frameworks.Add(dependencyGroup.TargetFramework);
          }
          else {
            // Add the dependency to the tree
            var dependencyNode = new DependencyNode {
              depth = MAXDEPTH - depth + 1,
              name = dependency.PackageName,
              parentName = name,
              frameworks = new HashSet<string> { dependencyGroup.TargetFramework },
              hasInitScript = false, // TODO: UPDATE THIS
              isDeprecated = false // TODO: UPDATE THIS
            };
            currentTree.TryAdd(dependency.PackageName, dependencyNode);
          }
        }
      }
    }

    var tasks = new List<Task>();
    // Check dependencies
    foreach (var entry in dependenciesToCheck) {
      // key = package name, value = version range
      var localDependencyNode = currentTree[entry.Key];
      if (depth - 1 > 0) {
        // Finished looping, return the tree
        tasks.Add(
              Task.Run(async () => { 
                  await GetDependencyTree(dataHandler, currentTree, name: entry.Key, range: entry.Value, parentName: name,  depth: depth - 1);
              }));
      }
    }
    var t = Task.WhenAll(tasks.ToArray());
        try {
          await t;
        }
        catch {
          Console.WriteLine("ERROR");
        }
    // If we get here, we have no more dependencies to check
    return currentTree;

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
