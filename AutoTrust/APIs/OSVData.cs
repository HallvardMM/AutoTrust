using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel;
using System.Net;

namespace AutoTrust
{
  public class OSVData
  {
    [JsonPropertyName("vulns")]
    public List<OSVVulnerabilities> Vulns { get; set; }
 
    public override string ToString()
    {
      string returnString = "";
      if(Vulns == null)
      {
        return "No known vulnerabilities found in the OSV database.\n";
      }
      foreach (OSVVulnerabilities vuln in Vulns)
      {
        returnString += vuln.ToString();
      }
      return returnString;
    }

    public async static Task<OSVData?> GetOSVData(HttpClient httpClient, string packageName, string packageVersion)
    {
      try
      {
        // Fetch package data
        var osvJSONPost = 
          $"{{\"version\": \"{packageVersion}\", \"package\": {{\"name\":\"{packageName}\",\"ecosystem\":\"NuGet\"}}}}";

        var content = new StringContent(osvJSONPost, System.Text.Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("https://api.osv.dev/v1/query", content);
        if (response.StatusCode == HttpStatusCode.OK)
        {
          var responseStream = await response.Content.ReadAsStreamAsync();
          var osvData = await JsonSerializer.DeserializeAsync<OSVData>(responseStream);
          return osvData;
        }
        else
        {
          string errorResponse = await response.Content.ReadAsStringAsync();
          Console.WriteLine($"An error occurred. Status code: {response.StatusCode}. Error message: {errorResponse}");
        }
      }
      catch (HttpRequestException ex)
      {
        // Handle any exceptions thrown by the HTTP client.
        Console.WriteLine($"An HTTP error occurred: {ex.Message}");
      }
      catch (JsonException ex)
      {
        // Handle any exceptions thrown during JSON deserialization.
        Console.WriteLine($"A JSON error occurred: {ex.Message}");
      }
      return null;
    }

  }

  public class OSVVulnerabilities
  {
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("summary")]
    public string Summary { get; set; }
    [JsonPropertyName("details")]
    public string Details { get; set; }
    [JsonPropertyName("modified")]
    public string Modified { get; set; }
    [JsonPropertyName("published")]
    public string Published { get; set; }
    [JsonPropertyName("database_specific")]
    public DatabaseSpecific DatabaseSpecific { get; set; }
    [JsonPropertyName("references")]
    public List<Reference> References { get; set; }
    [JsonPropertyName("affected")]
    public List<Affected> Affected { get; set; }
    [JsonPropertyName("schema_version")]
    public string SchemaVersion { get; set; }
    [JsonPropertyName("severity")]
    public List<Severity> Severity { get; set; }

    public override string ToString()
    {
      string stringReferences = "";
      foreach (Reference reference in References){
        stringReferences += reference.ToString();
      }
      string affectedReferences = "";
      foreach (Affected affected in Affected)
      {
        affectedReferences += affected.ToString();
      }
      return
        $"Id: {Id}\n" +
        $"Summary: {Summary}\n" +
        $"Details: {Details}\n" +
        $"Published: {Published}\n" +
         DatabaseSpecific.ToString() +
        $"Refrences: \n{stringReferences}" +
        $"Affected: \n{affectedReferences}";
    }
  }

  public class DatabaseSpecific
  {
    [JsonPropertyName("cwe_ids")]
    public List<string> CweIds { get; set; }
    [JsonPropertyName("severity")]
    public string Severity { get; set; }
    [JsonPropertyName("github_reviewed")]
    public bool GithubReviewed { get; set; }
    [JsonPropertyName("github_reviewed_at")]
    public DateTimeOffset GithubReviewedAt { get; set; }
    [JsonPropertyName("nvd_published_at")]
    public DateTimeOffset? NvdPublishedAt { get; set; }

    public override string ToString()
    {
      return $"CWE IDs: [{string.Join(", ", CweIds)}] Severity: {Severity} \n";
    }
  }

  public class Reference
  {
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("url")]
    public string Url { get; set; }

    public override string ToString()
    {
      return $"- {Type}: {Url}\n";
    }
  }

  public class Severity
  {
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("score")]
    public string Score { get; set; }

    public override string ToString()
    {
      return $"Severity: Type: {Type} Score: {Score}";
    }
  }

  public class Affected
  {
    [JsonPropertyName("package")]
    public OSVPackage Package { get; set; }
    [JsonPropertyName("ranges")]
    public List<Range> Ranges { get; set; }
    [JsonPropertyName("versions")]
    public List<string> Versions { get; set; }
    [JsonPropertyName("database_specific")]
    public OSVSource Source { get; set; }

    public override string ToString()
    {
      string returnString = "";
      foreach (Range range in Ranges)
      {
        returnString += range.ToString();
      }
      returnString += Source.ToString();
      return returnString;
    }
  }

  public class OSVPackage
  {
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("ecosystem")]
    public string Ecosystem { get; set; }
    [JsonPropertyName("purl")]
    public string Purl { get; set; }

    public override string ToString()
    {
      return $"Name: {Name} Ecosystem: {Ecosystem} Purl: {Purl}\n";
    }

  }

  public class OSVSource
  {
    [JsonPropertyName("source")]
    public string Source { get; set; }
    public override string ToString()
    {
      return $"Source: {Source}\n";
    }
  }

  public class Range
  {
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("events")]
    public List<Dictionary<string, string>> Events { get; set; }

    public override string ToString()
    {
      string returnString = $"- Type: {Type}\n";
      foreach (Dictionary<string, string> dict in Events)
      {
        foreach (KeyValuePair<string, string> pair in dict)
        {
          returnString += $"- {pair.Key}: {pair.Value}\n";
        }
      }
      return returnString;
    }
  }
}