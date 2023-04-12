namespace AutoTrust;

// This file checks the following criteria:
// TC-9: The component has an adequate number of maintainers and/or contributors
// TC-10: The component is being developed by an active maintainer domain

public class Contributors : ITrustCriteria {
  public static string Title => "Adequate number of contributors";

  public static readonly int NumberOfContributorsThreshold = 5;
  public static readonly int ActiveMaintainerThreshold = 2;

  public static (string, Status, string[]) Validate(DataHandler dataHandler) {
    var verbosityInfo = new List<string>();

    var contributorsCommitCountLastYear = dataHandler.GithubCommitsData?
      .GroupBy(x => x?.Author?.Login ?? "Unknown")
      .Select(x => new { Author = x.Key, Count = x.Count() })
      .OrderByDescending(x => x.Count)
      .ToDictionary(x => x.Author, x => x.Count);
    var filteredContributorsCommitLastYear = contributorsCommitCountLastYear?
      .Where(x => x.Value >= ActiveMaintainerThreshold)
      .ToDictionary(x => x.Key, x => x.Value);

    if (dataHandler.GithubContributorsCount is not null) {
      if (dataHandler.GithubContributorsCount < NumberOfContributorsThreshold) {
        verbosityInfo.Add($"Package has {dataHandler.GithubContributorsCount} contributors, which is less than the threshold of {NumberOfContributorsThreshold} contributors registered on GitHub");
        return ($"Package has less than {NumberOfContributorsThreshold} contributors!", Status.Fail, verbosityInfo.ToArray());
      }
      verbosityInfo.Add($"Package has {dataHandler.GithubContributorsCount} contributors registered on GitHub");
    }

    if (filteredContributorsCommitLastYear is null || filteredContributorsCommitLastYear.Count == 0) {
      verbosityInfo.Add($"Package have had no maintainers with {ActiveMaintainerThreshold} or more commits in the last year");
      return ($"Package has no active maintainers in the last year", Status.Fail, verbosityInfo.ToArray());
    }
    else if (filteredContributorsCommitLastYear.Count < ActiveMaintainerThreshold) {
      verbosityInfo.Add($"Package have had no maintainers with {ActiveMaintainerThreshold} or more commits in the last year");
      return ($"Package has less than {ActiveMaintainerThreshold} active maintainers in the last year", Status.Error, verbosityInfo.ToArray());
    }

    return ($"Package has an adequate number of active contributors: {dataHandler.GithubContributorsCount}", Status.Pass, verbosityInfo.ToArray());
  }
}
