namespace AutoTrust;

public class DataHandler
{
  public HttpClient httpClient { get; set; }
	public string packageName;
	public string packageVersion;
  public NugetPackage? nugetPackage;
  public NugetCatalogEntry? nugetCatalogEntry;
  public NugetPackageManifest? packageManifest;
  public GithubPackage? githubData;
  public GithubIssues? githubIssueData;
  public NugetDownloadCount? nugetDownloadCount;
  public OSVData? osvData;

	public DataHandler(string packageName, string packageVersion) {
    this.packageName = packageName;
    this.packageVersion = packageVersion;
    this.nugetPackage = null;
    this.nugetCatalogEntry = null;
    this.packageManifest = null;
    httpClient = new HttpClient();
  }

  public async Task fetchData() {
    this.nugetPackage = await NugetPackage.GetNugetPackage(httpClient, packageName, packageVersion);
    
    // Get the package catalog entry with a lot of data such as potential vulnerabilities
    if (nugetPackage?.CatalogEntry != null) {
      this.nugetCatalogEntry = await NugetCatalogEntry.GetNugetCatalogEntry(httpClient, nugetPackage.CatalogEntry);
    }

    this.packageManifest = await NugetPackageManifest.GetNugetPackageManifest(httpClient, packageName, packageVersion);

    var repositoryUrl = "";

    if (packageManifest?.Metadata.Repository?.Url?.ToLower(System.Globalization.CultureInfo.CurrentCulture).Contains("github.com") ?? false) {
      repositoryUrl = packageManifest.Metadata.Repository.Url;
    }
    else if (packageManifest?.Metadata.ProjectUrl?.ToLower(System.Globalization.CultureInfo.CurrentCulture).Contains("github.com") ?? false) {
      repositoryUrl = packageManifest.Metadata.ProjectUrl;
    }

    if (repositoryUrl != "") {
      this.githubData = await GithubPackage.GetGithubPackage(httpClient, repositoryUrl);
      this.githubIssueData = await GithubIssues.GetGithubIssues(httpClient, repositoryUrl);
    }

    this.nugetDownloadCount = await NugetDownloadCount.GetNugetDownloadCount(httpClient, packageName);
    this.osvData = await OSVData.GetOSVData(httpClient, packageName, packageVersion);    
    }
}