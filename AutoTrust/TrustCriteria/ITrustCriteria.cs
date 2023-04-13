namespace AutoTrust;

public enum Status {
  Pass,
  Error,
  Fail,
}

internal interface ITrustCriteria {

  public static abstract string Title { get; }
  public static abstract int TotalScoreImportance { get; } // Should be between 1 and 10
  static abstract (string, Status, string[]) Validate(DataHandler dataHandler);

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
