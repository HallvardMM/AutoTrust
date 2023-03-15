namespace AutoTrust;

public class KnownVulnerabilities : ITrustCriteria {
  public string Title => "Known Vulnerabilities";

  public static Status Validate(DataHandler dataHandler) {
    // Check if the package has any known vulnerabilities

    // if (dataHandler.NugetCatalogEntry.Vulnerabilities != null) {
    //   Console.WriteLine("NuGet has found known vulnerabilities");
    //   Console.WriteLine("NUGET: " + dataHandler.NugetCatalogEntry.Vulnerabilities[0]);
    // }
    // if (dataHandler.OsvData.Vulns != null) {
    //   Console.WriteLine("Osv has found known vulnerabilities");
    //   Console.WriteLine("OSV: " + dataHandler.OsvData.Vulns[0]);
    // }

    if (dataHandler.NugetCatalogEntry?.Vulnerabilities != null || dataHandler.OsvData?.Vulns != null) {
      PrettyPrint.FailPrint("Package has known vulnerabilities");
      return Status.Error;
    }

    PrettyPrint.SuccessPrint("No known vulnerabilities found");
    return Status.Pass;
  }
}
