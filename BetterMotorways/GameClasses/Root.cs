using HarmonyLib;
using BetterMotorways.Patches;

namespace BetterMotorways.GameClasses
{
  class Root
  {
    [HarmonyPatch(typeof(Game))]
    class BetterGame
    {
      static readonly CustomRules custom = new CustomRules();

      [HarmonyPostfix]
      [HarmonyPatch("Start")]
      public static void Start(ref Game __instance, GameStartReason gameStartReason)
      {
        Motorways.GameRules rules = __instance._simulation.Scope.Get<Motorways.GameRules>();
        //Helpers.DebugLine($"[Game] Starting {gameStartReason}. Creating BetterRules.");
        custom.AlterRules(ref rules);
      }
      [HarmonyPostfix]
      [HarmonyPatch("OnGameEnd")]
      public static void OnGameEnd(ref Game __instance, GameStartReason gameEndReason)
      {
        Motorways.GameRules rules = __instance._simulation.Scope.Get<Motorways.GameRules>();
        //Helpers.DebugLine($"[Game] Ending {gameEndReason}.");
        custom.RevertRules(ref rules);
      }
    }
  }
}
