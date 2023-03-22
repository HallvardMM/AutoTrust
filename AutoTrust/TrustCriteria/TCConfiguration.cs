namespace AutoTrust;

public class TCConfiguration {
  // Thresholds for age trust criteria
  public static readonly int VersionAgeInDaysThreshold = 22; //Same as npq
  public static readonly int VersionOldAgeInDaysThreshold = 365;
  
  // Thresholds for deprecation trust criteria
    // Currently none are set
  
  // Thresholds for knownVulnerabilities trust criteria
    // Currently none are set
  
  // Thresholds for popularity trust criteria
  public static readonly long DownloadsThreshold = 10000;
  public static readonly long StargazersCountThreshold = 2;
  public static readonly long ForksCountThreshold = 1;
  public static readonly long WatchersThreshold = 1;
  public static readonly long UsedByNugetPackagesThreshold = 10;
  public static readonly long UsedByGithubRepositoriesThreshold = 10;

}