namespace AutoTrust;

public enum Status {
  Pass,
  Fail,
  Error,
}

internal interface ITrustCriteria {

  public abstract string Title { get; }
  static abstract Status validate(DataHandler dataHandler);

  public void displayInConsole(Status status, string text) {
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
