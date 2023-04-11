
namespace AutoTrust;

public class PrettyPrint {
  public static void PrintTCMessage(string message, Status status, string[] additionalInfo, bool isVerbose) {
    switch (status) {
      case Status.Pass:
        SuccessPrint(message);
        break;
      case Status.Fail:
        FailPrint(message);
        break;
      case Status.Error:
        WarningPrint(message);
        break;
      default:
        break;
    }
    PrintAdditionalInfo(additionalInfo, isVerbose);

  }

  public static void PrintAdditionalInfo(string[] additionalInfo, bool isVerbose) {
    if (isVerbose) {
      foreach (var info in additionalInfo) {
        Console.WriteLine($"- {info}");
      }
      Console.WriteLine();
    }
  }

  public static void WarningPrint(string text) {
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"!: {text}");
    Console.ForegroundColor = ConsoleColor.White;
  }

  public static void FailPrint(string text) {
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"X: {text}");
    Console.ForegroundColor = ConsoleColor.White;
  }

  public static void SuccessPrint(string text) {
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"√: {text}");
    Console.ForegroundColor = ConsoleColor.White;
  }
}
