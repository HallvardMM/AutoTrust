namespace AutoTrust;

// This file checks the following criteria:
// TC-9: The component has an adequate number of maintainers and/or contributors
// TC-10: The component is being developed by an active maintainer domain

public class Contributors : ITrustCriteria {
  public static string Title => "Contributors";
  public static int TotalScoreImportance => 3;

  public static readonly int NumberOfContributorsThreshold = 2;
  public static readonly int ActiveMaintainerCommitsThreshold = 3;
  public static readonly int NumberOfActiveMaintainerThreshold = 2;

  public static (string, Status, string[]) Validate(DataHandler dataHandler) {
    var verbosityInfo = new List<string>();

    var contributorsCommitCountLastYear = dataHandler.GithubCommitsData?
      .GroupBy(x => x?.Author?.Login ?? "Unknown")
      .Select(x => new { Author = x.Key, Count = x.Count() })
      .OrderByDescending(x => x.Count)
      .ToDictionary(x => x.Author, x => x.Count);
    var filteredContributorsCommitLastYear = contributorsCommitCountLastYear?
      .Where(x => x.Value >= ActiveMaintainerCommitsThreshold)
      .ToDictionary(x => x.Key, x => x.Value);

    if (dataHandler.GithubContributorsCount is not null) {
      if (dataHandler.GithubContributorsCount < NumberOfContributorsThreshold) {
        verbosityInfo.Add($"Package has {dataHandler.GithubContributorsCount} contributors, which is less than the threshold of {NumberOfContributorsThreshold} contributors registered on GitHub");
        return ($"Package has less than {NumberOfContributorsThreshold} contributors!", Status.Fail, verbosityInfo.ToArray());
      }
      verbosityInfo.Add($"Package has {dataHandler.GithubContributorsCount} contributors registered on GitHub");
    }

    if (filteredContributorsCommitLastYear is null || filteredContributorsCommitLastYear.Count == 0) {
      verbosityInfo.Add($"Package have had no maintainers with {ActiveMaintainerCommitsThreshold} or more commits in the last year");
      return ($"Package has no active maintainers in the last year", Status.Fail, verbosityInfo.ToArray());
    }
    else if (filteredContributorsCommitLastYear.Count < NumberOfActiveMaintainerThreshold) {
      verbosityInfo.Add($"Package have had {filteredContributorsCommitLastYear.Count} active maintainers in the last year, which is less than the threshold of {NumberOfActiveMaintainerThreshold}");
      return ($"Package have had less than {NumberOfActiveMaintainerThreshold} active maintainers during the last year", Status.Error, verbosityInfo.ToArray());
    }

    return ($"Package has an adequate number of active contributors: {filteredContributorsCommitLastYear.Count}", Status.Pass, verbosityInfo.ToArray());
  }
}
