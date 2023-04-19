namespace AutoTrust;

using System.Net.Http.Json;
using System.Text.Json;

public class DataHandler {
  public HttpClient HttpClient { get; private set; }
  public string PackageName { get; private set; }
  public string PackageVersion { get; private set; }
  public bool IsPrerelease { get; private set; }
  public NugetPackage? NugetPackage { get; private set; }
  public NugetCatalogEntry? NugetCatalogEntry { get; private set; }
  public NugetPackageManifest? PackageManifest { get; private set; }
  public System.Collections.Concurrent.ConcurrentDictionary<string, DependencyNode>? DependencyTree { get; private set; }
  public GithubPackage? GithubData { get; private set; }
  public int? GithubOpenIssueCount { get; private set; }
  public int? GithubClosedIssueCount { get; private set; }
  public GithubIssuesSearch? GithubUpdatedIssueData { get; private set; }
  public int? GithubOpenPullRequestCount { get; private set; }
  public int? GithubClosedPullRequestCount { get; private set; }
  public GithubPullRequestsSearch? GithubUpdatedPullRequestData { get; private set; }
  public GithubReadme? GithubReadmeData { get; private set; }
  public List<GithubContributor?>? GithubContributorsData { get; private set; }
  public List<GithubCommit?>? GithubCommitsData { get; private set; }
  public int? GithubContributorsCount { get; private set; }
  public NugetDownloadCount? NugetDownloadCount { get; private set; }
  public OSVData? OsvData { get; private set; }
  public string UsedByInformation { get; private set; }
  public DateTimeOffset? OldestPublishedDate { get; private set; }
  public string? GithubToken { get; private set; }

  public DataHandler(HttpClient httpClient, string packageName, string packageVersion, bool prerelease) {
    this.PackageName = packageName;
    this.HttpClient = httpClient;
    this.PackageVersion = packageVersion;
    this.IsPrerelease = prerelease;
    this.NugetPackage = null;
    this.NugetCatalogEntry = null;
    this.PackageManifest = null;
    this.UsedByInformation = "";
    this.GithubToken = GetGithubToken();
  }

  public static string? GetGithubToken() {
    string? token = null;
    try {
      if (OperatingSystem.IsWindows()) {
        token =
        Environment.GetEnvironmentVariable("GITHUB_API_TOKEN") ??
        Environment.GetEnvironmentVariable("GITHUB_API_TOKEN", EnvironmentVariableTarget.User) ??
        Environment.GetEnvironmentVariable("GITHUB_API_TOKEN", EnvironmentVariableTarget.Machine);
      }
      else if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS()) {
        token = Environment.GetEnvironmentVariable("GITHUB_API_TOKEN");
      }
    }
    catch (Exception) {
    }
    return token;
  }

  public static async Task<T?> FetchGithubData<T>(HttpClient httpClient,
  string? githubToken, string url, string authorAndProject, bool isDiagnostic, string diagnosticText) {
    // https://www.stevejgordon.co.uk/sending-and-receiving-json-using-httpclient-with-system-net-http-json
    using (var request = new HttpRequestMessage(HttpMethod.Get, url)) {
      if (githubToken != null) {
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", githubToken);
      }
      request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
      request.Headers.UserAgent.TryParseAdd("request");
      using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)) {
        if (isDiagnostic) {
          Console.WriteLine(diagnosticText);
        }
        if (response.IsSuccessStatusCode) {
          try {
            return await response.Content.ReadFromJsonAsync<T>();
          }
          catch (JsonException ex) {
            // Handle any exceptions thrown during JSON deserialization.
            if (isDiagnostic) {
              Console.WriteLine($"Error: A JSON error occurred for {authorAndProject} from {url}: {ex.Message}");
            }
          }
        }
        else {
          if (isDiagnostic) {
            Console.WriteLine($"Error: An HTTP error occurred for {authorAndProject} from {url}: {response.StatusCode}");
          }
        }
      }
    }
    return default;
  }

  public static async Task<int?> FetchGithubHeaderCount(HttpClient httpClient,
  string? githubToken, string url, string authorAndProject, bool isDiagnostic, string diagnosticText) {
    // https://www.stevejgordon.co.uk/sending-and-receiving-json-using-httpclient-with-system-net-http-json
    using (var request = new HttpRequestMessage(HttpMethod.Get, url)) {
      if (githubToken != null) {
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", githubToken);
      }
      request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
      request.Headers.UserAgent.TryParseAdd("request");
      using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)) {
        if (isDiagnostic) {
          Console.WriteLine(diagnosticText);
        }
        if (response.IsSuccessStatusCode) {
          return HelperFunctions.GetLastPageNumber(response.Headers.GetValues("Link").FirstOrDefault());
        }
        else {
          if (isDiagnostic) {
            Console.WriteLine($"Error: An HTTP error occurred for {authorAndProject} from {url}: {response.StatusCode}");
          }
        }
      }
    }
    return null;
  }

  public async Task FetchData(bool isDiagnostic) {

    var (oldestVersion, latestVersion) = await NugetPackageVersion.GetLatestVersion(this.HttpClient, this.PackageName, this.IsPrerelease, isDiagnostic);
    if (this.PackageVersion is "") {
      if (latestVersion != null) {
        this.PackageVersion = latestVersion;
      }
      else {
        Console.WriteLine("Error: Package version not found!");
        return;
      }
    }

    var tasks = new List<Task> {
      Task.Run(async () => {
        this.NugetPackage = await NugetPackage.GetNugetPackage(this.HttpClient, this.PackageName, this.PackageVersion, isDiagnostic);

        // Get the package catalog entry with a lot of data such as potential vulnerabilities
        if (this.NugetPackage?.CatalogEntry != null) {
          this.NugetCatalogEntry = await NugetCatalogEntry.GetNugetCatalogEntry(this.HttpClient, this.NugetPackage.CatalogEntry, isDiagnostic);
        }
        
        // Build dependency tree after getting the catalog entry
        this.DependencyTree = await DependencyTreeBuilder.GetDependencyTree(this,
        new System.Collections.Concurrent.ConcurrentDictionary<string, DependencyNode>(),
        this.PackageName, this.PackageVersion, isDiagnostic);
      }),
      Task.Run(async () => {
        if(oldestVersion != null){
        // Get the package catalog entry with a lot of data such as potential vulnerabilities
        var oldPackage = await NugetPackage.GetNugetPackage(this.HttpClient, this.PackageName, oldestVersion, isDiagnostic);
        if (oldPackage?.Published != null) {
          this.OldestPublishedDate = oldPackage.Published;
        }
        }
      }),
      Task.Run(async () => {
        this.PackageManifest = await NugetPackageManifest.GetNugetPackageManifest(this.HttpClient, this.PackageName, this.PackageVersion, isDiagnostic);

        var repositoryUrl = "";

        if (!string.IsNullOrEmpty(this.PackageManifest?.Metadata.Repository?.Url) && (this.PackageManifest?.Metadata.Repository?.Url?.ToLowerInvariant().Contains("github.com") ?? false)) {
          repositoryUrl = this.PackageManifest.Metadata.Repository.Url;
        }
        else if (!string.IsNullOrEmpty(this.PackageManifest?.Metadata.ProjectUrl) && (this.PackageManifest?.Metadata.ProjectUrl?.ToLowerInvariant().Contains("github.com") ?? false)) {
          repositoryUrl = this.PackageManifest.Metadata.ProjectUrl;
        }
        else{
          Console.WriteLine("\rError: No Github repository found Github data for package, issues and readme will not be checked!");
          return;
        }

        //Extract the author and project from the repository url
        // https://github.com/aws/aws-sdk-net/
        // https://github.com/castleproject/Core
        // https://github.com/serilog/serilog.git
        // https://github.com/JamesNK/Newtonsoft.Json

        var authorAndProject = repositoryUrl.Replace("https://github.com/", "");
        if (authorAndProject.EndsWith(".git", StringComparison.InvariantCultureIgnoreCase)) {
          authorAndProject = authorAndProject[..^4];
        }
        if (authorAndProject.EndsWith("/", StringComparison.InvariantCultureIgnoreCase)) {
          authorAndProject = authorAndProject[..^1];
        }

        if (authorAndProject != "") {
          var tasks = new List<Task> {
          Task.Run(async () => this.GithubData = await GithubPackage.GetGithubPackage(this.HttpClient, this.GithubToken, authorAndProject, isDiagnostic)),
          Task.Run(async () => this.GithubReadmeData = await GithubReadme.GetGithubReadme(this.HttpClient, this.GithubToken, authorAndProject, isDiagnostic)),
          Task.Run(async () => (this.GithubContributorsData, this.GithubContributorsCount) = await GithubContributor.GetGithubContributors(this.HttpClient, this.GithubToken, authorAndProject, isDiagnostic)),
          Task.Run(async () => this.GithubCommitsData = await GithubCommit.GetGithubCommits(this.HttpClient, this.GithubToken, authorAndProject, isDiagnostic)),
          Task.Run(async () => this.GithubOpenIssueCount = await GithubIssuesRepos.GetGithubIssues(this.HttpClient, this.GithubToken, authorAndProject, GithubIssuesRepos.GetOpenGithubIssuesUrl(authorAndProject), isDiagnostic)),
          Task.Run(async () => this.GithubClosedIssueCount = await GithubIssuesRepos.GetGithubIssues(this.HttpClient,this.GithubToken, authorAndProject, GithubIssuesRepos.GetClosedGithubIssuesUrl(authorAndProject), isDiagnostic)),
          Task.Run(async () => this.GithubUpdatedIssueData = await GithubIssuesSearch.GetGithubIssues(this.HttpClient, this.GithubToken, authorAndProject, GithubIssuesSearch.GetUpdatedGithubIssuesUrl(authorAndProject,OpenIssues.OneYearAgoString), isDiagnostic)),
          Task.Run(async () => this.GithubOpenPullRequestCount = await GithubPullRequestsRepos.GetGithubPullRequestsRepos(this.HttpClient,this.GithubToken, authorAndProject, GithubPullRequestsRepos.GetOpenGithubPullRequestsUrl(authorAndProject), isDiagnostic)),
          Task.Run(async () => this.GithubClosedPullRequestCount = await GithubPullRequestsRepos.GetGithubPullRequestsRepos(this.HttpClient,this.GithubToken, authorAndProject, GithubPullRequestsRepos.GetClosedGithubPullRequestsUrl(authorAndProject), isDiagnostic)),
          Task.Run(async () => this.GithubUpdatedPullRequestData = await GithubPullRequestsSearch.GetGithubPullRequestsSearch(this.HttpClient,this.GithubToken, authorAndProject, GithubPullRequestsSearch.GetUpdatedGithubPullRequestsUrl(authorAndProject, OpenPullRequests.OneYearAgoString), isDiagnostic)),
        };
          var t = Task.WhenAll(tasks.ToArray());
          try {
            await t;
          }
          catch { }
        }else{
          Console.WriteLine($"\rError: No author and project found in url: {repositoryUrl}");
        };
      }),
      Task.Run(async () => {
        this.NugetDownloadCount = await NugetDownloadCount.GetNugetDownloadCount(this.HttpClient, this.PackageName, this.IsPrerelease, isDiagnostic);
        if (this.NugetDownloadCount?.Data[0].PackageName != null) {
          this.OsvData = await OSVData.GetOSVData(this.HttpClient, this.NugetDownloadCount.Data[0].PackageName, isDiagnostic);
        }
      }
      ),

      Task.Run(async () => {
        var usedByUrl = $"https://www.nuget.org/packages/{this.PackageName}/{this.PackageVersion}#usedby-body-tab";
        try {
          this.UsedByInformation = await this.HttpClient.GetStringAsync(usedByUrl);
          if(isDiagnostic){
            Console.WriteLine($"\rFound used by information from: {usedByUrl}");
          }
        }
        catch (HttpRequestException) {
          this.UsedByInformation = "";
          if(isDiagnostic){
            Console.WriteLine($"\rError: Failed to fetch used by information from {usedByUrl}");
          }
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
