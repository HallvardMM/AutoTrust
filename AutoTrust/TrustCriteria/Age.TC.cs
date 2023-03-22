namespace AutoTrust;

public class Age : ITrustCriteria {
  public string Title => "Package age";

  public static Status Validate(DataHandler dataHandler) {
    // Check if the package version is older than a certain threshold
    // Check if the package version is a pre-release version

    var now = DateTime.Now;
    if (dataHandler.NugetCatalogEntry?.Created == null) {
      PrettyPrint.FailPrint("Can't find package version creation date");
      return Status.Fail;
    }
    var ageOfVersion = now.Subtract(dataHandler.NugetCatalogEntry.Created.UtcDateTime).Days;
    var packageIsPreRelease = dataHandler.NugetCatalogEntry?.Version.Contains('-') ?? false;

    if (ageOfVersion < TCConfiguration.VersionAgeInDaysThreshold) {
      PrettyPrint.FailPrint($"Package version was created less than {TCConfiguration.VersionAgeInDaysThreshold} days ago: {ageOfVersion} days");
      if (packageIsPreRelease) {
        PrettyPrint.WarningPrint($"Package version is a pre-release version: {dataHandler.NugetCatalogEntry?.Version}");
      }
      return Status.Fail;
    }
    if (ageOfVersion > TCConfiguration.VersionOldAgeInDaysThreshold) {
      PrettyPrint.FailPrint($"Package version was created more than {TCConfiguration.VersionOldAgeInDaysThreshold} days ago: {ageOfVersion} days");
      if (packageIsPreRelease) {
        PrettyPrint.WarningPrint($"Package version is a pre-release version: {dataHandler.NugetCatalogEntry?.Version}");
      }
      return Status.Fail;
    }
    if (packageIsPreRelease) {
      PrettyPrint.WarningPrint($"Package version is a pre-release version: {dataHandler.NugetCatalogEntry?.Version}");
      return Status.Error;
    }

    PrettyPrint.SuccessPrint("Age of package version criteria passed");
    return Status.Pass;
  }
}
