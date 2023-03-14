
namespace AutoTrust;

public class PrettyPrint {

  /* Examples:   
 * PrettyPrint.SuccessPrint("This is success!");
 * PrettyPrint.WarningPrint("This is a warning!");
 * PrettyPrint.ErrorPrint("This is an error!");
 */

  public static void WarningPrint(string text) {
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"!: {text}");
    Console.ForegroundColor = ConsoleColor.White;
  }

  public static void ErrorPrint(string text) {
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
