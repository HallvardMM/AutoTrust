namespace AutoTrust;

public class DataHandler {
  public HttpClient HttpClient { get; private set; }
  public string PackageName { get; private set; }
  public string PackageVersion { get; private set; }
  public NugetPackage? NugetPackage { get; private set; }
  public NugetCatalogEntry? NugetCatalogEntry { get; private set; }
  public NugetPackageManifest? PackageManifest { get; private set; }
  public System.Collections.Concurrent.ConcurrentDictionary<string, DependencyNode>? DependencyTree { get; private set; }
  public GithubPackage? GithubData { get; private set; }
  public GithubIssues? GithubIssueData { get; private set; }
  public NugetDownloadCount? NugetDownloadCount { get; private set; }
  public OSVData? OsvData { get; private set; }
  public string UsedByInformation { get; private set; }

  public DataHandler(HttpClient httpClient, string packageName, string packageVersion) {
    this.PackageName = packageName;
    this.HttpClient = httpClient;
    this.PackageVersion = packageVersion;
    this.NugetPackage = null;
    this.NugetCatalogEntry = null;
    this.PackageManifest = null;
    this.UsedByInformation = "";
  }

  public async Task FetchData() {
    var tasks = new List<Task> {
      Task.Run(async () => {
        this.NugetPackage = await NugetPackage.GetNugetPackage(this.HttpClient, this.PackageName, this.PackageVersion);

        // Get the package catalog entry with a lot of data such as potential vulnerabilities
        if (this.NugetPackage?.CatalogEntry != null) {
          this.NugetCatalogEntry = await NugetCatalogEntry.GetNugetCatalogEntry(this.HttpClient, this.NugetPackage.CatalogEntry);
        }
        
        // Build dependency tree after getting the catalog entry
        this.DependencyTree = await DependencyTreeBuilder.GetDependencyTree(this, new System.Collections.Concurrent.ConcurrentDictionary<string, DependencyNode>(), this.PackageName, this.PackageVersion);
      }),
      Task.Run(async () => {
        this.PackageManifest = await NugetPackageManifest.GetNugetPackageManifest(this.HttpClient, this.PackageName, this.PackageVersion);

        var repositoryUrl = "";

        if (this.PackageManifest?.Metadata.Repository?.Url?.ToLower(System.Globalization.CultureInfo.CurrentCulture).Contains("github.com") ?? false) {
          repositoryUrl = this.PackageManifest.Metadata.Repository.Url;
        }
        else if (this.PackageManifest?.Metadata.ProjectUrl?.ToLower(System.Globalization.CultureInfo.CurrentCulture).Contains("github.com") ?? false) {
          repositoryUrl = this.PackageManifest.Metadata.ProjectUrl;
        }

        if (repositoryUrl != "") {
          this.GithubData = await GithubPackage.GetGithubPackage(this.HttpClient, repositoryUrl);
          this.GithubIssueData = await GithubIssues.GetGithubIssues(this.HttpClient, repositoryUrl);
        }
      }),
      Task.Run(async () => this.NugetDownloadCount = await NugetDownloadCount.GetNugetDownloadCount(this.HttpClient, this.PackageName)),
      Task.Run(async () => this.OsvData = await OSVData.GetOSVData(this.HttpClient, this.PackageName, this.PackageVersion)),
      // Task.Run(async () => this.DeprecatedNugetPackages = await Deprecated.GetDeprecatedPackages(this)),

      Task.Run(async () => {
        try {
          this.UsedByInformation = await this.HttpClient.GetStringAsync($"https://www.nuget.org/packages/{this.PackageName}/{this.PackageVersion}#usedby-body-tab");
        }
        catch (HttpRequestException) {
          this.UsedByInformation = "";
        }
      })
    };

    var t = Task.WhenAll(tasks.ToArray());
    try {
      await t;
    }
    catch { }
  }
}
