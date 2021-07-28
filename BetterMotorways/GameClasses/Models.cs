using HarmonyLib;
using Motorways.Models;

namespace BetterMotorways.GameClasses
{
  class Models
  {
    [HarmonyPatch(typeof(ScoreModel))]
    class Score
    {
      //[HarmonyPatch("AddScore")]
      //public static bool Prefix(ref  ScoreModel __instance)
      //{
      //  // Could edit score here! Skipping as too much cheating..
      //  int score = __instance.Score;
      //  __instance.Score = score + 1;
      //  return false; // Skip original code
      //}
    }

    [HarmonyPatch(typeof(CarparkModel))]
    class Carpark
    {
      //[HarmonyPatch("AddDestination")]
      //public static void Postfix(ref CarparkModel __instance, DestinationModel model)
      //{
      //  // Could edit the amount of destinations added. Skipping..
      //}
    }

    [HarmonyPatch(typeof(RoadChunkModel))]
    class RoadChunk
    {
      [HarmonyPatch("InboundVehicleCollidesWithTraversingVehicle")]
      public static bool Prefix(RoadChunkModel __instance, RoadChunkModel.InboundVehicle inboundVehicle, ref bool __result)
      {
        return Patches.Simulation.VehiclesCollide(ref __result);
      }
    }
  }
}
