namespace AutoTrust;

public class Age : ITrustCriteria {
  public static string Title => "Package age";

  private static readonly int VersionAgeInDaysThreshold = 21;
  private static readonly int VersionOldAgeInDaysThreshold = 365;
  public static (string, Status, string[]) Validate(DataHandler dataHandler) {
    // Check if the package version is older than a certain threshold
    // Check if the package version is a pre-release version

    // List of passed criteria
    var passedCriteria = new List<string>();

    var now = DateTime.Now;
    if (dataHandler.NugetCatalogEntry?.Created == null) {
      return ("Cannot find package version creation date", Status.Fail, passedCriteria.ToArray());
    }
    else {
      passedCriteria.Add($"Package version {dataHandler.PackageVersion} was created {now.Subtract(dataHandler.NugetCatalogEntry.Created.UtcDateTime).Days} days ago");
    }
    var ageOfVersion = now.Subtract(dataHandler.NugetCatalogEntry.Created.UtcDateTime).Days;
    var packageIsPreRelease = dataHandler.PackageVersion.Contains('-');

    if (ageOfVersion < VersionAgeInDaysThreshold) {
      return ($"Package version {dataHandler.PackageVersion} was created less than 3 weeks ago: {ageOfVersion} days", Status.Fail, passedCriteria.ToArray());
    }
    else {
      passedCriteria.Add($"Package version {dataHandler.PackageVersion} was created more than 3 weeks ago: {ageOfVersion} days");
    }
    if (ageOfVersion > VersionOldAgeInDaysThreshold) {
      return ($"Package version {dataHandler.PackageVersion} was created more than 1 year ago: {ageOfVersion} days", Status.Fail, passedCriteria.ToArray());
    }
    else {
      passedCriteria.Add($"Package version {dataHandler.PackageVersion} was created less than 1 year ago: {ageOfVersion} days");
    }

    if (packageIsPreRelease) {
      return ($"Package version is a pre-release version: {dataHandler.PackageVersion}", Status.Error, passedCriteria.ToArray());
    }
    else {
      passedCriteria.Add($"Package version is not a pre-release version: {dataHandler.PackageVersion}");
    }

    return ("Age of package version criteria passed", Status.Pass, passedCriteria.ToArray());
  }
}
