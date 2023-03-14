namespace AutoTrust;

public class Popularity : ITrustCriteria {
  public string Title => "Package Popularity";
  private static readonly long downloadsThreshold = 10000;

  public static Status validate(DataHandler dataHandler) {
    if (dataHandler.nugetDownloadCount == null) {
      PrettyPrint.FailPrint("Can't find download count for package");
      return Status.Error;
    }
    else if (dataHandler.nugetDownloadCount.Data[0].TotalDownloads > downloadsThreshold) {
      PrettyPrint.SuccessPrint("Package download count: " + dataHandler.nugetDownloadCount.Data[0].TotalDownloads + " is higher than threshold: " + downloadsThreshold);
      return Status.Pass;
    }
    else {
      PrettyPrint.FailPrint("Package download count: " + dataHandler.nugetDownloadCount.Data[0].TotalDownloads + " is lower than threshold: " + downloadsThreshold);
      return Status.Error;
    }
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

