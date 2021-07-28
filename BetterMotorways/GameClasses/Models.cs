using HarmonyLib;
using Motorways.Models;

namespace BetterMotorways.GameClasses
{
  class Models
  {
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
