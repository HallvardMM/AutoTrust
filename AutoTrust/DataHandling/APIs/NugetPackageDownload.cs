namespace AutoTrust;

public class NugetPackageDownload {

  //This class is not used in the current version of AutoTrust, 
  //but can be used to download Nuget packages for further analysis in the future
  public static async Task DownloadNugetPackage(HttpClient httpClient, NugetPackage nugetPackage, string packageName, string packageVersion) {
    var responseStream = await httpClient.GetStreamAsync(nugetPackage?.PackageContent);
    using var fileSystem = new FileStream($"./{packageName}.{packageVersion}.nupkg", FileMode.OpenOrCreate);
    await responseStream.CopyToAsync(fileSystem);
  }

}
