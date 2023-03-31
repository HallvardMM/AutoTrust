namespace AutoTrust;

public class WidespreadUse : ITrustCriteria {
  public static string Title => "Package widespread use";
  private static readonly int VersionOldAgeInDaysThreshold = 365;

  private static readonly int PreviousVersionsToCheck = 10;

  private static readonly long DownloadsThreshold = 10000 * PreviousVersionsToCheck;

  public static (string, Status) Validate(DataHandler dataHandler) {

    var now = DateTime.Now;
    if (dataHandler.OldestPublishedDate == null) {
      // PrettyPrint.FailPrint("Cannot find an old stable version of package and evaluate widespread use");
      return ("Cannot find an old stable version of package and evaluate widespread use", Status.Fail);
    }
    else {
      var ageOfOldestVersion = now.Subtract(dataHandler.OldestPublishedDate.Value.UtcDateTime).Days;
      if (ageOfOldestVersion < VersionOldAgeInDaysThreshold) {
        // PrettyPrint.FailPrint($"Oldest version of package found in Nuget was released less than a year ago: {ageOfOldestVersion} days");
        return ($"Oldest version of package found in Nuget was released less than a year ago: {ageOfOldestVersion} days", Status.Fail);
      }
    }

    if (dataHandler.NugetDownloadCount == null) {
      // PrettyPrint.FailPrint("Cannot find download count for package and evaluate widespread use");
      return ("Cannot find download count for package and evaluate widespread use", Status.Fail);
    }

    long numberOfDownloadsFromLastVersions = 0;
    var indexOfCurrentVersion = 0;
    for (var i = 0; i < dataHandler.NugetDownloadCount.Data[0].Versions.Count; i++) {
      if (dataHandler.NugetDownloadCount.Data[0].Versions[i].Version == dataHandler.PackageVersion) {
        indexOfCurrentVersion = i;
        break;
      }
    }

    var versionsToCheck = new List<NugetDownloadCountVersion>();
    if (indexOfCurrentVersion < PreviousVersionsToCheck) {
      // TODO: FIND A WAY TO PRINT THIS WARNING
      // PrettyPrint.WarningPrint($"Package has less than {PreviousVersionsToCheck} stable versions prior to {dataHandler.PackageVersion}");
      versionsToCheck = dataHandler.NugetDownloadCount.Data[0].Versions.GetRange(0, indexOfCurrentVersion);
    }
    else {
      versionsToCheck = dataHandler.NugetDownloadCount.Data[0].Versions.GetRange(indexOfCurrentVersion - PreviousVersionsToCheck, PreviousVersionsToCheck);
    }
    versionsToCheck.ForEach(v => numberOfDownloadsFromLastVersions += v.Downloads);

    if (numberOfDownloadsFromLastVersions < DownloadsThreshold) {
      var checkedVersions = indexOfCurrentVersion < PreviousVersionsToCheck ? indexOfCurrentVersion : PreviousVersionsToCheck;
      // PrettyPrint.FailPrint($"The {checkedVersions} package versions prior to {dataHandler.PackageVersion} has less than {DownloadsThreshold} downloads: {numberOfDownloadsFromLastVersions}");
      return ($"The {checkedVersions} package versions prior to {dataHandler.PackageVersion} has less than {DownloadsThreshold} downloads: {numberOfDownloadsFromLastVersions}", Status.Fail);
    }

    // PrettyPrint.SuccessPrint($"Package has been in widespread use for over a year");
    return ($"Package has been in widespread use for over a year", Status.Pass);
  }
}
