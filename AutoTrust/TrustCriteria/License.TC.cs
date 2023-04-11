namespace AutoTrust;
public class License : ITrustCriteria {
  public static string Title => "Package License";

  public static (string, Status, string[]) Validate(DataHandler dataHandler) {

    var verbosityInfo = new List<string>();

    if (!string.IsNullOrEmpty(dataHandler?.NugetCatalogEntry?.LicenseExpression)) {
      verbosityInfo.Add($"Package has registered a standard license on Nuget");
      return ($"Package uses a standard license ({dataHandler?.NugetCatalogEntry?.LicenseExpression}): {dataHandler?.NugetCatalogEntry?.LicenseUrl}", Status.Pass, verbosityInfo.ToArray());
    }
    else {
      verbosityInfo.Add($"Package has not registered a standard license on Nuget");
    }

    if (!string.IsNullOrEmpty(dataHandler?.NugetCatalogEntry?.LicenseUrl)) {
      verbosityInfo.Add($"Package has registered a license on Nuget but it is not a standard license");
      return ($"Package uses a custom license: {dataHandler?.NugetCatalogEntry?.LicenseUrl}", Status.Error, verbosityInfo.ToArray());
    }
    else {
      verbosityInfo.Add($"Package has not registered a license on Nuget");
    }

    if (!string.IsNullOrEmpty(dataHandler?.PackageManifest?.Metadata?.LicenseUrl)) {
      verbosityInfo.Add($"Package has registered a license in the package manifest but cannot tell if it is a standard license");
      return ($"Package uses a custom license: {dataHandler?.PackageManifest?.Metadata?.LicenseUrl}", Status.Error, verbosityInfo.ToArray());
    }

    if (!string.IsNullOrEmpty(dataHandler?.PackageManifest?.Metadata?.License?.Value)) {
      verbosityInfo.Add($"Package has registered a license in the package manifest");
      return ($"Package might use a custom license ({dataHandler?.PackageManifest?.Metadata?.License?.Value}): {dataHandler?.PackageManifest?.Metadata?.LicenseUrl}",
      Status.Error, verbosityInfo.ToArray());
    }
    verbosityInfo.Add($"Package has not registered a license in the package manifest");

    if (!string.IsNullOrEmpty(dataHandler?.GithubData?.License?.SpdxId)) {
      verbosityInfo.Add($"Package has registered a standard license using a spdx on Github");
      return ($"Package standard license ({dataHandler?.GithubData?.License?.SpdxId}) only found on Github: {dataHandler?.GithubData?.License?.Url}",
      Status.Error, verbosityInfo.ToArray());
    }
    else {
      verbosityInfo.Add($"Package has not registered a standard license using a spdx on Github");
    }

    if (!string.IsNullOrEmpty(dataHandler?.GithubData?.License?.Name)) {
      verbosityInfo.Add($"Package has registered a non-standard license on Github");
      return ($"Package non-standard license ({dataHandler?.GithubData?.License?.Name}) only found on Github: {dataHandler?.GithubData?.License?.Url}", Status.Error, verbosityInfo.ToArray());
    }
    else {
      verbosityInfo.Add($"Package has not registered a non-standard license on Github");
    }

    if (!string.IsNullOrEmpty(dataHandler?.NugetDownloadCount?.Data[0].LicenseUrl)) {
      var downloadCountUrl = $"https://azuresearch-usnc.nuget.org/query?q=packageid:{dataHandler.PackageName}";
      verbosityInfo.Add($"Package has registered a license on Nuget for: {downloadCountUrl}");
      return ($"An associated license might have been found: {dataHandler?.NugetDownloadCount?.Data[0].LicenseUrl}",
       Status.Error, verbosityInfo.ToArray());
    }
    else {
      verbosityInfo.Add($"Package has not registered a license on Nuget");
    }

    // Check if package contains a LICENSE
    for (var i = 0; i < dataHandler?.NugetCatalogEntry?.PackageEntries?.Count; i++) {
      if (dataHandler.NugetCatalogEntry?.PackageEntries?[i].Name is "LICENSE" or "LICENSE.md") {
        verbosityInfo.Add($"Package contains a LICENSE file, but no license has been registered on Nuget");
        return ("Not reported license found in package!", Status.Fail, verbosityInfo.ToArray());
      }
    };

    verbosityInfo.Add($"None of the files in the package was named 'LICENSE' or 'LICENSE.md'");

    return ("No license found!", Status.Fail, verbosityInfo.ToArray());
  }
}
