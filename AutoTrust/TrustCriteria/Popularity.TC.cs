namespace AutoTrust;

public class Popularity : ITrustCriteria {
  public string Title => "Package Popularity";
  private static readonly long downloadsThreshold = 10000;
  private static readonly long stargazersCountThreshold = 2;
  private static readonly long forksCountThreshold = 1; 
  private static readonly long watchersThreshold = 1; 

  public static Status validate(DataHandler dataHandler) {
    // Check download count
    if (dataHandler.nugetDownloadCount == null) {
      PrettyPrint.FailPrint("Can't find download count for package");
      return Status.Error;
    }
    else if (dataHandler.nugetDownloadCount.Data[0].TotalDownloads <= downloadsThreshold) {
      PrettyPrint.FailPrint("Package download count: " + dataHandler.nugetDownloadCount.Data[0].TotalDownloads + " is lower than threshold: " + downloadsThreshold);
      return Status.Error;
    }
    
    // Check number of stars, forks and watchers on github
    if (dataHandler.githubData == null) {
      PrettyPrint.FailPrint("Can't find github data for package");
      return Status.Error;
    }
    else if (dataHandler.githubData.StargazersCount <= stargazersCountThreshold) {
      PrettyPrint.FailPrint("Package github stargazers count: " + dataHandler.githubData.StargazersCount + " is lower than threshold: " + stargazersCountThreshold);
      return Status.Error;
    }
    else if (dataHandler.githubData.ForksCount <= forksCountThreshold) {
      PrettyPrint.FailPrint("Package github forks count: " + dataHandler.githubData.ForksCount + " is lower than threshold: " + forksCountThreshold);
      return Status.Error;
    }
    else if (dataHandler.githubData.WatchersCount <= watchersThreshold) {
      PrettyPrint.FailPrint("Package github watchers count: " + dataHandler.githubData.WatchersCount + " is lower than threshold: " + watchersThreshold);
      return Status.Error;
    }

    // Check number of projects that depend on this package

    PrettyPrint.SuccessPrint("Package popularity criteria passed");
    return Status.Pass;
  }

  public static long? GetPackageVersionDownloadCount(DataHandler dataHandler) {
    // Function for getting the download count for a specific package version
    for (var i = 0; i < dataHandler.nugetDownloadCount?.Data[0].Versions.Count; i++) {
      if (dataHandler.nugetDownloadCount.Data[0].Versions[i]?.Version == dataHandler.packageVersion) {
        return dataHandler.nugetDownloadCount.Data[0].Versions[i].Downloads;
      }
    }
    return null;
  }
}

