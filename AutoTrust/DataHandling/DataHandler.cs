namespace AutoTrust;

public class DataHandler {
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

  public DataHandler(HttpClient httpClient, string packageName, string packageVersion) {
    this.packageName = packageName;
    this.httpClient = httpClient;
    this.packageVersion = packageVersion;
    this.nugetPackage = null;
    this.nugetCatalogEntry = null;
    this.packageManifest = null;
  }

  public async Task fetchData() {
    this.nugetPackage = await NugetPackage.GetNugetPackage(this.httpClient, this.packageName, this.packageVersion);

    // Get the package catalog entry with a lot of data such as potential vulnerabilities
    if (this.nugetPackage?.CatalogEntry != null) {
      this.nugetCatalogEntry = await NugetCatalogEntry.GetNugetCatalogEntry(this.httpClient, this.nugetPackage.CatalogEntry);
    }

    this.packageManifest = await NugetPackageManifest.GetNugetPackageManifest(this.httpClient, this.packageName, this.packageVersion);

    var repositoryUrl = "";

    if (this.packageManifest?.Metadata.Repository?.Url?.ToLower(System.Globalization.CultureInfo.CurrentCulture).Contains("github.com") ?? false) {
      repositoryUrl = this.packageManifest.Metadata.Repository.Url;
    }
    else if (this.packageManifest?.Metadata.ProjectUrl?.ToLower(System.Globalization.CultureInfo.CurrentCulture).Contains("github.com") ?? false) {
      repositoryUrl = this.packageManifest.Metadata.ProjectUrl;
    }

    if (repositoryUrl != "") {
      this.githubData = await GithubPackage.GetGithubPackage(this.httpClient, repositoryUrl);
      this.githubIssueData = await GithubIssues.GetGithubIssues(this.httpClient, repositoryUrl);
    }

    this.nugetDownloadCount = await NugetDownloadCount.GetNugetDownloadCount(this.httpClient, this.packageName);
    this.osvData = await OSVData.GetOSVData(this.httpClient, this.packageName, this.packageVersion);
  }
}
