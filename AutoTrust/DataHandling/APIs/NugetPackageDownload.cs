namespace AutoTrust;

public class NugetPackageDownload {

  //TODO: Downloaded packages should go in seperate folder and be deleted after tests
  public static async Task DownloadNugetPackage(HttpClient httpClient, NugetPackage nugetPackage, string packageName, string packageVersion) {
    var responseStream = await httpClient.GetStreamAsync(nugetPackage?.PackageContent);
    using var fileSystem = new FileStream($"./{packageName}.{packageVersion}.nupkg", FileMode.OpenOrCreate);
    await responseStream.CopyToAsync(fileSystem);
  }

}
