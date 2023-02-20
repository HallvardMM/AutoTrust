using AutoTrust;
using System;
using System.Net.Http.Json;
using System.Diagnostics;


Console.WriteLine("Fetching relevant data..."); //TODO remove this

var httpClient = new HttpClient
{
    BaseAddress = new Uri("https://api.chucknorris.io/")
};


// Heads up: add and update are used similarly in dotnet
// dotnet add package <PACKAGE_NAME> 
// dotnet add package <PACKAGE_NAME> -v <VERSION> 


// Write code that fetches the package name and version

//var args = Environment.GetCommandLineArgs();




var query = args.AsQueryable();

if (query.ElementAtOrDefault(0) == "add" & query.ElementAtOrDefault(1) == "package")
{
    // Fetch metadata about the package from the NuGet API, GitHub API, and security databases
    try
    {
        var packageName = query.ElementAt(2);
        string packageVersion;

        //TODO: This leads to out of bounds error if no version is specified
        if (query.ElementAtOrDefault(3) == "-v" || query.ElementAtOrDefault(3) == "--version")
        {
            packageVersion = query.ElementAt(4);
        }
        else
        {
            packageVersion = "latest";
        }
        

        
        using (Process dotnetProcess = new Process())
        {
            // Run the command using dotnet.exe
            // dotnet add package <PACKAGE_NAME> -v <VERSION>
            dotnetProcess.StartInfo.UseShellExecute = false;
            dotnetProcess.StartInfo.CreateNoWindow = true;
            dotnetProcess.StartInfo.RedirectStandardInput = true;
            dotnetProcess.StartInfo.RedirectStandardOutput = true;
            dotnetProcess.StartInfo.FileName = "dotnet.exe";
            
            if (packageVersion == "latest")
            {
                dotnetProcess.StartInfo.Arguments = "add package " + packageName;
                Console.WriteLine("This is ran: " + dotnetProcess.StartInfo.FileName + " " + dotnetProcess.StartInfo.Arguments);
            }
            else
            {
                dotnetProcess.StartInfo.Arguments = "add package " + packageName + " -v " + packageVersion;
                Console.WriteLine("This is ran: " + dotnetProcess.StartInfo.FileName +" "+ dotnetProcess.StartInfo.Arguments);

            }
            dotnetProcess.Start();
            dotnetProcess.StandardInput.Flush();
            dotnetProcess.StandardInput.Close();
            dotnetProcess.WaitForExit();
            Console.WriteLine(dotnetProcess.StandardOutput.ReadToEnd());

        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}
else
{
    try
    {
        using (Process dotnetProcess = new Process())
        {
            dotnetProcess.StartInfo.UseShellExecute = false;
            dotnetProcess.StartInfo.CreateNoWindow = true;
            dotnetProcess.StartInfo.RedirectStandardInput = true;
            dotnetProcess.StartInfo.RedirectStandardOutput = true;
            dotnetProcess.StartInfo.FileName = "dotnet.exe";
            dotnetProcess.StartInfo.Arguments = string.Join(" ", query.ToArray());
            Console.WriteLine("This is ran: " + dotnetProcess.StartInfo.FileName +" "+ dotnetProcess.StartInfo.Arguments);
            dotnetProcess.Start();
            dotnetProcess.StandardInput.Flush();
            dotnetProcess.StandardInput.Close();
            dotnetProcess.WaitForExit();
            Console.WriteLine(dotnetProcess.StandardOutput.ReadToEnd());
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}