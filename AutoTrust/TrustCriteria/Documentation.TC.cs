namespace AutoTrust;

public class Documentation : ITrustCriteria {
  public static string Title => "Package Documentation";
  public static readonly int NugetFileByteSizeMinThreshold = 300;

  public static (string, Status, string[]) Validate(DataHandler dataHandler) {
    var verbosityInfo = new List<string>();

    // Check if package contains a README
    for (var i = 0; i < dataHandler.NugetCatalogEntry?.PackageEntries?.Count; i++) {
      if (dataHandler.NugetCatalogEntry?.PackageEntries?[i].Name is "README" or "README.md") {
        if (dataHandler.NugetCatalogEntry?.PackageEntries?[i].Length > NugetFileByteSizeMinThreshold) {
          verbosityInfo.Add($"Package has README.file or README.md file in package that is larger than {NugetFileByteSizeMinThreshold} bytes");
          return ("Package contains documentation: README found in package", Status.Pass, verbosityInfo.ToArray());
        }
        else {
          verbosityInfo.Add($"Package has README.file or README.md file in package that is smaller than {NugetFileByteSizeMinThreshold} bytes");
          break;
        }
      }
      if (i == dataHandler.NugetCatalogEntry?.PackageEntries?.Count - 1) {
        verbosityInfo.Add("Package does not have a README.file or README.md file in package");
      }
    };

    // Check if Github has a README
    if (!string.IsNullOrEmpty(dataHandler.GithubReadmeData?.HtmlUrl)) {
      if (dataHandler.GithubReadmeData?.Size > NugetFileByteSizeMinThreshold) {
        verbosityInfo.Add($"Package has a README.file or README.md file on Github that is larger than {NugetFileByteSizeMinThreshold} bytes");
        return ($"Package contains documentation: README found on Github: {dataHandler.GithubReadmeData?.HtmlUrl}", Status.Pass, verbosityInfo.ToArray());
      }
      else {
        verbosityInfo.Add($"Package has a README.file or README.md file on Github that is smaller than {NugetFileByteSizeMinThreshold} bytes");
      }
    }
    else {
      verbosityInfo.Add("Package does not have a README.file or README.md file on Github");
    }

    // Check if projectURL is set and not Github (might be a homepage)
    if (!string.IsNullOrEmpty(dataHandler?.NugetCatalogEntry?.ProjectUrl) && (!dataHandler?.NugetCatalogEntry?.ProjectUrl.Contains("github") ?? false)) {
      verbosityInfo.Add("In Nuget Catalog Entry there is registered a webpage that is not Github and might contain documentation");
      return ($"Package has webpage that is not Github that might contain documentation: {dataHandler?.NugetCatalogEntry?.ProjectUrl}",
      Status.Error, verbosityInfo.ToArray());
    }
    verbosityInfo.Add("In Nuget Catalog Entry there is no registered webpage or it is Github");

    // Check if Github contains a Github wiki or a webpage
    if (dataHandler?.GithubData != null) {

      if (dataHandler.GithubData?.HasWiki == true) {
        verbosityInfo.Add("Package has a GitHub wiki that might contain documentation");
        return ($"Package has GitHub wiki that might contain documentation: {dataHandler.GithubData?.HtmlUrl}/wiki", Status.Error, verbosityInfo.ToArray());
      }

      if (!string.IsNullOrEmpty(dataHandler.GithubData?.Homepage)) {
        verbosityInfo.Add("Package has a registered homepage on GitHub that might contain documentation");
        return ($"Package has webpage that might contain documentation: {dataHandler.GithubData.Homepage}", Status.Error, verbosityInfo.ToArray());
      }
    }
    verbosityInfo.Add("Package does not have a GitHub wiki or a registered homepage on GitHub");

    return ("No documentation found!", Status.Fail, verbosityInfo.ToArray());
  }
}
