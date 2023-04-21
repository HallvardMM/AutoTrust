namespace AutoTrust;

public class TerminalSpinner {
  private static readonly string[] _spinner = new string[] { "|", "/", "-", "\\" };
  // private static readonly string[] _spinner = new string[] { "⣾", "⣽", "⣻", "⢿", "⡿", "⣟", "⣯", "⣷" };

  private static int spinnerIndex;
  private static readonly int spinnerDelay = 100;
  private static Timer? _timer;

  public static void Start() => _timer = new Timer(_ => Console.Write($"\r{_spinner[spinnerIndex++ % _spinner.Length]}"), null, TimeSpan.Zero, TimeSpan.FromMilliseconds(spinnerDelay));

  public static void Stop() {
    _timer?.Change(Timeout.Infinite, 0);
    Console.Write("\r");
    // Console.Write("\r ");
  }
}
