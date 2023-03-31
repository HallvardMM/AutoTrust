namespace AutoTrust;

// Example deprecated package: EntityFramework.MappingAPI
public class Deprecated : ITrustCriteria {
  public static string Title => "Deprecated package";

  public static (string, Status) Validate(DataHandler dataHandler) {
    // Check if the package is deprecated or uses any deprecated dependencies
    if (dataHandler.NugetCatalogEntry?.Deprecation is not null) {
      if (dataHandler.NugetCatalogEntry.Deprecation.AlternatePackage?.AlternatePackageName is not null) {
        // PrettyPrint.FailPrint($"Package is deprecated, use '{dataHandler.NugetCatalogEntry?.Deprecation.AlternatePackage.AlternatePackageName}' instead!");
        return ($"Package is deprecated, use '{dataHandler.NugetCatalogEntry?.Deprecation.AlternatePackage.AlternatePackageName}' instead!", Status.Fail);
      }
      else {
        // PrettyPrint.FailPrint("Package is deprecated, no known alternative package to use!");
        return ("Package is deprecated, no known alternative package to use!", Status.Fail);
      }
    }

    // Package is not deprecated
    // PrettyPrint.SuccessPrint("Package is not deprecated");
    return ("Package is not deprecated", Status.Pass);
  }
}

