namespace AutoTrust;

public enum Status {
  Pass,
  Error,
  Fail,
}

internal interface ITrustCriteria {

  public static abstract string Title { get; }
  static abstract (string, Status) Validate(DataHandler dataHandler, bool isVerbose);

  public void DisplayInConsole(Status status, string text) {
    switch (status) {
      case Status.Pass:
        PrettyPrint.SuccessPrint(text);
        break;
      case Status.Fail:
        PrettyPrint.FailPrint(text);
        break;
      case Status.Error:
        PrettyPrint.WarningPrint(text);
        break;
      default:
        break;
    }
  }
}
