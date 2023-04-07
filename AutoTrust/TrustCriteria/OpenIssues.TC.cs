namespace AutoTrust;

public class OpenIssues : ITrustCriteria {
  public static string Title => "Open Issues";


  public static readonly string OneYearAgoString = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
  private static readonly double RatioOpenClosed = 0.2;
  private static readonly double RatioOpenNewOld = 0.2;

  public static (string, Status, string[]) Validate(DataHandler dataHandler) {
    // Check if the package version is older than a certain threshold
    // Check if the package version is a pre-release version

    // List of passed criteria
    var passedCriteria = new List<string>();



    return ("Open Issues of package passed criteria", Status.Pass, passedCriteria.ToArray());
  }
}
