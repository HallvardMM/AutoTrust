namespace AutoTrust;

public class OpenIssues : ITrustCriteria {
  public static string Title => "Open Issues";
  public static int TotalScoreImportance => 3;

  public static readonly string OneYearAgoString = DateTime.UtcNow.AddYears(-1).ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
  private static readonly double RatioOpenClosed = 0.6; // TODO:Is this good? Less than 60% of the total issues should be open
  private static readonly double RatioOpenNewOld = 0.3; // TODO:Is this good? More than 30% of the open issues should be addressed in the last year
  private static readonly double TooFewIssuesThreshold = 30;

  public static (string, Status, string[]) Validate(DataHandler dataHandler) {
    // List of passed criteria
    var verbosityInfo = new List<string>();

    if (dataHandler.GithubOpenIssueData is null || dataHandler.GithubOpenIssueData.TotalCount == 0) {
      verbosityInfo.Add("No open issues found");
      return ("Open Issues of package failed criteria", Status.Fail, verbosityInfo.ToArray());
    }

    verbosityInfo.Add("Open issues found");

    var totalIssues = dataHandler.GithubOpenIssueData.TotalCount + dataHandler.GithubClosedIssueData?.TotalCount;
    if (totalIssues < TooFewIssuesThreshold) {
      verbosityInfo.Add($"Open issues {dataHandler.GithubOpenIssueData.TotalCount} and closed issues {dataHandler.GithubClosedIssueData?.TotalCount} combined is {totalIssues} which is less than {TooFewIssuesThreshold}");
      return ($"Less than {TooFewIssuesThreshold} open and closed issues. To few to evaluate.", Status.Error, verbosityInfo.ToArray());
    }

    verbosityInfo.Add($"Open issues {dataHandler.GithubOpenIssueData.TotalCount} and closed issues {dataHandler.GithubClosedIssueData?.TotalCount} combined is {totalIssues} which is more than {TooFewIssuesThreshold}");

    if ((double)dataHandler.GithubOpenIssueData.TotalCount / totalIssues >= RatioOpenClosed) {
      verbosityInfo.Add($"Ratio of open issues {dataHandler.GithubOpenIssueData.TotalCount} to closed issues {dataHandler.GithubClosedIssueData?.TotalCount} is more than " + RatioOpenClosed);
      return ($"Open Issues of package failed. Ratio of open issues to open and closed issues is more than {RatioOpenClosed * 100}%", Status.Fail, verbosityInfo.ToArray());
    }
    verbosityInfo.Add($"Ratio of open issues {dataHandler.GithubOpenIssueData.TotalCount} to closed issues {dataHandler.GithubClosedIssueData?.TotalCount} is less than " + RatioOpenClosed);

    if (dataHandler.GithubUpdatedIssueData is null) {
      verbosityInfo.Add("No updated issues found");
      return ("Open Issues of package failed. No updated open issues found!", Status.Fail, verbosityInfo.ToArray());
    }

    if ((double)dataHandler.GithubUpdatedIssueData.TotalCount / dataHandler.GithubOpenIssueData.TotalCount < RatioOpenNewOld) {
      verbosityInfo.Add($"There are {dataHandler.GithubUpdatedIssueData?.TotalCount} updated open issues since {OneYearAgoString} and {dataHandler.GithubOpenIssueData.TotalCount} open issues and the ratio between them is less than {RatioOpenNewOld}");
      return ($"Open Issues of package failed. Less than {RatioOpenNewOld * 100}% of open issues are interacted with since {OneYearAgoString}", Status.Fail, verbosityInfo.ToArray());
    }

    verbosityInfo.Add($"There are {dataHandler.GithubUpdatedIssueData?.TotalCount} updated open issues since {OneYearAgoString} and {dataHandler.GithubOpenIssueData.TotalCount} open issues and the ratio between them is more than {RatioOpenNewOld}");

    return ("Open Issues of package passed criteria", Status.Pass, verbosityInfo.ToArray());
  }
}
