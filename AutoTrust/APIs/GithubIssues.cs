using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace AutoTrust
{
  public class GithubIssues
  {
    [JsonPropertyName("total_count")]
    public long TotalCount { get; set; }
    //IncompleteResults is probably not that valuable:
    //https://docs.github.com/en/rest/search?apiVersion=2022-11-28#timeouts-and-incomplete-results
    [JsonPropertyName("incomplete_results")]
    public bool IncompleteResults { get; set; } 

    public override string ToString()
    {
      string returnString = $"Open issues: {TotalCount}\n";
      if (IncompleteResults)
      {
        returnString += "Warning: Was not able to fetch all open issues from Github!\n";
      }
      return returnString;
    }
  }
}
