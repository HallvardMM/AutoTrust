namespace AutoTrust;

public class OpenIssues : ITrustCriteria {
  public static string Title => "Open Issues";


  public static readonly string OneYearAgoString = DateTime.UtcNow.AddYears(-1).ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
  private static readonly double RatioOpenClosed = 0.2; // TODO:Is this good? Less than 20% of the total issues should be open
  private static readonly double RatioOpenNewOld = 0.5; // TODO:Is this good? More than 50% of the open issues should be addressed in the last year

  public static (string, Status, string[]) Validate(DataHandler dataHandler) {
    // List of passed criteria
    var passedCriteria = new List<string>();

    if (dataHandler.GithubOpenIssueData is null || dataHandler.GithubOpenIssueData.TotalCount == 0) {
      passedCriteria.Add("No open issues found");
      return ("Open Issues of package failed criteria", Status.Fail, passedCriteria.ToArray());
    }

    passedCriteria.Add("Open issues found");

    var totalIssues = dataHandler.GithubOpenIssueData.TotalCount + dataHandler.GithubClosedIssueData?.TotalCount;
    if ((double)dataHandler.GithubOpenIssueData.TotalCount / totalIssues >= RatioOpenClosed) {
      passedCriteria.Add($"Ratio of open issues {dataHandler.GithubOpenIssueData.TotalCount} to closed issues {dataHandler.GithubClosedIssueData?.TotalCount} is more than " + RatioOpenClosed);
      return ($"Open Issues of package failed. Ratio of open issues to open and closed issues is more than {RatioOpenClosed * 100}%", Status.Fail, passedCriteria.ToArray());
    }
    passedCriteria.Add($"Ratio of open issues {dataHandler.GithubOpenIssueData.TotalCount} to closed issues {dataHandler.GithubClosedIssueData?.TotalCount} is less than " + RatioOpenClosed);

    if (dataHandler.GithubUpdatedIssueData is null) {
      passedCriteria.Add("No updated issues found");
      return ("Open Issues of package failed. No updated open issues found!", Status.Fail, passedCriteria.ToArray());
    }

    if ((double)dataHandler.GithubUpdatedIssueData.TotalCount / dataHandler.GithubOpenIssueData.TotalCount < RatioOpenNewOld) {
      passedCriteria.Add($"There are {dataHandler.GithubUpdatedIssueData?.TotalCount} updated open issues since {OneYearAgoString} and {dataHandler.GithubOpenIssueData.TotalCount} open issues and the ratio between them is less than {RatioOpenNewOld}");
      return ($"Open Issues of package failed. Less than {RatioOpenNewOld * 100}% of open issues are interacted with since {OneYearAgoString}", Status.Fail, passedCriteria.ToArray());
    }

    passedCriteria.Add($"There are {dataHandler.GithubUpdatedIssueData?.TotalCount} updated open issues since {OneYearAgoString} and {dataHandler.GithubOpenIssueData.TotalCount} open issues and the ratio between them is more than {RatioOpenNewOld}");

    return ("Open Issues of package passed criteria", Status.Pass, passedCriteria.ToArray());
  }
}
