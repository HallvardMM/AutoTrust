namespace AutoTrust;

public class HelperFunctions {
  public static void AddSecurityScoreOfTC(int totalScoreImportance, Status status, ref double totalTCSecurityScore, ref int totalPossibleTCSecurityScore) {
    double scorePercentage = 0;
    switch (status) {
      case Status.Pass:
        scorePercentage = 1;
        break;
      case Status.Error:
        scorePercentage = 0.5;
        break;
      case Status.Fail:
        scorePercentage = 0;
        break;
      default:
        break;
    }
    totalTCSecurityScore += totalScoreImportance * scorePercentage;
    totalPossibleTCSecurityScore += totalScoreImportance;
  }

  public static int CalculateNumberOfStars(double score, int possibleScore) {
    var total_score_percentage = score / possibleScore;
    switch (total_score_percentage) {
      case var n when (n >= 0.9):
        return 5;
      case var n when (n >= 0.8):
        return 4;
      case var n when (n >= 0.6):
        return 3;
      case var n when (n >= 0.4):
        return 2;
      case var n when (n >= 0.0):
        return 1;
      default:
        return 0;
    }
  }
}