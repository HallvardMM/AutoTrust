namespace AutoTrust;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

public class GithubReadme {
  [JsonPropertyName("name")]
  public string Name { get; set; } = string.Empty;
  [JsonPropertyName("path")]
  public string Path { get; set; } = string.Empty;
  [JsonPropertyName("sha")]
  public string Sha { get; set; } = string.Empty;
  [JsonPropertyName("size")]
  public long Size { get; set; }
  [JsonPropertyName("url")]
  public string Url { get; set; } = string.Empty;
  [JsonPropertyName("html_url")]
  public string HtmlUrl { get; set; } = string.Empty;
  [JsonPropertyName("git_url")]
  public string GitUrl { get; set; } = string.Empty;
  [JsonPropertyName("download_url")]
  public string DownloadUrl { get; set; } = string.Empty;
  [JsonPropertyName("type")]
  public string Type { get; set; } = string.Empty;
  [JsonPropertyName("content")]
  public string Content { get; set; } = string.Empty;
  [JsonPropertyName("encoding")]
  public string Encoding { get; set; } = string.Empty;
  [JsonPropertyName("_links")]
  public Links? Links { get; set; }

  public override string ToString() {
    if (this.HtmlUrl == "") {
      return "No documentation found!";
    }
    return this.HtmlUrl;
  }

  public static async Task<GithubReadme?> GetGithubReadme(HttpClient httpClient, string authorAndProject, bool isDiagnostic) {
    var githubReadmeUrl = GetGithubReadmeUrl(authorAndProject);
    try {
      // Fetch package data
      var githubReadmeData = await httpClient.GetFromJsonAsync<GithubReadme>(githubReadmeUrl);
      if (isDiagnostic) {
        Console.WriteLine($"Found readme data for {authorAndProject} from {githubReadmeUrl}");
      }
      return githubReadmeData;
    }
    catch (HttpRequestException ex) {
      // Handle any exceptions thrown by the HTTP client.
      Console.WriteLine($"Error: An HTTP error occurred for {authorAndProject} from {githubReadmeUrl}: {ex.Message}");
    }
    catch (JsonException ex) {
      // Handle any exceptions thrown during JSON deserialization.
      Console.WriteLine($"Error: A JSON error occurred for {authorAndProject} from {githubReadmeUrl}: {ex.Message}");
    }
    return null;
  }

  public static string GetGithubReadmeUrl(string authorAndProject) => "https://api.github.com/repos/" + authorAndProject + "/readme";
}

public class Links {
  [JsonPropertyName("self")]
  public string Self { get; set; } = string.Empty;
  [JsonPropertyName("git")]
  public string Git { get; set; } = string.Empty;
  [JsonPropertyName("html")]
  public string Html { get; set; } = string.Empty;
}
