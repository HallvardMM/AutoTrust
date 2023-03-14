namespace AutoTrust;

public class Popularity : ITrustCriteria {
  public string Title => "Package Popularity";
  private static readonly long DownloadsThreshold = 10000;
  private static readonly long StargazersCountThreshold = 2;
  private static readonly long ForksCountThreshold = 1;
  private static readonly long WatchersThreshold = 1;

  public static Status Validate(DataHandler dataHandler) {
    // Check download count
    if (dataHandler.NugetDownloadCount == null) {
      PrettyPrint.FailPrint("Can't find download count for package");
      return Status.Error;
    }
    else if (dataHandler.NugetDownloadCount.Data[0].TotalDownloads <= DownloadsThreshold) {
      PrettyPrint.FailPrint("Package download count: " + dataHandler.NugetDownloadCount.Data[0].TotalDownloads + " is lower than threshold: " + DownloadsThreshold);
      return Status.Error;
    }

    // Check number of stars, forks and watchers on github
    if (dataHandler.GithubData == null) {
      PrettyPrint.FailPrint("Can't find github data for package");
      return Status.Error;
    }
    else if (dataHandler.GithubData.StargazersCount <= StargazersCountThreshold) {
      PrettyPrint.FailPrint("Package github stargazers count: " + dataHandler.GithubData.StargazersCount + " is lower than threshold: " + StargazersCountThreshold);
      return Status.Error;
    }
    else if (dataHandler.GithubData.ForksCount <= ForksCountThreshold) {
      PrettyPrint.FailPrint("Package github forks count: " + dataHandler.GithubData.ForksCount + " is lower than threshold: " + ForksCountThreshold);
      return Status.Error;
    }
    else if (dataHandler.GithubData.WatchersCount <= WatchersThreshold) {
      PrettyPrint.FailPrint("Package github watchers count: " + dataHandler.GithubData.WatchersCount + " is lower than threshold: " + WatchersThreshold);
      return Status.Error;
    }

    // Check number of projects that depend on this package

    PrettyPrint.SuccessPrint("Package popularity criteria passed");
    return Status.Pass;
  }

  public static long? GetPackageVersionDownloadCount(DataHandler dataHandler) {
    // Function for getting the download count for a specific package version
    for (var i = 0; i < dataHandler.NugetDownloadCount?.Data[0].Versions.Count; i++) {
      if (dataHandler.NugetDownloadCount.Data[0].Versions[i]?.Version == dataHandler.PackageVersion) {
        return dataHandler.NugetDownloadCount.Data[0].Versions[i].Downloads;
      }
    }
    return null;
  }
}

