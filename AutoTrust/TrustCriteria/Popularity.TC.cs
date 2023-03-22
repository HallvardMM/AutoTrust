namespace AutoTrust;

using System.Globalization;

public class Popularity : ITrustCriteria {
  public string Title => "Package Popularity";

  public static Status Validate(DataHandler dataHandler) {
    // Check download count
    if (dataHandler.NugetDownloadCount == null) {
      PrettyPrint.FailPrint("Can't find download count for package");
      return Status.Fail;
    }
    else if (dataHandler.NugetDownloadCount.Data[0].TotalDownloads <= TCConfiguration.DownloadsThreshold) {
      PrettyPrint.FailPrint("Package download count: " + dataHandler.NugetDownloadCount.Data[0].TotalDownloads + " is lower than threshold: " + TCConfiguration.DownloadsThreshold);
      return Status.Fail;
    }

    // Check number of stars, forks and watchers on github
    if (dataHandler.GithubData == null) {
      PrettyPrint.FailPrint("Can't find github data for package");
      return Status.Fail;
    }
    else if (dataHandler.GithubData.StargazersCount <= TCConfiguration.StargazersCountThreshold) {
      PrettyPrint.FailPrint("Package github stargazers count: " + dataHandler.GithubData.StargazersCount + " is lower than threshold: " + TCConfiguration.StargazersCountThreshold);
      return Status.Fail;
    }
    else if (dataHandler.GithubData.ForksCount <= TCConfiguration.ForksCountThreshold) {
      PrettyPrint.FailPrint("Package github forks count: " + dataHandler.GithubData.ForksCount + " is lower than threshold: " + TCConfiguration.ForksCountThreshold);
      return Status.Fail;
    }
    else if (dataHandler.GithubData.WatchersCount <= TCConfiguration.WatchersThreshold) {
      PrettyPrint.FailPrint("Package github watchers count: " + dataHandler.GithubData.WatchersCount + " is lower than threshold: " + TCConfiguration.WatchersThreshold);
      return Status.Fail;
    }

    if (dataHandler.UsedByInformation == "") {
      PrettyPrint.FailPrint("Cannot find information about dependents of package");
      return Status.Fail;
    }

    var usedByCriteria = ValidateUsedByCriteria(dataHandler);
    if (usedByCriteria != Status.Pass) {
      return usedByCriteria;
    }

    // Check number of projects that depend on this package

    PrettyPrint.SuccessPrint("Package popularity criteria passed");
    return Status.Pass;
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

  public static Status ValidateUsedByCriteria(DataHandler dataHandler) {
    var nuGetPackagesString = "<strong>NuGet packages </strong> (";

    var indexOfNuGetPackages = dataHandler.UsedByInformation.IndexOf(nuGetPackagesString, StringComparison.Ordinal);

    var githubRepositoriesString = "<strong>GitHub repositories </strong> (";

    var indexOfGithubRepositories = dataHandler.UsedByInformation.IndexOf(githubRepositoriesString, StringComparison.Ordinal);

    if (indexOfNuGetPackages == -1) {
      PrettyPrint.WarningPrint("Package is not used by any Nuget packages");
      if (indexOfGithubRepositories == -1) {
        PrettyPrint.WarningPrint("Package is not part of any Github projects");
      }
      return Status.Error;
    }

    if (indexOfGithubRepositories == -1) {
      PrettyPrint.WarningPrint("Package is not part of any Github projects");
      return Status.Error;
    }

    var endIndexNuGetPackages = dataHandler.UsedByInformation.IndexOf(")", indexOfNuGetPackages, StringComparison.Ordinal);

    var endIndexGithubRepositories = dataHandler.UsedByInformation.IndexOf(")", indexOfGithubRepositories, StringComparison.Ordinal);

    var nuGetUsed = dataHandler.UsedByInformation[(indexOfNuGetPackages + nuGetPackagesString.Length)..endIndexNuGetPackages].Trim();
    var githubUsed = dataHandler.UsedByInformation[(indexOfGithubRepositories + githubRepositoriesString.Length)..endIndexGithubRepositories].Trim();

    if (ConvertStringWithSIPrefixToNumber(nuGetUsed) < TCConfiguration.UsedByNugetPackagesThreshold) {
      PrettyPrint.WarningPrint($"Package is used by less than {TCConfiguration.UsedByNugetPackagesThreshold} Nuget packages: {nuGetUsed}");
      if (ConvertStringWithSIPrefixToNumber(githubUsed) < TCConfiguration.UsedByGithubRepositoriesThreshold) {
        PrettyPrint.WarningPrint($"Package is used by less than {TCConfiguration.UsedByGithubRepositoriesThreshold} Github repositories: {githubUsed}");
      }
      return Status.Error;
    }
    if (ConvertStringWithSIPrefixToNumber(githubUsed) < TCConfiguration.UsedByGithubRepositoriesThreshold) {
      PrettyPrint.WarningPrint($"Package is used by less than {TCConfiguration.UsedByGithubRepositoriesThreshold} Github repositories: {githubUsed}");
      return Status.Error;
    }

    return Status.Pass;
  }

  public static long ConvertStringWithSIPrefixToNumber(string numberWithPrefix) {
    // Function for converting a string with SI prefix to a number
    // Example: 1.2k -> 1200
    var number = 0.0;
    var prefix = numberWithPrefix.Last();
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

