namespace AutoTrust;

public class WidespreadUse : ITrustCriteria {
  public string Title => "Package wi";
  private static readonly int VersionOldAgeInDaysThreshold = 365;
  public static Status Validate(DataHandler dataHandler) {
    // Check if the package version is older than a certain threshold
    // Check if the package version is a pre-release version

    var now = DateTime.Now;
    if (dataHandler.OldestPublishedDate == null) {
      PrettyPrint.FailPrint("Can't find oldest version of package");
      return Status.Fail;
    }
    if (dataHandler.OldestPublishedDate != null) {
      var ageOfOldestVersion = now.Subtract(dataHandler.OldestPublishedDate.Value.UtcDateTime).Days;
      if (ageOfOldestVersion < VersionOldAgeInDaysThreshold) {
        PrettyPrint.FailPrint($"Oldest version of package found in Nuget was released {VersionOldAgeInDaysThreshold} days ago: {ageOfOldestVersion} days");
        return Status.Fail;
      }
    }

    PrettyPrint.SuccessPrint($"Package has been in widespread use for over {VersionOldAgeInDaysThreshold} days");
    return Status.Pass;
  }
}
