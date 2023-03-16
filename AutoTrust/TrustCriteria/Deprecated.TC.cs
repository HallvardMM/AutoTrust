namespace AutoTrust;

// Example deprecated package: EntityFramework.MappingAPI
public class Deprecated : ITrustCriteria {
  public string Title => "Deprecated package";

  public static Status Validate(DataHandler dataHandler) {
    // Check if the package is deprecated or uses any deprecated dependencies
    if (dataHandler.NugetCatalogEntry?.Deprecation is not null) {
      if (dataHandler.NugetCatalogEntry.Deprecation.AlternatePackage?.AlternatePackageName is not null) {
        PrettyPrint.FailPrint($"Package is deprecated, use '{dataHandler.NugetCatalogEntry?.Deprecation.AlternatePackage.AlternatePackageName}' instead!");
      }
      else {
        PrettyPrint.FailPrint("Package is deprecated, no known alternate package!");
      }
      return Status.Fail;
    }

    if (dataHandler.DeprecatedNugetPackages is not null && dataHandler.DeprecatedNugetPackages.Count != 0) {
      foreach (var entry in dataHandler.DeprecatedNugetPackages) {
        if (entry.Value.Contains("")) {
          Console.WriteLine($"Depends on package '{entry.Key}' which is deprecated");
        }
        else {
          Console.WriteLine($"Depends on package '{entry.Key}' which is deprecated for framework '{string.Join(", ", entry.Value)}'");
        }
      }
      PrettyPrint.FailPrint("Package uses deprecated dependency!");
      return Status.Fail;
    }

    PrettyPrint.SuccessPrint("Package is not deprecated");
    return Status.Pass;
  }

  public static async Task<Dictionary<string, HashSet<string>>> GetDeprecatedPackages(DataHandler dataHandler) {
    var deprecatedPackages = new Dictionary<string, HashSet<string>>();
    var tasks = new List<Task>();
    if (dataHandler.NugetCatalogEntry?.DependencyGroups is not null) {
      foreach (var dependencyGroup in dataHandler.NugetCatalogEntry.DependencyGroups) {
        foreach (var dependency in dependencyGroup.Dependencies) {
          tasks.Add(
            Task.Run(async () => {
              // Get the package
              var dependencyNugetPackage = await NugetPackage.GetNugetPackage(dataHandler.HttpClient, dependency.PackageName, GetFirstPackageVersion(dependency.Range));
              // Get the package catalog entry 
              if (dependencyNugetPackage?.CatalogEntry != null) {
                var dependencyNugetCatalogEntry = await NugetCatalogEntry.GetNugetCatalogEntry(dataHandler.HttpClient, dependencyNugetPackage.CatalogEntry);
                if (dependencyNugetCatalogEntry?.Deprecation is not null) {
                  if (!deprecatedPackages.ContainsKey(dependency.PackageName)) {
                    deprecatedPackages.Add(dependency.PackageName, new HashSet<string> { dependencyGroup.TargetFramework });
                  }
                  else {
                    deprecatedPackages[dependency.PackageName].Add(dependencyGroup.TargetFramework);
                  }
                }
              }
            })
          );      
        }
      }
      Task t = Task.WhenAll(tasks);
      try {
         t.Wait();
      }
      catch {}   
    }

    return deprecatedPackages;
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

