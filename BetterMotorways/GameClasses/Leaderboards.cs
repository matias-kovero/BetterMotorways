using HarmonyLib;
using Motorways.Leaderboards;

namespace BetterMotorways.GameClasses
{
  class Leaderboards
  {
    [HarmonyPatch(typeof(LeaderboardService))]
    class Service
    {
      //[HarmonyPatch("SubmitScore")]
      //public static void Prefix(ref LeaderboardService __instance, LeaderboardId leaderboardId, ref int score, LeaderboardScoreState scoreState)
      //{
      //  if (scoreState == LeaderboardScoreState.NotSubmitted)
      //  {
      //    // Score state should never be set to NotSubmitted.
      //    return;
      //  }
      //  // Could edit score here! Skipping as too much cheating..
      //  // Hard-coded score for every game.
      //  score = 1;
      //  // Dynamic Score
      //  score = (int)(score * 1.25);
      //}
    }
  }
}
