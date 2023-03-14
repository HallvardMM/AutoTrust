// API docs: https://osv.dev/docs/#tag/api/operation/OSV_QueryAffectedBatch
// Info about properties details and format: https://ossf.github.io/osv-schema/

namespace AutoTrust;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

public class OSVData {
  [JsonPropertyName("vulns")]
  public List<OSVVulnerabilities>? Vulns { get; set; }

  public override string ToString() {
    var returnString = "";
    if (this.Vulns == null) {
      return "No known vulnerabilities found in the OSV database.\n";
    }
    foreach (var vuln in this.Vulns) {
      returnString += vuln.ToString();
    }
    return returnString;
  }

  public static async Task<OSVData?> GetOSVData(HttpClient httpClient, string packageName, string packageVersion) {
    try {
      // Fetch package data
      var osvJSONPost =
        $"{{\"version\": \"{packageVersion}\", \"package\": {{\"name\":\"{packageName}\",\"ecosystem\":\"NuGet\"}}}}";

      var content = new StringContent(osvJSONPost, System.Text.Encoding.UTF8, "application/json");
      var response = await httpClient.PostAsync("https://api.osv.dev/v1/query", content);
      if (response.StatusCode == HttpStatusCode.OK) {
        var responseStream = await response.Content.ReadAsStreamAsync();
        var osvData = await JsonSerializer.DeserializeAsync<OSVData>(responseStream);
        return osvData;
      }
      else {
        var errorResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"An error occurred. Status code: {response.StatusCode}. Error message: {errorResponse}");
      }
    }
    catch (HttpRequestException ex) {
      // Handle any exceptions thrown by the HTTP client.
      Console.WriteLine($"An HTTP error occurred: {ex.Message}");
    }
    catch (JsonException ex) {
      // Handle any exceptions thrown during JSON deserialization.
      Console.WriteLine($"A JSON error occurred: {ex.Message}");
    }
    return null;
  }

}

public class OSVVulnerabilities {
  [JsonPropertyName("id")]
  public required string Id { get; set; }
  [JsonPropertyName("summary")]
  public required string Summary { get; set; }
  [JsonPropertyName("details")]
  public required string Details { get; set; }
  [JsonPropertyName("modified")]
  public string Modified { get; set; } = string.Empty;
  [JsonPropertyName("published")]
  public string Published { get; set; } = string.Empty;
  [JsonPropertyName("references")]
  public List<Reference> References { get; set; } = new List<Reference>();
  [JsonPropertyName("affected")]
  public required List<Affected> Affected { get; set; }
  [JsonPropertyName("schema_version")]
  public string SchemaVersion { get; set; } = string.Empty;
  [JsonPropertyName("severity")]
  public List<Severity> Severity { get; set; } = new List<Severity>();

  public override string ToString() {
    var stringReferences = "";
    foreach (var reference in this.References) {
      stringReferences += reference.ToString();
    }
    var affectedReferences = "";
    foreach (var affected in this.Affected) {
      affectedReferences += affected.ToString();
    }
    return
      $"Id: {this.Id}\n" +
      $"Summary: {this.Summary}\n" +
      $"Details: {this.Details}\n" +
      $"Published: {this.Published}\n" +
      $"References: \n{stringReferences}" +
      $"Affected: \n{affectedReferences}";
  }
}

public class Reference {
  [JsonPropertyName("type")]
  public string Type { get; set; } = string.Empty;
  [JsonPropertyName("url")]
  public string Url { get; set; } = string.Empty;

  public override string ToString() => $"- {this.Type}: {this.Url}\n";
}

public class Severity {
  [JsonPropertyName("type")]
  public string Type { get; set; } = string.Empty;
  [JsonPropertyName("score")]
  public string Score { get; set; } = string.Empty;

  public override string ToString() => $"Severity: Type: {this.Type} Score: {this.Score}";
}

public class Affected {
  [JsonPropertyName("package")]
  public required OSVPackage Package { get; set; }
  [JsonPropertyName("ranges")]
  public List<Range> Ranges { get; set; } = new List<Range>();
  [JsonPropertyName("versions")]
  public List<string> Versions { get; set; } = new List<string>();

  public override string ToString() {
    var returnString = "";
    foreach (var range in this.Ranges) {
      returnString += range.ToString();
    }
    return returnString;
  }
}

public class OSVPackage {
  [JsonPropertyName("name")]
  public string Name { get; set; } = string.Empty;
  [JsonPropertyName("ecosystem")]
  public string Ecosystem { get; set; } = string.Empty;
  [JsonPropertyName("purl")]
  public string Purl { get; set; } = string.Empty;

  public override string ToString() => $"Name: {this.Name} Ecosystem: {this.Ecosystem} Purl: {this.Purl}\n";

}

public class OSVSource {
  [JsonPropertyName("source")]
  public string Source { get; set; } = string.Empty;
  public override string ToString() => $"Source: {this.Source}\n";
}

public class Range {
  [JsonPropertyName("type")]
  public string Type { get; set; } = string.Empty;
  [JsonPropertyName("events")]
  public List<VulnerabilityEvent> Events { get; set; } = new List<VulnerabilityEvent>();

  public override string ToString() {
    var returnString = $"- Type: {this.Type}\n";
    foreach (var e in this.Events) {
      returnString += $"- Event: {e}\n";
    }
    return returnString;
  }
}

public class VulnerabilityEvent {
  [JsonPropertyName("introduced")]
  public string Introduced { get; set; } = string.Empty;
  [JsonPropertyName("fixed")]
  public string Fixed { get; set; } = string.Empty;
  [JsonPropertyName("limit")]
  public string Limit { get; set; } = string.Empty;

  public override string ToString() => $"Introduced: {this.Introduced} Fixed: {this.Fixed} Limit: {this.Limit}\n";
}
