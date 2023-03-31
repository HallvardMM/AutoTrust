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
      return ("Can't find package version creation date", Status.Fail);
    }
    var ageOfVersion = now.Subtract(dataHandler.NugetCatalogEntry.Created.UtcDateTime).Days;
    var packageIsPreRelease = dataHandler.PackageVersion.Contains('-');

    if (ageOfVersion < VersionAgeInDaysThreshold) {
      return ($"Package version {dataHandler.PackageVersion} was created less than 3 weeks ago: {ageOfVersion} days", Status.Fail);
    }
    if (ageOfVersion > VersionOldAgeInDaysThreshold) {
      return ($"Package version {dataHandler.PackageVersion} was created more than 1 year ago: {ageOfVersion} days", Status.Fail);
    }
    if (packageIsPreRelease) {
      return ($"Package version is a pre-release version: {dataHandler.PackageVersion}", Status.Error);
    }

    return ("Age of package version criteria passed", Status.Pass);
  }
}
