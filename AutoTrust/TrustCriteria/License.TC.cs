namespace AutoTrust;
public class License : ITrustCriteria {
  public static string Title => "Package License";
  public static int TotalScoreImportance => 7;

  // List of Spdx licenses: https://spdx.org/licenses/
  // List based on: https://www.synopsys.com/blogs/software-security/top-open-source-licenses/
  public static readonly string[] HighRiskLicenses = new string[] {
    "AGPL-1.0-only",
    "AGPL-1.0-or-later",
    "AGPL-3.0-only",
    "AGPL-3.0-or-later",
    "GPL-1.0-only",
    "GPL-1.0-or-later",
    "GPL-2.0-only",
    "GPL-2.0-or-later",
    "GPL-3.0-only",
    "GPL-3.0-or-later",
    "LGPL-2.0-only",
    "LGPL-2.0-or-later",
    "LGPL-2.1-only",
    "LGPL-2.1-or-later",
    "LGPL-3.0-only",
    "LGPL-3.0-or-later",
    //These ones are deprecated but still used in some packages
    "AGPL-1.0",
    "AGPL-3.0",
    "GPL-1.0",
    "GPL-1.0+",
    "GPL-2.0",
    "GPL-2.0+",
    "GPL-2.0-with-autoconf-exception",
    "GPL-2.0-with-bison-exception",
    "GPL-2.0-with-classpath-exception",
    "GPL-2.0-with-font-exception",
    "GPL-2.0-with-GCC-exception",
    "GPL-3.0",
    "GPL-3.0+",
    "GPL-3.0-with-autoconf-exception",
    "GPL-3.0-with-GCC-exception",
    "LGPL-2.0",
    "LGPL-2.0+",
    "LGPL-2.1",
    "LGPL-2.1+",
    "LGPL-3.0",
    "LGPL-3.0+",
    "RPSL-1.0",
    "CECILL-1.0",
    "CECILL-1.1",
    "CECILL-2.0",
    "CECILL-B",
    "CECILL-C",
    "EUPL-1.0",
    "EUPL-1.1",
    "EUPL-1.2"
  };

  public static readonly string[] MediumRiskLicenses = new string[] {
    "CDDL-1.0",
    "CDDL-1.1",
    "EPL-1.0",
    "EPL-2.0",
    "MPL-1.0",
    "MPL-1.1",
    "MPL-2.0",
    "MPL-2.0-no-copyleft-exception",
    "MS-LPL",
    "MS-PL",
    "MS-RL",
    "CPAL-1.0",
    "eCos-2.0"
  };

  public static readonly string[] LowRiskLicenses = new string[]{
    "Apache-1.0",
    "Apache-1.1",
    "Apache-2.0",
    "MIT",
    "ISC",
    "BSD-2-Clause",
    "BSD-3-Clause",
    "Unlicense",
    "OFL-1.1",
    "Zlib",
    "WTFPL",
    "AFL-1.1",
    "AFL-1.2",
    "AFL-2.0",
    "AFL-2.1",
    "AFL-3.0",
    "Condor-1.1",
    "ECL-1.0",
    "ECL-2.0",
    "EUDatagrid",
    "FTL"
  };

  public static (bool isSpdxLicense, string riskType) IsSpdxLicense(string? license) {
    if (string.IsNullOrEmpty(license)) {
      return (false, "Unknown");
    }
    if (HighRiskLicenses.Contains(license)) {
      return (true, "High");
    }
    else if (MediumRiskLicenses.Contains(license)) {
      return (true, "Medium");
    }
    else if (LowRiskLicenses.Contains(license)) {
      return (true, "Low");
    }
    else {
      return (false, "Unknown");
    }
  }

  public static (string, Status, string[]) Validate(DataHandler dataHandler) {

    var verbosityInfo = new List<string>();

    if (!string.IsNullOrEmpty(dataHandler?.NugetCatalogEntry?.LicenseExpression)) {
      verbosityInfo.Add($"Package has registered a standard license on Nuget");
      (var isSpdxLicense, var riskType) = IsSpdxLicense(dataHandler?.NugetCatalogEntry?.LicenseExpression);
      if (isSpdxLicense) {
        if (riskType == "High") {
          verbosityInfo.Add($"Package uses a high risk license based on https://www.synopsys.com/blogs/software-security/top-open-source-licenses/");
          return ($"Package uses a high risk license ({dataHandler?.NugetCatalogEntry?.LicenseExpression}): {dataHandler?.NugetCatalogEntry?.LicenseUrl}", Status.Fail, verbosityInfo.ToArray());
        }
        else if (riskType == "Medium") {
          verbosityInfo.Add($"Package uses a medium risk license based on https://www.synopsys.com/blogs/software-security/top-open-source-licenses/");
          return ($"Package uses a medium risk license ({dataHandler?.NugetCatalogEntry?.LicenseExpression}): {dataHandler?.NugetCatalogEntry?.LicenseUrl}", Status.Error, verbosityInfo.ToArray());
        }
        else if (riskType == "Low") {
          verbosityInfo.Add($"Package uses a low risk license based on https://www.synopsys.com/blogs/software-security/top-open-source-licenses/");
          return ($"Package uses a low risk license ({dataHandler?.NugetCatalogEntry?.LicenseExpression}): {dataHandler?.NugetCatalogEntry?.LicenseUrl}", Status.Pass, verbosityInfo.ToArray());
        }
      }
      else {
        return ($"Package uses a standard license, but it is not automatically evaluated ({dataHandler?.NugetCatalogEntry?.LicenseExpression}): {dataHandler?.NugetCatalogEntry?.LicenseUrl}", Status.Error, verbosityInfo.ToArray());
      }
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
      (var isSpdxLicense, var riskType) = IsSpdxLicense(dataHandler?.GithubData?.License?.SpdxId);
      if (isSpdxLicense) {
        verbosityInfo.Add($"The license found on Github of type {dataHandler?.GithubData?.License?.SpdxId} is seen as {riskType} risk");
        if (riskType == "High") {
          return ($"Package standard license ({dataHandler?.GithubData?.License?.SpdxId}) only found on Github: {dataHandler?.GithubData?.License?.Url} with {riskType} risk",
          Status.Fail, verbosityInfo.ToArray());
        }
        return ($"Package standard license ({dataHandler?.GithubData?.License?.SpdxId}) only found on Github: {dataHandler?.GithubData?.License?.Url} with {riskType} risk",
        Status.Error, verbosityInfo.ToArray());
      }
      else {
        verbosityInfo.Add($"The license found on Github of type {dataHandler?.GithubData?.License?.SpdxId} is not evaluated regarding risk");
        return ($"Package standard license ({dataHandler?.GithubData?.License?.SpdxId}) only found on Github: {dataHandler?.GithubData?.License?.Url}",
        Status.Error, verbosityInfo.ToArray());
      }

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
