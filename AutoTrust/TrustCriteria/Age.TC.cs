namespace AutoTrust;

public class Age : ITrustCriteria {
  public string Title => "Package age";

  private static readonly int VersionAgeInDaysThreshold = 22; //Same as npq
  private static readonly int VersionOldAgeInDaysThreshold = 365;
  public static Status Validate(DataHandler dataHandler) {
    // Check if the package version is older than a certain threshold
    // Check if the package version is a pre-release version

    var now = DateTime.Now;
    if (dataHandler.NugetCatalogEntry?.Created == null) {
      PrettyPrint.FailPrint("Can't find package version creation date");
      return Status.Fail;
    }
    var ageOfVersion = now.Subtract(dataHandler.NugetCatalogEntry.Created.UtcDateTime).Days;
    var packageIsPreRelease = dataHandler.PackageVersion.Contains('-');

    if (ageOfVersion < VersionAgeInDaysThreshold) {
      PrettyPrint.FailPrint($"Package version {dataHandler.PackageVersion} was created less than {VersionAgeInDaysThreshold} days ago: {ageOfVersion} days");
      return Status.Fail;
    }
    if (ageOfVersion > VersionOldAgeInDaysThreshold) {
      PrettyPrint.FailPrint($"Package version {dataHandler.PackageVersion} was created more than {VersionOldAgeInDaysThreshold} days ago: {ageOfVersion} days");
      return Status.Fail;
    }
    if (packageIsPreRelease) {
      PrettyPrint.WarningPrint($"Package version is a pre-release version: {dataHandler.PackageVersion}");
      return Status.Error;
    }

    PrettyPrint.SuccessPrint("Age of package version criteria passed");
    return Status.Pass;
  }
}
