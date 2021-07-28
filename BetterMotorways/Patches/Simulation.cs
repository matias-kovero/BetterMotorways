using FixMath;

namespace BetterMotorways.Patches
{
  class Simulation
  {
    public static bool VehiclesCollide(ref bool isCollide)
    {
      if (Main.stopVehicleCollisions.Value)
      {
        isCollide = false;
        return false; // Skipping original code.
      }
      return true; // Return true for our prefix - original code will run.
    }
  }

  class CustomRules
  {
    private Motorways.GameRules defaults;

    public void AlterRules(ref Motorways.GameRules rules)
    {
      if (Main.useCustomRules.Value)
      {
        defaults = rules;
        Helpers.DebugLine($"[Rules] Using custom rules.");
        SetRules(ref rules);
      }
    }
    public void RevertRules(ref Motorways.GameRules rules)
    {
      if (defaults != null)
      {
        rules = defaults;
        Helpers.DebugLine($"[Rules] Reverting rules.");
      }
    }
    private void SetRules(ref Motorways.GameRules rules)
    {
      // Vehicles
      rules._constants.maxAcceleration = (Fix64)Main.maxAcceleration.Value;
      rules._constants.controlledIntersectionAcceleration = (Fix64)Main.controlledIntersectionAcceleration.Value;
      rules._constants.roundaboutAcceleration = (Fix64)Main.roundaboutAcceleration.Value;
      rules._constants.maxSpeedOnMotorways = (Fix64)Main.maxSpeedOnMotorways.Value;
      rules._constants.decelerationFactorAtIntersections = (Fix64)Main.decelerationFactorAtIntersections.Value;
      rules._constants.speedMultiplier = (Fix64)Main.speedMultiplier.Value;
      rules._constants.MaximumTimeToWaitAtIntersection = (Fix64)Main.MaximumTimeToWaitAtIntersection.Value;
      // Lanes
      rules._constants.defaultLaneSpeed = (Fix64)Main.defaultLaneSpeed.Value;
      rules._constants.sharpTurnSpeedMultiplier = (Fix64)Main.sharpTurnSpeedMultiplier.Value;
      rules._constants.rightAngleTurnSpeedMultiplier = (Fix64)Main.rightAngleTurnSpeedMultiplier.Value;
      rules._constants.intersectionSpeedMultiplier = (Fix64)Main.intersectionSpeedMultiplier.Value;
      rules._constants.roundaboutSpeedMultiplier = (Fix64)Main.roundaboutSpeedMultiplier.Value;
      rules._constants.useAverageLaneSpeedRatherThanMin = Main.useAverageLaneSpeedRatherThanMin.Value;
      // Traffic Lights
      rules._constants.changeDelay = (Fix64)Main.changeDelay.Value;
      rules._constants.overtimeChangeDelay = (Fix64)Main.overtimeChangeDelay.Value;
      rules._constants.amberDelay = (Fix64)Main.amberDelay.Value;
      rules._constants.minimumNearbyCarsBeforeSwapping = Main.minimumNearbyCarsBeforeSwapping.Value;
      rules._constants.distanceToCountForNearbyCars = (Fix64)Main.distanceToCountForNearbyCars.Value;
      rules._constants.MaximumIdleTimeAtTrafficLightBeforeMaxWeight = (Fix64)Main.MaximumIdleTimeAtTrafficLightBeforeMaxWeight.Value;
      rules._constants.IdleTimeAtTrafficLightWeightMultiplier = (Fix64)Main.IdleTimeAtTrafficLightWeightMultiplier.Value;
      // Building Spawning
      rules._constants.FailedHouseSpawnCooldown = (Fix64)Main.FailedHouseSpawnCooldown.Value;
      rules._constants.FailedDestinationRetryDelay = (Fix64)Main.FailedDestinationRetryDelay.Value;
      rules._constants.MaxFailedBuildingSpawnsBeforeIgnoringWeights = Main.MaxFailedBuildingSpawnsBeforeIgnoringWeights.Value;
      rules._constants.MaxFailedDoubleCarparkSpawnsBeforeCovertingToSingle = Main.MaxFailedDoubleCarparkSpawnsBeforeCovertingToSingle.Value;
      rules._constants.MinimumTimeBetweenDestinationSpawns = (Fix64)Main.MinimumTimeBetweenDestinationSpawns.Value;
      rules._constants.DelayBetweenSameGroupHouseSpawns = (Fix64)Main.DelayBetweenSameGroupHouseSpawns.Value;
      rules._constants._houseContributionMultiplier = (Fix64)Main.houseContributionMultiplier.Value;
      // Suburb Spawning
      rules._constants.MinimumSuburbCountScale = (Fix64)Main.MinimumSuburbCountScale.Value;
      rules._constants.MinimumSuburbCountExponent = (Fix64)Main.MinimumSuburbCountExponent.Value;
      rules._constants.MaximumSuburbCountScale = (Fix64)Main.MaximumSuburbCountScale.Value;
      rules._constants.MaximumSuburbCountExponent = (Fix64)Main.MaximumSuburbCountExponent.Value;
      rules._constants.MinimumSpawnAttemptsForSuburbMultiplier = Main.MinimumSpawnAttemptsForSuburbMultiplier.Value;
      rules._constants.MaximumSpawnAttemptsForSuburbMultiplier = Main.MaximumSpawnAttemptsForSuburbMultiplier.Value;
      rules._constants.MaximumDelayedBuildingSuburbCountMultiplier = (Fix64)Main.MaximumDelayedBuildingSuburbCountMultiplier.Value;
      // Big Pins
      rules._constants.MaxOvercrowdTime = (Fix64)Main.MaxOvercrowdTime.Value;
      rules._constants.GracePeriodTime = (Fix64)Main.GracePeriodTime.Value;
      rules._constants.OvercrowdTimerAcceleration = (Fix64)Main.OvercrowdTimerAcceleration.Value;
      rules._constants.OvercrowdTimerCarArrivalDeceleration = (Fix64)Main.OvercrowdTimerCarArrivalDeceleration.Value;
      rules._constants.OvercrowdTimerReturnSpeed = (Fix64)Main.OvercrowdTimerReturnSpeed.Value;
      rules._constants.PercentageToReduceTimerOnCarArrival = (Fix64)Main.PercentageToReduceTimerOnCarArrival.Value;
      rules._constants.MinimumAmountToReduceTimerOnCarArrival = (Fix64)Main.MinimumAmountToReduceTimerOnCarArrival.Value;
      rules._constants.MaximumAmountToReduceTimerOnCarArrival = (Fix64)Main.MaximumAmountToReduceTimerOnCarArrival.Value;
      rules._constants.MinimumOvercrowdTimerSpeed = (Fix64)Main.MinimumOvercrowdTimerSpeed.Value;
      rules._constants.MaximumOvercrowdTimerSpeed = (Fix64)Main.MaximumOvercrowdTimerSpeed.Value;
      // Demand
      rules._constants.DemandMultiplierForBuildings = (Fix64)Main.DemandMultiplierForBuildings.Value;
      rules._constants.DemandMultiplierForUpgradedBuildings = (Fix64)Main.DemandMultiplierForUpgradedBuildings.Value;
      rules._constants.AverageCarsPerDay = (Fix64)Main.AverageCarsPerDay.Value;
      rules._constants.DelayBeforeFirstPinOfDestination = (Fix64)Main.DelayBeforeFirstPinOfDestination.Value;
    }
  }
}
