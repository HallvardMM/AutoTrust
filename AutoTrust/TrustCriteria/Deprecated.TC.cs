namespace AutoTrust;

// Example deprecated package: EntityFramework.MappingAPI
public class Deprecated : ITrustCriteria {
  public static string Title => "Deprecated package";
  public static int TotalScoreImportance => 10;

  public static (string, Status, string[]) Validate(DataHandler dataHandler) {
    // Check if the package is deprecated or uses any deprecated dependencies

    var verbosityInfo = new List<string>();

    if (dataHandler.NugetCatalogEntry?.Deprecation is not null) {
      if (dataHandler.NugetCatalogEntry.Deprecation.AlternatePackage?.AlternatePackageName is not null) {
        var alternatePackage = dataHandler.NugetCatalogEntry.Deprecation.AlternatePackage.AlternatePackageName;
        verbosityInfo.Add("NuGet Catalog Entry has registered the package as deprecated with an alternate package");
        return ($"Package is deprecated, use '{alternatePackage}' instead!", Status.Fail, verbosityInfo.ToArray());
      }
      else {
        verbosityInfo.Add("NuGet Catalog Entry has registered the package as deprecated but no alternate package is specified");
        return ("Package is deprecated, no known alternative package to use!", Status.Fail, verbosityInfo.ToArray());
      }
    }

    verbosityInfo.Add("NuGet Catalog Entry has not registered the package as deprecated");
    // Package is not deprecated
    return ("Package is not deprecated", Status.Pass, verbosityInfo.ToArray());
  }
}

