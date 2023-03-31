namespace AutoTrust;

public class Age : ITrustCriteria {
  public static string Title => "Package age";

  private static readonly int VersionAgeInDaysThreshold = 21;
  private static readonly int VersionOldAgeInDaysThreshold = 365;
  
  public static (string, Status) Validate(DataHandler dataHandler) {
    // Check if the package version is older than a certain threshold
    // Check if the package version is a pre-release version

    var now = DateTime.Now;
    if (dataHandler.NugetCatalogEntry?.Created == null) {
      // PrettyPrint.FailPrint("Can't find package version creation date");
      return ("Can't find package version creation date", Status.Fail);
    }
    var ageOfVersion = now.Subtract(dataHandler.NugetCatalogEntry.Created.UtcDateTime).Days;
    var packageIsPreRelease = dataHandler.PackageVersion.Contains('-');

    if (ageOfVersion < VersionAgeInDaysThreshold) {
      // PrettyPrint.FailPrint($"Package version {dataHandler.PackageVersion} was created less than 3 weeks ago: {ageOfVersion} days");
      return ($"Package version {dataHandler.PackageVersion} was created less than 3 weeks ago: {ageOfVersion} days", Status.Fail);
    }
    if (ageOfVersion > VersionOldAgeInDaysThreshold) {
      // PrettyPrint.FailPrint($"Package version {dataHandler.PackageVersion} was created more than 1 year ago: {ageOfVersion} days");
      return ($"Package version {dataHandler.PackageVersion} was created more than 1 year ago: {ageOfVersion} days", Status.Fail);
    }
    if (packageIsPreRelease) {
      // PrettyPrint.WarningPrint($"Package version is a pre-release version: {dataHandler.PackageVersion}");
      return ($"Package version is a pre-release version: {dataHandler.PackageVersion}", Status.Error);
    }

    // PrettyPrint.SuccessPrint("Age of package version criteria passed");
    return ("Age of package version criteria passed", Status.Pass);
  }
}
