namespace AutoTrust;

public class KnownVulnerabilities : ITrustCriteria {
  public string Title => "Known Vulnerabilities";

  public static Status Validate(DataHandler dataHandler) {
    // Check if the package has any known vulnerabilities

    if (dataHandler.NugetCatalogEntry?.Vulnerabilities != null || dataHandler.OsvData?.Vulns != null) {
      PrettyPrint.FailPrint("Package has known vulnerabilities");
      return Status.Fail;
    }

    PrettyPrint.SuccessPrint("No known vulnerabilities found");
    return Status.Pass;
  }
}
