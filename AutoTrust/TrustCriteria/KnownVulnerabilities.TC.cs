namespace AutoTrust;

public class KnownVulnerabilities : ITrustCriteria {
  public string Title => "Known Vulnerabilities";

  public static Status Validate(DataHandler dataHandler) {
    long oldVulnerabilities = 0;
    long currentVulnerabilities = 0;

    if (dataHandler.NugetCatalogEntry?.Vulnerabilities != null) {
      PrettyPrint.FailPrint($"Package has {dataHandler.NugetCatalogEntry?.Vulnerabilities.Count} known vulnerabilities registered in NuGet");
      return Status.Fail;
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
      PrettyPrint.FailPrint($"Package has {currentVulnerabilities} current vulnerabilities registered in OSV");
      return Status.Fail;
    }

    if (oldVulnerabilities > 0) {
      PrettyPrint.WarningPrint($"Package has {oldVulnerabilities} known vulnerabilities registered in OSV but not for the current version");
      return Status.Error;
    }

    PrettyPrint.SuccessPrint("No current or old vulnerabilities found");
    return Status.Pass;
  }
}
