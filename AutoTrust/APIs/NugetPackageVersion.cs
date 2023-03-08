namespace AutoTrust
{
  public class NugetPackageVersion
  {
    public List<string> Versions { get; set; } = new List<string>();

    public static string GetVersionsUrl(string packageName)
    {
     return ($"https://api.nuget.org/v3-flatcontainer/{packageName.ToLower()}/index.json");
    }

    public static string? GetLatestStableVersion(List<string> versions)
    {
      for (int i = versions.Count - 1; i >= 0; i--)
      {
        if (!versions[i].Contains("-"))
        {
          return versions[i];
        }
      }
      return null; // TODO: Should maybe return an error?
    }

    public override string ToString()
    {
      return $"[{string.Join(", ", Versions)}]";
    }

  }

}