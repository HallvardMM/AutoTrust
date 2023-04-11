namespace AutoTrust;

public class KnownVulnerabilities : ITrustCriteria {
  public static string Title => "Known Vulnerabilities";

  public static (string, Status, string[]) Validate(DataHandler dataHandler) {
    var passedCriteria = new List<string>();
    long oldVulnerabilities = 0;
    long currentVulnerabilities = 0;

    if (dataHandler.NugetCatalogEntry?.Vulnerabilities != null) {
      var nugetVulnerabilityUrl = $"https://www.nuget.org/packages/{dataHandler.PackageName.ToLowerInvariant()}/{dataHandler.PackageVersion.ToLowerInvariant()}";
      var vulnerabilitiesMessage = $"Package has {dataHandler.NugetCatalogEntry?.Vulnerabilities.Count} known vulnerabilities registered in NuGet: {nugetVulnerabilityUrl}";
      passedCriteria.Add($"Nuget Catalog Entry has registered {dataHandler.NugetCatalogEntry?.Vulnerabilities.Count} vulnerabilities");
      return (vulnerabilitiesMessage, Status.Fail, passedCriteria.ToArray());
    }

    passedCriteria.Add("Nuget Catalog Entry has not registered any vulnerabilities");

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
      passedCriteria.Add($"OSV has registered {currentVulnerabilities} vulnerabilities for the current version");
      return ($"Package has {currentVulnerabilities} current vulnerabilities registered in OSV: https://osv.dev/list?ecosystem=NuGet&q={dataHandler.PackageName}",
      Status.Fail, passedCriteria.ToArray());
    }
    else {
      passedCriteria.Add($"OSV has not registered any vulnerabilities for the current version");
    }

    if (oldVulnerabilities > 0) {
      passedCriteria.Add($"OSV has registered {oldVulnerabilities} vulnerabilities for older versions of the package");
      return ($"Package has {oldVulnerabilities} known vulnerabilities registered in OSV but not for the current version: https://osv.dev/list?ecosystem=NuGet&q={dataHandler.PackageName}",
      Status.Error, passedCriteria.ToArray());
    }
    else {
      passedCriteria.Add($"OSV has not registered any vulnerabilities for older versions of the package");
    }

    return ("No current or old vulnerabilities found", Status.Pass, passedCriteria.ToArray());
  }
}
