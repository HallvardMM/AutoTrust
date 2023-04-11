// Linked to TC: 16
namespace AutoTrust;

public class OpenPullRequests : ITrustCriteria {
  public static string Title => "Open Pull Requests";


  public static readonly string OneYearAgoString = DateTime.UtcNow.AddYears(-1).ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
  private static readonly double RatioOpenClosed = 0.2; // TODO:Is this good? Less than 20% of the total PRs should be open
  private static readonly double RatioOpenNewOld = 0.5; // TODO:Is this good? More than 50% of the open PRs should be addressed in the last year
  private static readonly double ToFewPullRequestsThreshold = 10;

  public static (string, Status, string[]) Validate(DataHandler dataHandler) {
    var verbosityInfo = new List<string>();

    if (dataHandler.GithubOpenPullRequestData is null || dataHandler.GithubOpenPullRequestData.TotalCount == 0) {
      verbosityInfo.Add("No open pull requests found");
      return ("Open pull requests of package failed criteria", Status.Fail, verbosityInfo.ToArray());
    }

    verbosityInfo.Add("Open pull requests found");

    var totalPullRequests = dataHandler.GithubOpenPullRequestData.TotalCount + dataHandler.GithubClosedPullRequestData?.TotalCount;
    if (totalPullRequests < ToFewPullRequestsThreshold) {
      verbosityInfo.Add($"Open pull requests {dataHandler.GithubOpenPullRequestData.TotalCount} and closed pull requests {dataHandler.GithubClosedPullRequestData?.TotalCount} combined is {totalPullRequests} which is less than {ToFewPullRequestsThreshold}");
      return ($"Less than {ToFewPullRequestsThreshold} open and closed pull requests. To few to evaluate.", Status.Error, verbosityInfo.ToArray());
    }

    verbosityInfo.Add($"Open pull requests {dataHandler.GithubOpenPullRequestData.TotalCount} and closed pull requests {dataHandler.GithubClosedPullRequestData?.TotalCount} combined is {totalPullRequests} which is more than {ToFewPullRequestsThreshold}");

    if ((double)dataHandler.GithubOpenPullRequestData.TotalCount / totalPullRequests >= RatioOpenClosed) {
      verbosityInfo.Add($"Ratio of open pull requests {dataHandler.GithubOpenPullRequestData.TotalCount} to closed pull requests {dataHandler.GithubClosedPullRequestData?.TotalCount} is more than " + RatioOpenClosed);
      return ($"Open pull requests of package failed. Ratio of open pull requests to open and closed pull requests is more than {RatioOpenClosed * 100}%", Status.Fail, verbosityInfo.ToArray());
    }
    verbosityInfo.Add($"Ratio of open pull requests {dataHandler.GithubOpenPullRequestData.TotalCount} to closed pull requests {dataHandler.GithubClosedPullRequestData?.TotalCount} is less than " + RatioOpenClosed);

    if (dataHandler.GithubUpdatedPullRequestData is null) {
      verbosityInfo.Add("No updated pull requests found");
      return ("Open pull requests of package failed. No updated open pull requests found!", Status.Fail, verbosityInfo.ToArray());
    }

    if ((double)dataHandler.GithubUpdatedPullRequestData.TotalCount / dataHandler.GithubOpenPullRequestData.TotalCount < RatioOpenNewOld) {
      verbosityInfo.Add($"There are {dataHandler.GithubUpdatedPullRequestData?.TotalCount} updated open pull requests since {OneYearAgoString} and {dataHandler.GithubOpenPullRequestData.TotalCount} open pull requests and the ratio between them is less than {RatioOpenNewOld}");
      return ($"Open pull requests of package failed. Less than {RatioOpenNewOld * 100}% of open pull requests are interacted with since {OneYearAgoString}", Status.Fail, verbosityInfo.ToArray());
    }

    verbosityInfo.Add($"There are {dataHandler.GithubUpdatedPullRequestData?.TotalCount} updated open pull requests since {OneYearAgoString} and {dataHandler.GithubOpenPullRequestData.TotalCount} open pull requests and the ratio between them is more than {RatioOpenNewOld}");

    return ("Open pull requests of package passed criteria", Status.Pass, verbosityInfo.ToArray());
  }
}
