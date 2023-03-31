
namespace AutoTrust;

public class PrettyPrint {
  public static void PrintTCMessage(string message, Status status) {
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
