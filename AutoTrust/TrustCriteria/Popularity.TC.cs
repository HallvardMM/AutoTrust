namespace AutoTrust;

public class Popularity : ITrustCriteria
{
  public string Title { get { return "Package Popularity"; } }

  private static long downloadsThreshold = 10000;

	public static Status validate(DataHandler dataHandler) {
    if (dataHandler.nugetDownloadCount == null) {
      PrettyPrint.FailPrint("Can't find download count for package");
      return Status.Error;
    }
    else if (dataHandler.nugetDownloadCount.Data[0].TotalDownloads > downloadsThreshold) {
      // Console.WriteLine("Package has " + dataHandler.nugetDownloadCount.Data[0].TotalDownloads + " total downloads");
      // Console.WriteLine("Package version has " + GetPackageVersionDownloadCount(dataHandler) + " downloads");
      PrettyPrint.SuccessPrint("Package download count: " + dataHandler.nugetDownloadCount.Data[0].TotalDownloads + " is higher than threshold: " + downloadsThreshold);
      return Status.Pass;
    }
    else {
      PrettyPrint.FailPrint("Package download count: " + dataHandler.nugetDownloadCount.Data[0].TotalDownloads + " is lower than threshold: " + downloadsThreshold);
      return Status.Error;}
  }

  	public static long GetPackageVersionDownloadCount(DataHandler dataHandler) {
    for (var i = 0; i < dataHandler.nugetDownloadCount?.Data[0].Versions.Count; i++) {
      if (dataHandler.nugetDownloadCount.Data[0].Versions[i]?.Version == dataHandler.packageVersion) {
        return dataHandler.nugetDownloadCount.Data[0].Versions[i].Downloads;
      }
    }
    throw new Exception("Could not find download count for package version");
	}
}
  
