namespace AutoTrust;

public class WidespreadUse : ITrustCriteria {
  public string Title => "Package widespread use";
  private static readonly int VersionOldAgeInDaysThreshold = 365;
  public static Status Validate(DataHandler dataHandler) {

    var now = DateTime.Now;
    if (dataHandler.OldestPublishedDate == null) {
      PrettyPrint.FailPrint("Cannot find an old stable version of package");
      return Status.Fail;
    }
    else {
      var ageOfOldestVersion = now.Subtract(dataHandler.OldestPublishedDate.Value.UtcDateTime).Days;
      if (ageOfOldestVersion < VersionOldAgeInDaysThreshold) {
        PrettyPrint.FailPrint($"Oldest version of package found in Nuget was released less than a year ago: {ageOfOldestVersion} days");
        return Status.Fail;
      }
    }

    PrettyPrint.SuccessPrint($"Package has been in widespread use for over a year");
    return Status.Pass;
  }
}
