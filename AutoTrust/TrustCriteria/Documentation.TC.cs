namespace AutoTrust;

public class Documentation : ITrustCriteria {
  public static string Title => "Package Documentation";
  
  public static (string, Status) Validate(DataHandler dataHandler) {
    // Check if package contains a README
    for (var i = 0; i < dataHandler.NugetCatalogEntry?.PackageEntries?.Count; i++) {
      if (dataHandler.NugetCatalogEntry?.PackageEntries?[i].Name is "README" or "README.md") {
        // PrettyPrint.SuccessPrint("Package contains documentation: README found in package");
        return ("Package contains documentation: README found in package", Status.Pass);
      }
    };

    // Check if Github has a README
    if (!string.IsNullOrEmpty(dataHandler.GithubReadmeData?.HtmlUrl)) {
      // PrettyPrint.SuccessPrint($"Github page contains README: {dataHandler.GithubReadmeData?.HtmlUrl}");
      return ($"Github page contains README: {dataHandler.GithubReadmeData?.HtmlUrl}", Status.Pass);
    }

    // Check if projectURL is set and not Github (might be an homepage)
    if (!string.IsNullOrEmpty(dataHandler?.NugetCatalogEntry?.ProjectUrl) && (!dataHandler?.NugetCatalogEntry?.ProjectUrl.Contains("github") ?? false)) {
      // PrettyPrint.WarningPrint($"Package has webpage that is not Github that might contain documentation: {dataHandler?.NugetCatalogEntry?.ProjectUrl}");
      return ($"Package has webpage that is not Github that might contain documentation: {dataHandler?.NugetCatalogEntry?.ProjectUrl}", Status.Error);
    }

    // Check if Github contains a Github wiki or a webpage
    if (dataHandler?.GithubData != null) {

      if (dataHandler.GithubData?.HasWiki == true) {
        // PrettyPrint.WarningPrint($"Package has Github wiki that might contain documentation: {dataHandler.GithubData?.HtmlUrl}/wiki");
        return ($"Package has Github wiki that might contain documentation: {dataHandler.GithubData?.HtmlUrl}/wiki", Status.Error);
      }

      if (!string.IsNullOrEmpty(dataHandler.GithubData?.Homepage)) {
        // PrettyPrint.WarningPrint($"Package has webpage that might contain documentation: {dataHandler.GithubData.Homepage}");
        return ($"Package has webpage that might contain documentation: {dataHandler.GithubData.Homepage}", Status.Error);
      }
    }

    // PrettyPrint.FailPrint("No documentation found!");
    return ("No documentation found!", Status.Fail);
  }
}
