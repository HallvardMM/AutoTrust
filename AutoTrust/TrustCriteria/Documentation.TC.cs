namespace AutoTrust;

public class Documentation : ITrustCriteria {
  public string Title => "Package Documentation";
  public static Status Validate(DataHandler dataHandler) {

    // Check if package contains a README
    for (var i = 0; i < dataHandler.NugetCatalogEntry?.PackageEntries?.Count; i++) {
      if (dataHandler.NugetCatalogEntry?.PackageEntries?[i].Name is "README" or "README.md") {
        PrettyPrint.SuccessPrint("Package contains documentation: README found in package");
        return Status.Pass;
      }
    };

    // Check if Github has a README
    if (!string.IsNullOrEmpty(dataHandler.GithubReadmeData?.HtmlUrl)) {
      PrettyPrint.SuccessPrint($"Github page contains README: {dataHandler.GithubReadmeData?.HtmlUrl}");
      return Status.Pass;
    }

    // Check if projectURL is set and not Github (might be an homepage)
    if (!string.IsNullOrEmpty(dataHandler?.NugetCatalogEntry?.ProjectUrl) && (!dataHandler?.NugetCatalogEntry?.ProjectUrl.Contains("github") ?? false)) {
      PrettyPrint.WarningPrint($"Package has webpage that is not Github that might contain documentation: {dataHandler?.NugetCatalogEntry?.ProjectUrl}");
      return Status.Error;
    }

    // Check if Github contains a Github wiki or a webpage
    if (dataHandler?.GithubData != null) {

      if (dataHandler.GithubData?.HasWiki == true) {
        PrettyPrint.WarningPrint($"Package has Github wiki that might contain documentation: {dataHandler.GithubData?.HtmlUrl}/wiki");
        return Status.Error;
      }

      if (!string.IsNullOrEmpty(dataHandler.GithubData?.Homepage)) {
        PrettyPrint.WarningPrint($"Package has webpage that might contain documentation: {dataHandler.GithubData.Homepage}");
        return Status.Error;
      }
    }

    PrettyPrint.FailPrint("No documentation found!");
    return Status.Fail;
  }
}
