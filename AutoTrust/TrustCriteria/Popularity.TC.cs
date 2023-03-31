namespace AutoTrust;

using System.Globalization;

public class Popularity : ITrustCriteria {
  public static string Title => "Package Popularity";
  private static readonly long DownloadsThreshold = 10000;
  private static readonly long StargazersCountThreshold = 2;
  private static readonly long ForksCountThreshold = 1;
  private static readonly long WatchersThreshold = 1;
  private static readonly long UsedByNugetPackagesThreshold = 10;
  private static readonly long UsedByGithubRepositoriesThreshold = 10;


  public static (string, Status) Validate(DataHandler dataHandler) {
    // Check download count
    if (dataHandler.NugetDownloadCount == null) {
      return ("Can't find download count for package", Status.Fail);
    }
    else if (dataHandler.NugetDownloadCount.Data[0].TotalDownloads <= DownloadsThreshold) {
      return ("Package download count: " + dataHandler.NugetDownloadCount.Data[0].TotalDownloads + " is lower than threshold: " + DownloadsThreshold, Status.Fail);
    }

    // Check number of stars, forks and watchers on github
    if (dataHandler.GithubData == null) {
      return ("Can't find github data for package", Status.Fail);
    }
    else if (dataHandler.GithubData.StargazersCount <= StargazersCountThreshold) {
      return ("Package github stargazers count: " + dataHandler.GithubData.StargazersCount + " is lower than threshold: " + StargazersCountThreshold, Status.Fail);
    }
    else if (dataHandler.GithubData.ForksCount <= ForksCountThreshold) {
      return ("Package github forks count: " + dataHandler.GithubData.ForksCount + " is lower than threshold: " + ForksCountThreshold, Status.Fail);
    }
    else if (dataHandler.GithubData.WatchersCount <= WatchersThreshold) {
      return ("Package github watchers count: " + dataHandler.GithubData.WatchersCount + " is lower than threshold: " + WatchersThreshold, Status.Fail);
    }

    if (dataHandler.UsedByInformation == "") {
      return ("Cannot find information about dependents of package", Status.Fail);
    }

    var (usedByCriteriaString, usedByCriteriaBool)  = ValidateUsedByCriteria(dataHandler);
    if (usedByCriteriaBool != Status.Pass) {
      return (usedByCriteriaString, usedByCriteriaBool);
    }

    return ("Package popularity criteria passed", Status.Pass);
  }

  public static long? GetPackageVersionDownloadCount(DataHandler dataHandler) {
    // Function for getting the download count for a specific package version
    for (var i = 0; i < dataHandler.NugetDownloadCount?.Data[0].Versions.Count; i++) {
      if (dataHandler.NugetDownloadCount.Data[0].Versions[i]?.Version == dataHandler.PackageVersion) {
        return dataHandler.NugetDownloadCount.Data[0].Versions[i].Downloads;
      }
    }
    return null;
  }

  public static (string, Status) ValidateUsedByCriteria(DataHandler dataHandler) {
    var nuGetPackagesString = "<strong>NuGet packages </strong> (";

    var indexOfNuGetPackages = dataHandler.UsedByInformation.IndexOf(nuGetPackagesString, StringComparison.Ordinal);

    var githubRepositoriesString = "<strong>GitHub repositories </strong> (";

    var indexOfGithubRepositories = dataHandler.UsedByInformation.IndexOf(githubRepositoriesString, StringComparison.Ordinal);

    if (indexOfNuGetPackages == -1 && indexOfGithubRepositories == -1) {
      return ("Package is not used by any Nuget packages and is not part of any Github projects", Status.Error);
    }
    else if (indexOfNuGetPackages == -1) {
      return ("Package is not used by any Nuget packages", Status.Error);
    }
    else if (indexOfGithubRepositories == -1) {
      return ("Package is not part of any Github projects", Status.Error);
    }

    var endIndexNuGetPackages = dataHandler.UsedByInformation.IndexOf(")", indexOfNuGetPackages, StringComparison.Ordinal);

    var endIndexGithubRepositories = dataHandler.UsedByInformation.IndexOf(")", indexOfGithubRepositories, StringComparison.Ordinal);

    var nuGetUsed = dataHandler.UsedByInformation[(indexOfNuGetPackages + nuGetPackagesString.Length)..endIndexNuGetPackages].Trim();
    var githubUsed = dataHandler.UsedByInformation[(indexOfGithubRepositories + githubRepositoriesString.Length)..endIndexGithubRepositories].Trim();

    if (ConvertStringWithSIPrefixToNumber(nuGetUsed) < UsedByNugetPackagesThreshold && ConvertStringWithSIPrefixToNumber(githubUsed) < UsedByGithubRepositoriesThreshold) {
      return ($"Package is used by less than {UsedByNugetPackagesThreshold} Nuget packages: {nuGetUsed} and less than {UsedByGithubRepositoriesThreshold} Github repositories: {githubUsed}", Status.Error);
    }
    else if (ConvertStringWithSIPrefixToNumber(nuGetUsed) < UsedByNugetPackagesThreshold) {
      return ($"Package is used by less than {UsedByNugetPackagesThreshold} Nuget packages: {nuGetUsed}", Status.Error);
    }
    else if (ConvertStringWithSIPrefixToNumber(githubUsed) < UsedByGithubRepositoriesThreshold) {
      return ($"Package is used by less than {UsedByGithubRepositoriesThreshold} Github repositories: {githubUsed}", Status.Error);
    }

    if (ConvertStringWithSIPrefixToNumber(githubUsed) < UsedByGithubRepositoriesThreshold) {
      return ($"Package is used by less than {UsedByGithubRepositoriesThreshold} Github repositories: {githubUsed}", Status.Error);
    }

    return ("", Status.Pass);
  }

  public static long ConvertStringWithSIPrefixToNumber(string numberWithPrefix) {
    // Function for converting a string with SI prefix to a number
    // Example: 1.2k -> 1200
    var number = 0.0;
    var prefix = numberWithPrefix.Last();
    if (!char.IsLetter(prefix)) {
      if (long.TryParse(numberWithPrefix, CultureInfo.InvariantCulture, out var regularNumber)) {
        return regularNumber;
      }
      else {
        return 0; // Return 0 if last character is not a letter and the string is not a number
      }
    }
    var numberString = numberWithPrefix[..^1];
    if (double.TryParse(numberString, CultureInfo.InvariantCulture, out var numberDouble)) {
      number = numberDouble;
    }
    else {
      return 0; // Return 0 if the string is not a number
    }
    switch (prefix) {
      case 'k':
        number *= 1000;
        break;
      case 'K': // Nuget uses K for kilo
        number *= 1000;
        break;
      case 'M':
        number *= 1000000;
        break;
      case 'G':
        number *= 1000000000;
        break;
      case 'T':
        number *= 1000000000000;
        break;
      case 'P':
        number *= 1000000000000000;
        break;
      case 'E':
        number *= 1000000000000000000;
        break;
      default:
        break;
    }
    return (long)number;
  }
}

