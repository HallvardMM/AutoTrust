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
        PrettyPrint.FailPrint("Package is deprecated, no known alternative package to use!");
      }
      return Status.Fail;
    }

    // Package is not deprecated
    PrettyPrint.SuccessPrint("Package is not deprecated");
    return Status.Pass;
  }
}

