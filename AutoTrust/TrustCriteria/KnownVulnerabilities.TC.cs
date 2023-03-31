namespace AutoTrust;

public class KnownVulnerabilities : ITrustCriteria {
  public static string Title => "Known Vulnerabilities";

  public static (string, Status) Validate(DataHandler dataHandler) {
    long oldVulnerabilities = 0;
    long currentVulnerabilities = 0;

    if (dataHandler.NugetCatalogEntry?.Vulnerabilities != null) {
      return ($"Package has {dataHandler.NugetCatalogEntry?.Vulnerabilities.Count} known vulnerabilities registered in NuGet: https://www.nuget.org/packages/{dataHandler.PackageName.ToLower(System.Globalization.CultureInfo.InvariantCulture)}/{dataHandler.PackageVersion.ToLower(System.Globalization.CultureInfo.InvariantCulture)}", Status.Fail);
    }

    //Check all Osv vulnerabilities
    dataHandler.OsvData?.Vulns?.ForEach(vulnerability => {
      vulnerability.Affected.ForEach(affectedPackage => {
        if (affectedPackage.Versions.Contains(dataHandler.PackageVersion)) {
          currentVulnerabilities++;
        }
      });
      oldVulnerabilities++;
    });

    if (currentVulnerabilities > 0) {
      return ($"Package has {currentVulnerabilities} current vulnerabilities registered in OSV: https://osv.dev/list?ecosystem=NuGet&q={dataHandler.PackageName}", Status.Fail);
    }

    if (oldVulnerabilities > 0) {
      return ($"Package has {oldVulnerabilities} known vulnerabilities registered in OSV but not for the current version: https://osv.dev/list?ecosystem=NuGet&q={dataHandler.PackageName}", Status.Error);
    }

    return ("No current or old vulnerabilities found", Status.Pass);
  }
}
