namespace AutoTrust;

public enum Status {
    Pass,
    Fail,
    Error,
  }
interface ITrustCriteria
{
    
    public abstract string Title { get; }
    abstract static Status validate(DataHandler dataHandler);

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
    }
  } 
}