namespace AutoTrust;

public class WidespreadUse : ITrustCriteria {
  public static string Title => "Package widespread use";
  private static readonly int VersionOldAgeInDaysThreshold = 365;

  private static readonly int PreviousVersionsToCheck = 10;

  private static readonly long DownloadsThreshold = 10000 * PreviousVersionsToCheck;

  public static (string, Status, string[]) Validate(DataHandler dataHandler) {
    var verbosityInfo = new List<string>();

    var now = DateTime.Now;
    if (dataHandler.OldestPublishedDate == null) {
      verbosityInfo.Add("Cannot find an old stable version of package on Nuget and evaluate widespread use");
      return ("Cannot find an old stable version of package and evaluate widespread use", Status.Fail, verbosityInfo.ToArray());
    }
    else {
      var ageOfOldestVersion = now.Subtract(dataHandler.OldestPublishedDate.Value.UtcDateTime).Days;
      if (ageOfOldestVersion < VersionOldAgeInDaysThreshold) {
        verbosityInfo.Add($"Oldest version of package found in Nuget was released less than a year ago: {ageOfOldestVersion} days");
        return ($"Oldest version of package found in Nuget was released less than a year ago: {ageOfOldestVersion} days", Status.Fail, verbosityInfo.ToArray());
      }
      else {
        verbosityInfo.Add($"Oldest version of package found in Nuget was released more than a year ago: {ageOfOldestVersion} days");
      }
    }

    if (dataHandler.NugetDownloadCount == null) {
      verbosityInfo.Add("Cannot find download count for package on Nuget");
      return ("Cannot find download count for package and evaluate widespread use", Status.Fail, verbosityInfo.ToArray());
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
      verbosityInfo.Add($"Package has less than {PreviousVersionsToCheck} stable versions prior to {dataHandler.PackageVersion}");
      versionsToCheck = dataHandler.NugetDownloadCount.Data[0].Versions.GetRange(0, indexOfCurrentVersion);
    }
    else {
      verbosityInfo.Add($"Package has more than {PreviousVersionsToCheck} stable versions prior to {dataHandler.PackageVersion}");
      versionsToCheck = dataHandler.NugetDownloadCount.Data[0].Versions.GetRange(indexOfCurrentVersion - PreviousVersionsToCheck, PreviousVersionsToCheck);
    }
    versionsToCheck.ForEach(v => numberOfDownloadsFromLastVersions += v.Downloads);

    if (numberOfDownloadsFromLastVersions < DownloadsThreshold) {
      var checkedVersions = indexOfCurrentVersion < PreviousVersionsToCheck ? indexOfCurrentVersion : PreviousVersionsToCheck;
      verbosityInfo.Add($"The {checkedVersions} package versions prior to {dataHandler.PackageVersion} has less than {DownloadsThreshold} downloads: {numberOfDownloadsFromLastVersions}");
      return ($"The {checkedVersions} package versions prior to {dataHandler.PackageVersion} has less than {DownloadsThreshold} downloads: {numberOfDownloadsFromLastVersions}", Status.Fail, verbosityInfo.ToArray());
    }

    if (indexOfCurrentVersion < PreviousVersionsToCheck) {
      return ($"The {indexOfCurrentVersion} package versions prior to {dataHandler.PackageVersion} passed the download threshold {DownloadsThreshold} downloads: {numberOfDownloadsFromLastVersions}, but is less than {PreviousVersionsToCheck}", Status.Error, verbosityInfo.ToArray());
    }

    verbosityInfo.Add($"The {PreviousVersionsToCheck} package versions prior to {dataHandler.PackageVersion} has more than {DownloadsThreshold} downloads: {numberOfDownloadsFromLastVersions}");


    return ($"Package has been in widespread use for over a year", Status.Pass, verbosityInfo.ToArray());
  }
}
