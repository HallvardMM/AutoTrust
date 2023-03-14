
namespace AutoTrust;

public class PrettyPrint {
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
