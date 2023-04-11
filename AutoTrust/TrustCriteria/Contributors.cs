namespace AutoTrust;

// This file checks the following criteria:
// TC-9: The component has an adequate number of maintainers and/or contributors
// TC-10: The component is being developed by an active maintainer domain

public class Contributors : ITrustCriteria {
  public static string Title => "Adequate number of contributors";

  public static readonly int NumberOfContributorsThreshold = 5;

  public static (string, Status, string[]) Validate(DataHandler dataHandler) {
    var verbosityInfo = new List<string>();

    if (dataHandler.GithubContributorsCount is not null) {
      if (dataHandler.GithubContributorsCount < NumberOfContributorsThreshold) {
        verbosityInfo.Add($"Package has {dataHandler.GithubContributorsCount} contributors, which is less than the threshold of {NumberOfContributorsThreshold} contributors registered on GitHub");
        return ($"Package has less than {NumberOfContributorsThreshold} contributors!", Status.Fail, verbosityInfo.ToArray());
      }
    verbosityInfo.Add($"Package has {dataHandler.GithubContributorsCount} contributors registered on GitHub");
    }

    

    return ($"Package has an adequate number of contributors: {dataHandler.GithubContributorsCount}", Status.Pass, verbosityInfo.ToArray());
  }
}
