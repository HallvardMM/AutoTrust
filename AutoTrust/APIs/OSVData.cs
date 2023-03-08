using System.Text.Json.Serialization;

// API docs: https://osv.dev/docs/#tag/api/operation/OSV_QueryAffectedBatch
// Info about properties details and format: https://ossf.github.io/osv-schema/

namespace AutoTrust
{
  public class OSVData
  {
    [JsonPropertyName("vulns")]
    public List<OSVVulnerabilities>? Vulns { get; set; }
 
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
  }

  public class OSVVulnerabilities
  {
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
    public List<Severity> Severity { get; set; }= new List<Severity>();

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
        $"Refrences: \n{stringReferences}" +
        $"Affected: \n{affectedReferences}";
    }
  }

  public class Reference
  {
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    public override string ToString()
    {
      return $"- {Type}: {Url}\n";
    }
  }

  public class Severity
  {
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
    [JsonPropertyName("score")]
    public string Score { get; set; } = string.Empty;

    public override string ToString()
    {
      return $"Severity: Type: {Type} Score: {Score}";
    }
  }

  public class Affected
  {
    [JsonPropertyName("package")]
    public required OSVPackage Package { get; set; }
    [JsonPropertyName("ranges")]
    public List<Range> Ranges { get; set; } = new List<Range>();
    [JsonPropertyName("versions")]
    public List<string> Versions { get; set; } = new List<string>();

    public override string ToString()
    {
      string returnString = "";
      foreach (Range range in Ranges)
      {
        returnString += range.ToString();
      }
      return returnString;
    }
  }

  public class OSVPackage
  {
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("ecosystem")]
    public string Ecosystem { get; set; } = string.Empty;
    [JsonPropertyName("purl")]
    public string Purl { get; set; } = string.Empty;

    public override string ToString()
    {
      return $"Name: {Name} Ecosystem: {Ecosystem} Purl: {Purl}\n";
    }

  }

  public class OSVSource
  {
    [JsonPropertyName("source")]
    public string Source { get; set; } = string.Empty;
    public override string ToString()
    {
      return $"Source: {Source}\n";
    }
  }

  public class Range
  {
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
    [JsonPropertyName("events")]
    public List<Event> Events { get; set; }  = new List<Event>();

    public override string ToString()
    {
      string returnString = $"- Type: {Type}\n";
      foreach (Event e in Events)
      {
        returnString += $"- Event: {e.ToString()}\n";
      }
      return returnString;
    }
  }

  public class Event {
    [JsonPropertyName("introduced")]
    public string Introduced { get; set; } = string.Empty;
    [JsonPropertyName("fixed")]
    public string Fixed { get; set; } = string.Empty;
    [JsonPropertyName("limit")]
    public string Limit { get; set; } = string.Empty;
    
    public override string ToString()
    {
      return $"Introduced: {Introduced} Fixed: {Fixed} Limit: {Limit}\n";
    }
  }
}