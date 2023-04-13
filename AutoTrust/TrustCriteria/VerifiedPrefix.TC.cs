namespace AutoTrust;

public class VerifiedPrefix : ITrustCriteria {
  public static string Title => "Package has verified prefix";
  public static int TotalScoreImportance => 7;

  public static (string, Status, string[]) Validate(DataHandler dataHandler) {
    // Check if the package has a verified prefix found in the same api as download count
    // https://learn.microsoft.com/nb-no/nuget/nuget-org/id-prefix-reservation#third-party-feed-provider-scenarios

    // List of passed criteria
    var verbosityInfo = new List<string>();

    if (dataHandler?.NugetDownloadCount?.TotalHits != 1) {
      verbosityInfo.Add($"There are multiple packages matching the package name!");
      return ("Could not check if Prefix is not verified on Nuget", Status.Fail, verbosityInfo.ToArray());
    }
    verbosityInfo.Add($"There is one package matching the package name when searching on Nuget");

    if (dataHandler.NugetDownloadCount.Data[0].Verified) {
      verbosityInfo.Add($"Package has verified prefix on Nuget");
      return ("Package prefix is verified on Nuget", Status.Pass, verbosityInfo.ToArray());
    }

    verbosityInfo.Add($"Package does not have a verified prefix on Nuget");

    return ("Package prefix is not verified on Nuget", Status.Error, verbosityInfo.ToArray());
  }
}
