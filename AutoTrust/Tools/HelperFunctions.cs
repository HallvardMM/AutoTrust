namespace AutoTrust;
using System.Globalization;
using System.Text.RegularExpressions;

#pragma warning disable SYSLIB1045

public class HelperFunctions {
  public static int GetLastPageNumber(string? linkHeader) {
    if (linkHeader is null) {
      return -1;
    }
    var match = Regex.Match(linkHeader, @"&page=(\d+)[^>]*>; rel=""last""");
    if (int.TryParse(match.Groups[1].Value, CultureInfo.InvariantCulture, out var regularNumber)) {
      return regularNumber;
    }
    else {
      return -1;
    }
  }

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
    return total_score_percentage switch {
      var n when n >= 0.9 => 5,
      var n when n >= 0.8 => 4,
      var n when n >= 0.6 => 3,
      var n when n >= 0.4 => 2,
      var n when n >= 0.0 => 1,
      _ => 0,
    };
  }
}
