namespace AutoTrust;

public class Documentation : ITrustCriteria {
  public static string Title => "Package Documentation";
  public static (string, Status, string[]) Validate(DataHandler dataHandler) {
    var passedCriteria = new List<string>();
    // Check if package contains a README
    for (var i = 0; i < dataHandler.NugetCatalogEntry?.PackageEntries?.Count; i++) {
      if (dataHandler.NugetCatalogEntry?.PackageEntries?[i].Name is "README" or "README.md") {
        passedCriteria.Add("Package has README.file or README.md file in package");
        return ("Package contains documentation: README found in package", Status.Pass, passedCriteria.ToArray());
      }
    };
    passedCriteria.Add("Package does not have a README.file or README.md file in package");

    // Check if Github has a README
    if (!string.IsNullOrEmpty(dataHandler.GithubReadmeData?.HtmlUrl)) {
      passedCriteria.Add("Package has a README.file or README.md file on Github");
      return ($"Github page contains README: {dataHandler.GithubReadmeData?.HtmlUrl}", Status.Pass, passedCriteria.ToArray());
    }
    passedCriteria.Add("Package does not have a README.file or README.md file on Github");

    // Check if projectURL is set and not Github (might be an homepage)
    if (!string.IsNullOrEmpty(dataHandler?.NugetCatalogEntry?.ProjectUrl) && (!dataHandler?.NugetCatalogEntry?.ProjectUrl.Contains("github") ?? false)) {
      passedCriteria.Add("In Nuget Catalog Entry there is registered a webpage that is not Github and might contain documentation");
      return ($"Package has webpage that is not Github that might contain documentation: {dataHandler?.NugetCatalogEntry?.ProjectUrl}",
      Status.Error, passedCriteria.ToArray());
    }
    passedCriteria.Add("In Nuget Catalog Entry there is no registered webpage or it is Github");

    // Check if Github contains a Github wiki or a webpage
    if (dataHandler?.GithubData != null) {

      if (dataHandler.GithubData?.HasWiki == true) {
        passedCriteria.Add("Package has a GitHub wiki that might contain documentation");
        return ($"Package has GitHub wiki that might contain documentation: {dataHandler.GithubData?.HtmlUrl}/wiki", Status.Error, passedCriteria.ToArray());
      }

      if (!string.IsNullOrEmpty(dataHandler.GithubData?.Homepage)) {
        passedCriteria.Add("Package has a registered homepage on GitHub that might contain documentation");
        return ($"Package has webpage that might contain documentation: {dataHandler.GithubData.Homepage}", Status.Error, passedCriteria.ToArray());
      }
    }
    passedCriteria.Add("Package does not have a GitHub wiki or a registered homepage on GitHub");

    return ("No documentation found!", Status.Fail, passedCriteria.ToArray());
  }
}
