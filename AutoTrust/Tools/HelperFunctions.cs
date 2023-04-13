namespace AutoTrust;

public class HelperFunctions {
  public static void AddSecurityScoreOfTC(int totalScoreImportance, Status status, ref double totalTCSecurityScore, ref int totalPossibleTCSecurityScore) {
    double scorePercentage = 0;
    switch (status) {
      case Status.Pass:
        scorePercentage = 1;
        break;
      case Status.Fail:
        scorePercentage = 0;
        break;
      case Status.Error:
        scorePercentage = 0.5;
        break;
      default:
        break;
    }
    totalTCSecurityScore += totalScoreImportance * scorePercentage;
    totalPossibleTCSecurityScore += totalScoreImportance;
  }
}