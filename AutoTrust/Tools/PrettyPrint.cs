
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

  public static void StarPrint(int numberOfStars) {
    var maxStars = 5;
    if (numberOfStars > maxStars || numberOfStars < 0) {
      throw new ArgumentOutOfRangeException(nameof(numberOfStars), "Number of stars must be between 0 and 5");
    }
    var greyStars = maxStars - numberOfStars;
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.Write($"{string.Concat(Enumerable.Repeat("* ", numberOfStars))}");
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.Write($"{string.Concat(Enumerable.Repeat("* ", greyStars))}");
    Console.WriteLine($"({numberOfStars}/{maxStars})");
    Console.ForegroundColor = ConsoleColor.White;
  }

  public static void SecurityScorePrint(int numberOfStars) {
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.WriteLine($"---------------\nSecurity Score:");
    StarPrint(numberOfStars);
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.WriteLine("---------------");
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
