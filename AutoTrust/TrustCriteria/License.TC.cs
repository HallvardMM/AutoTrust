namespace AutoTrust;
public class License : ITrustCriteria {
  public static string Title => "Package License";
  
  public static (string, Status) Validate(DataHandler dataHandler) {

    if (!string.IsNullOrEmpty(dataHandler?.NugetCatalogEntry?.LicenseExpression)) {
      // PrettyPrint.SuccessPrint($"Package uses a standard license ({dataHandler?.NugetCatalogEntry?.LicenseExpression}): {dataHandler?.NugetCatalogEntry?.LicenseUrl}");
      return ($"Package uses a standard license ({dataHandler?.NugetCatalogEntry?.LicenseExpression}): {dataHandler?.NugetCatalogEntry?.LicenseUrl}", Status.Pass);
    }

    if (!string.IsNullOrEmpty(dataHandler?.NugetCatalogEntry?.LicenseUrl)) {
      // PrettyPrint.WarningPrint($"Package uses a custom license: {dataHandler?.NugetCatalogEntry?.LicenseUrl}");
      return ($"Package uses a custom license: {dataHandler?.NugetCatalogEntry?.LicenseUrl}", Status.Error);
    }

    if (!string.IsNullOrEmpty(dataHandler?.PackageManifest?.Metadata?.LicenseUrl)) {
      // PrettyPrint.WarningPrint($"Package uses a custom license: {dataHandler?.PackageManifest?.Metadata?.LicenseUrl}");
      return ($"Package uses a custom license: {dataHandler?.PackageManifest?.Metadata?.LicenseUrl}", Status.Error);
    }


    if (!string.IsNullOrEmpty(dataHandler?.PackageManifest?.Metadata?.License?.Value)) {
      // PrettyPrint.WarningPrint($"Package might use a custom license ({dataHandler?.PackageManifest?.Metadata?.License?.Value}): {dataHandler?.PackageManifest?.Metadata?.LicenseUrl}");
      return ($"Package might use a custom license ({dataHandler?.PackageManifest?.Metadata?.License?.Value}): {dataHandler?.PackageManifest?.Metadata?.LicenseUrl}", Status.Error);
    }

    if (!string.IsNullOrEmpty(dataHandler?.GithubData?.License?.SpdxId)) {
      // PrettyPrint.WarningPrint($"Package standard license ({dataHandler?.GithubData?.License?.SpdxId}) only found on Github: {dataHandler?.GithubData?.License?.Url}");
      return ($"Package standard license ({dataHandler?.GithubData?.License?.SpdxId}) only found on Github: {dataHandler?.GithubData?.License?.Url}", Status.Error);
    }

    if (!string.IsNullOrEmpty(dataHandler?.GithubData?.License?.Name)) {
      // PrettyPrint.WarningPrint($"Package non-standard license ({dataHandler?.GithubData?.License?.Name}) only found on Github: {dataHandler?.GithubData?.License?.Url}");
      return ($"Package non-standard license ({dataHandler?.GithubData?.License?.Name}) only found on Github: {dataHandler?.GithubData?.License?.Url}", Status.Error);
    }

    if (!string.IsNullOrEmpty(dataHandler?.NugetDownloadCount?.Data[0].LicenseUrl)) {
      // PrettyPrint.WarningPrint($"An associated license might have been found: {dataHandler?.NugetDownloadCount?.Data[0].LicenseUrl}");
      return ($"An associated license might have been found: {dataHandler?.NugetDownloadCount?.Data[0].LicenseUrl}", Status.Error);
    }

    // Check if package contains a LICENSE
    for (var i = 0; i < dataHandler?.NugetCatalogEntry?.PackageEntries?.Count; i++) {
      if (dataHandler.NugetCatalogEntry?.PackageEntries?[i].Name is "LICENSE" or "LICENSE.md") {
        // PrettyPrint.FailPrint("Not reported license found in package!");
        return ("Not reported license found in package!", Status.Fail);
      }
    };


    // PrettyPrint.FailPrint("No license found!");
    return ("No license found!", Status.Fail);
  }
}
