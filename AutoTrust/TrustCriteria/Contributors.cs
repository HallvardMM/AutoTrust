namespace AutoTrust;

public class Contributors : ITrustCriteria {
  public static string Title => "Adequate number of contributors";

  private static readonly int NumberOfContributorsThreshold = 5;

  public static (string, Status) Validate(DataHandler dataHandler) {
    if (dataHandler.GithubContributorsData is not null) {
      if (dataHandler.GithubContributorsData.Count < NumberOfContributorsThreshold) {
        return ($"Package has {dataHandler.GithubContributorsData.Count} contributors, which is less than the threshold of {NumberOfContributorsThreshold}!", Status.Fail);
      }
    }

    return ("Package has an adequate number of contributors", Status.Pass);
  }
}
