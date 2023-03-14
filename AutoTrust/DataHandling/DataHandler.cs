namespace AutoTrust;

public class DataHandler
{
  public HttpClient httpClient { get; private set; }
	public string packageName { get; private set; }
	public string packageVersion { get; private set; }
  public NugetPackage? nugetPackage { get; private set; }
  public NugetCatalogEntry? nugetCatalogEntry { get; private set; }
  public NugetPackageManifest? packageManifest { get; private set; }
  public GithubPackage? githubData { get; private set; }
  public GithubIssues? githubIssueData { get; private set; }
  public NugetDownloadCount? nugetDownloadCount { get; private set; }
  public OSVData? osvData { get; private set; }

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