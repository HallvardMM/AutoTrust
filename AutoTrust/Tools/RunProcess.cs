using System.Diagnostics;

namespace AutoTrust
{
  public class RunProcess
  {
    public static void ProcessExecution(string argumentsString)
    {
      try
      {
        using (Process dotnetProcess = new Process())
        {
          dotnetProcess.StartInfo.UseShellExecute = false;
          dotnetProcess.StartInfo.CreateNoWindow = true;
          dotnetProcess.StartInfo.RedirectStandardInput = true;
          dotnetProcess.StartInfo.RedirectStandardOutput = true;
          dotnetProcess.StartInfo.FileName = "dotnet";
          dotnetProcess.StartInfo.Arguments = argumentsString;
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
  }
}