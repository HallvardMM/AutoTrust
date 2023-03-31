namespace AutoTrust;

// Example deprecated package: EntityFramework.MappingAPI
public class Deprecated : ITrustCriteria {
  public static string Title => "Deprecated package";

  public static (string, Status, string[]) Validate(DataHandler dataHandler) {
    // Check if the package is deprecated or uses any deprecated dependencies

    var passedCriteria = new List<string>();

    if (dataHandler.NugetCatalogEntry?.Deprecation is not null) {
      if (dataHandler.NugetCatalogEntry.Deprecation.AlternatePackage?.AlternatePackageName is not null) {
        var alternatePackage = dataHandler.NugetCatalogEntry.Deprecation.AlternatePackage.AlternatePackageName;
        passedCriteria.Add("NuGet Catalog Entry has registered the package as deprecated with an alternate package");
        return ($"Package is deprecated, use '{alternatePackage}' instead!", Status.Fail, passedCriteria.ToArray());
      }
      else {
        passedCriteria.Add("NuGet Catalog Entry has registered the package as deprecated but no alternate package is specified");
        return ("Package is deprecated, no known alternative package to use!", Status.Fail, passedCriteria.ToArray());
      }
    }

    passedCriteria.Add("NuGet Catalog Entry has not registered the package as deprecated");
    // Package is not deprecated
    return ("Package is not deprecated", Status.Pass, passedCriteria.ToArray());
  }
}

