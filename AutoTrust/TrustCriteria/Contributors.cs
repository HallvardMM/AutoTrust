namespace AutoTrust;

public class Contributors : ITrustCriteria {
  public static string Title => "Adequate number of contributors";

  public static readonly int NumberOfContributorsThreshold = 5;

  public static (string, Status, string[]) Validate(DataHandler dataHandler) {
    var verbosityInfo = new List<string>();

    if (dataHandler.GithubContributorsData is not null) {
      if (dataHandler.GithubContributorsData.Count < NumberOfContributorsThreshold) {
        verbosityInfo.Add($"Package has {dataHandler.GithubContributorsData.Count} less than {NumberOfContributorsThreshold} contributors registered on GitHub");
        return ($"Package has less than {NumberOfContributorsThreshold} contributors!", Status.Fail, verbosityInfo.ToArray());
      }
    }
    verbosityInfo.Add($"Package has more than {NumberOfContributorsThreshold} contributors registered on GitHub");

    return ("Package has an adequate number of contributors", Status.Pass, verbosityInfo.ToArray());
  }
}
