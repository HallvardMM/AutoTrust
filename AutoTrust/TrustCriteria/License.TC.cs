namespace AutoTrust;
public class License : ITrustCriteria {
  public static string Title => "Package License";

  public static (string, Status) Validate(DataHandler dataHandler) {

    if (!string.IsNullOrEmpty(dataHandler?.NugetCatalogEntry?.LicenseExpression)) {
      return ($"Package uses a standard license ({dataHandler?.NugetCatalogEntry?.LicenseExpression}): {dataHandler?.NugetCatalogEntry?.LicenseUrl}", Status.Pass);
    }

    if (!string.IsNullOrEmpty(dataHandler?.NugetCatalogEntry?.LicenseUrl)) {
      return ($"Package uses a custom license: {dataHandler?.NugetCatalogEntry?.LicenseUrl}", Status.Error);
    }

    if (!string.IsNullOrEmpty(dataHandler?.PackageManifest?.Metadata?.LicenseUrl)) {
      return ($"Package uses a custom license: {dataHandler?.PackageManifest?.Metadata?.LicenseUrl}", Status.Error);
    }


    if (!string.IsNullOrEmpty(dataHandler?.PackageManifest?.Metadata?.License?.Value)) {
      return ($"Package might use a custom license ({dataHandler?.PackageManifest?.Metadata?.License?.Value}): {dataHandler?.PackageManifest?.Metadata?.LicenseUrl}", Status.Error);
    }

    if (!string.IsNullOrEmpty(dataHandler?.GithubData?.License?.SpdxId)) {
      return ($"Package standard license ({dataHandler?.GithubData?.License?.SpdxId}) only found on Github: {dataHandler?.GithubData?.License?.Url}", Status.Error);
    }

    if (!string.IsNullOrEmpty(dataHandler?.GithubData?.License?.Name)) {
      return ($"Package non-standard license ({dataHandler?.GithubData?.License?.Name}) only found on Github: {dataHandler?.GithubData?.License?.Url}", Status.Error);
    }

    if (!string.IsNullOrEmpty(dataHandler?.NugetDownloadCount?.Data[0].LicenseUrl)) {
      return ($"An associated license might have been found: {dataHandler?.NugetDownloadCount?.Data[0].LicenseUrl}", Status.Error);
    }

    // Check if package contains a LICENSE
    for (var i = 0; i < dataHandler?.NugetCatalogEntry?.PackageEntries?.Count; i++) {
      if (dataHandler.NugetCatalogEntry?.PackageEntries?[i].Name is "LICENSE" or "LICENSE.md") {
        return ("Not reported license found in package!", Status.Fail);
      }
    };


    return ("No license found!", Status.Fail);
  }
}
