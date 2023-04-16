// Linked to TC: 16
namespace AutoTrust;

public class OpenPullRequests : ITrustCriteria {
  public static string Title => "Open Pull Requests";
  public static int TotalScoreImportance => 3;

  public static readonly string OneYearAgoString = DateTime.UtcNow.AddYears(-1).ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
  private static readonly double RatioOpenClosed = 0.6;
  private static readonly double RatioOpenNewOld = 0.3;
  private static readonly double TooFewPullRequestsThreshold = 10;

  public static (string, Status, string[]) Validate(DataHandler dataHandler) {
    var verbosityInfo = new List<string>();

    if (dataHandler.GithubOpenPullRequestCount is null or 0) {
      verbosityInfo.Add("No open pull requests found");
      return ("Open pull requests of package failed criteria", Status.Fail, verbosityInfo.ToArray());
    }

    verbosityInfo.Add("Open pull requests found");

    var totalPullRequests = dataHandler.GithubOpenPullRequestCount + dataHandler.GithubClosedPullRequestCount;
    if (totalPullRequests < TooFewPullRequestsThreshold) {
      verbosityInfo.Add($"Open pull requests {dataHandler.GithubOpenPullRequestCount} and closed pull requests {dataHandler.GithubClosedPullRequestCount} combined is {totalPullRequests} which is less than {TooFewPullRequestsThreshold}");
      return ($"Less than {TooFewPullRequestsThreshold} open and closed pull requests. To few to evaluate.", Status.Error, verbosityInfo.ToArray());
    }

    verbosityInfo.Add($"Open pull requests {dataHandler.GithubOpenPullRequestCount} and closed pull requests {dataHandler.GithubClosedPullRequestCount} combined is {totalPullRequests} which is more than {TooFewPullRequestsThreshold}");

    if ((double)dataHandler.GithubOpenPullRequestCount / totalPullRequests >= RatioOpenClosed) {
      verbosityInfo.Add($"Ratio of open pull requests {dataHandler.GithubOpenPullRequestCount} to closed pull requests {dataHandler.GithubClosedPullRequestCount} is more than " + RatioOpenClosed);
      return ($"Open pull requests of package failed. Ratio of open pull requests to open and closed pull requests is more than {RatioOpenClosed * 100}%", Status.Fail, verbosityInfo.ToArray());
    }
    verbosityInfo.Add($"Ratio of open pull requests {dataHandler.GithubOpenPullRequestCount} to closed pull requests {dataHandler.GithubClosedPullRequestCount} is less than " + RatioOpenClosed);

    if (dataHandler.GithubUpdatedPullRequestData is null) {
      verbosityInfo.Add("No updated pull requests found");
      return ("Open pull requests of package failed. No updated open pull requests found!", Status.Fail, verbosityInfo.ToArray());
    }

    if ((double)dataHandler.GithubUpdatedPullRequestData.TotalCount / dataHandler.GithubOpenPullRequestCount < RatioOpenNewOld) {
      verbosityInfo.Add($"There are {dataHandler.GithubUpdatedPullRequestData?.TotalCount} updated open pull requests since {OneYearAgoString} and {dataHandler.GithubOpenPullRequestCount} open pull requests and the ratio between them is less than {RatioOpenNewOld}");
      return ($"Open pull requests of package failed. Less than {RatioOpenNewOld * 100}% of open pull requests are interacted with since {OneYearAgoString}", Status.Fail, verbosityInfo.ToArray());
    }

    verbosityInfo.Add($"There are {dataHandler.GithubUpdatedPullRequestData?.TotalCount} updated open pull requests since {OneYearAgoString} and {dataHandler.GithubOpenPullRequestCount} open pull requests and the ratio between them is more than {RatioOpenNewOld}");

    return ("Open pull requests of package passed criteria", Status.Pass, verbosityInfo.ToArray());
  }
}
