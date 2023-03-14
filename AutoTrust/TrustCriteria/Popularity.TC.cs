namespace AutoTrust;

public class Popularity : ITrustCriteria
{
  public string Title { get { return "Package Popularity"; } }

  private static int downloadsThreshold = 1000;

	public static Status validate(DataHandler dataHandler) {
    if (dataHandler.nugetDownloadCount == null) {
      // this.errors.Add("Could not get download count for package");
      return Status.Error;
    }
    else {
      Console.WriteLine("Package has " + dataHandler.nugetDownloadCount + " downloads");
      Console.WriteLine(dataHandler.nugetDownloadCount.ToString(dataHandler.packageVersion));
    }
    // else if (dataHandler.nugetDownloadCount < this.downloadsThreshold) {
    //   this.errors.Add("Package has less than " + this.downloadsThreshold + " downloads");
    //   return Status.Fail;
    // }
    return Status.Pass;
  }
  
}