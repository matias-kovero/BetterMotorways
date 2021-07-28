using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace BetterMotorways
{
  [BepInPlugin(GUID, MODNAME, VERSION)]
  public class Main : BaseUnityPlugin
  {
    public const string
      MODNAME = "BetterMotorways",
      AUTHOR = "MK",
      GUID = AUTHOR + "-" + MODNAME,
      VERSION = "1.0.0";

    internal readonly Harmony harmony;
    internal readonly Assembly assembly;

    public static readonly bool isDebug = false;

    /* Simulation */
    public static ConfigEntry<bool> stopVehicleCollisions;
    /* Game Rules */
    public static ConfigEntry<bool> useCustomRules;
    // Vechicles
    public static ConfigEntry<float> maxAcceleration;
    public static ConfigEntry<float> controlledIntersectionAcceleration;
    public static ConfigEntry<float> roundaboutAcceleration;
    public static ConfigEntry<long> maxSpeedOnMotorways;
    public static ConfigEntry<float> decelerationFactorAtIntersections;
    public static ConfigEntry<float> speedMultiplier;
    public static ConfigEntry<long> MaximumTimeToWaitAtIntersection;
    // Lanes
    public static ConfigEntry<float> defaultLaneSpeed;
    public static ConfigEntry<float> sharpTurnSpeedMultiplier;
    public static ConfigEntry<float> rightAngleTurnSpeedMultiplier;
    public static ConfigEntry<float> intersectionSpeedMultiplier;
    public static ConfigEntry<float> roundaboutSpeedMultiplier;
    public static ConfigEntry<bool> useAverageLaneSpeedRatherThanMin;
    // Traffic Lights
    public static ConfigEntry<long> changeDelay;
    public static ConfigEntry<long> overtimeChangeDelay;
    public static ConfigEntry<long> amberDelay;
    public static ConfigEntry<int> minimumNearbyCarsBeforeSwapping;
    public static ConfigEntry<long> distanceToCountForNearbyCars;
    public static ConfigEntry<long> MaximumIdleTimeAtTrafficLightBeforeMaxWeight;
    public static ConfigEntry<float> IdleTimeAtTrafficLightWeightMultiplier;
    public static ConfigEntry<bool> americanRedLightRules;
    public static ConfigEntry<bool> greenLightsIgnoreCollisions;
    // Building Spawning
    public static ConfigEntry<long> FailedHouseSpawnCooldown;
    public static ConfigEntry<long> FailedDestinationRetryDelay;
    public static ConfigEntry<int> MaxFailedBuildingSpawnsBeforeIgnoringWeights;
    public static ConfigEntry<int> MaxFailedDoubleCarparkSpawnsBeforeCovertingToSingle;
    public static ConfigEntry<long> MinimumTimeBetweenDestinationSpawns;
    public static ConfigEntry<long> DelayBetweenSameGroupHouseSpawns;
    public static ConfigEntry<float> houseContributionMultiplier;
    // Suburb Spawning
    public static ConfigEntry<float> MinimumSuburbCountScale;
    public static ConfigEntry<float> MinimumSuburbCountExponent;
    public static ConfigEntry<float> MaximumSuburbCountScale;
    public static ConfigEntry<float> MaximumSuburbCountExponent;
    public static ConfigEntry<int> MinimumSpawnAttemptsForSuburbMultiplier;
    public static ConfigEntry<int> MaximumSpawnAttemptsForSuburbMultiplier;
    public static ConfigEntry<long> MaximumDelayedBuildingSuburbCountMultiplier;
    // Big Pins
    public static ConfigEntry<long> MaxOvercrowdTime;
    public static ConfigEntry<long> GracePeriodTime;
    public static ConfigEntry<float> OvercrowdTimerAcceleration;
    public static ConfigEntry<float> OvercrowdTimerCarArrivalDeceleration;
    public static ConfigEntry<float> OvercrowdTimerReturnSpeed;
    public static ConfigEntry<long> PercentageToReduceTimerOnCarArrival;
    public static ConfigEntry<long> MinimumAmountToReduceTimerOnCarArrival;
    public static ConfigEntry<long> MaximumAmountToReduceTimerOnCarArrival;
    public static ConfigEntry<long> MinimumOvercrowdTimerSpeed;
    public static ConfigEntry<long> MaximumOvercrowdTimerSpeed;
    // Demand
    public static ConfigEntry<float> DemandMultiplierForBuildings;
    public static ConfigEntry<float> DemandMultiplierForUpgradedBuildings;
    public static ConfigEntry<float> AverageCarsPerDay;
    public static ConfigEntry<long> DelayBeforeFirstPinOfDestination;

    /**
     * Constructor
     */
    public Main()
    {
      harmony = new Harmony(GUID);
      assembly = Assembly.GetExecutingAssembly();
    }
    /**
     * Called once when both the game and the plug-in are loaded
     */
    public void Awake()
    {
      // Simulation
      stopVehicleCollisions = Config.Bind("Simulation", nameof(stopVehicleCollisions), false);
      // CustomRules
      useCustomRules = Config.Bind("GameRules", nameof(useCustomRules), false);
      // Vehicles
      maxAcceleration = Config.Bind("GameRules.Vehicle", nameof(maxAcceleration), 0.6f);
      controlledIntersectionAcceleration = Config.Bind("GameRules.Vehicle", nameof(controlledIntersectionAcceleration), 0.6f);
      roundaboutAcceleration = Config.Bind("GameRules.Vehicle", nameof(roundaboutAcceleration), 1f);
      maxSpeedOnMotorways = Config.Bind("GameRules.Vehicle", nameof(maxSpeedOnMotorways), 3L);
      decelerationFactorAtIntersections = Config.Bind("GameRules.Vehicle", nameof(decelerationFactorAtIntersections), 0f,
        new ConfigDescription("Controls how much a car will slow at intersections. 0 means they slow a lot, 2 means not at all.", new AcceptableValueRange<float>(0f, 2f)));
      speedMultiplier = Config.Bind("GameRules.Vehicle", nameof(speedMultiplier), 1f);
      MaximumTimeToWaitAtIntersection = Config.Bind("GameRules.Vehicle", nameof(MaximumTimeToWaitAtIntersection), 45L,
        "How long to wait before just going");
      // Lanes
      defaultLaneSpeed = Config.Bind("GameRules.Lanes", nameof(defaultLaneSpeed), 1f,
        "Base lane speed.");
      sharpTurnSpeedMultiplier = Config.Bind("GameRules.Lanes", nameof(sharpTurnSpeedMultiplier), 0.3333333333333333f,
        "How slow do cars go on a hairpin turn?");
      rightAngleTurnSpeedMultiplier = Config.Bind("GameRules.Lanes", nameof(rightAngleTurnSpeedMultiplier), 0.6666666666666666f,
        "How slow do cars go on a right hand turn?");
      intersectionSpeedMultiplier = Config.Bind("GameRules.Lanes", nameof(intersectionSpeedMultiplier), 0.5f,
        "How slow do cars go when heading towards an intersection?");
      roundaboutSpeedMultiplier = Config.Bind("GameRules.Lanes", nameof(maxAcceleration), 2f,
        "How fast do cars go when heading towards a roundabout?");
      useAverageLaneSpeedRatherThanMin = Config.Bind("GameRules.Lanes", nameof(useAverageLaneSpeedRatherThanMin), true);
      // Traffic Lights
      changeDelay = Config.Bind("GameRules.TrafficLights", nameof(changeDelay), 10L,
        "The minimum delay before traffic lights changing (seconds).");
      overtimeChangeDelay = Config.Bind("GameRules.TrafficLights", nameof(overtimeChangeDelay), 5L,
        "The delay before changing traffic lights if in overtime (seconds).");
      amberDelay = Config.Bind("GameRules.TrafficLights", nameof(amberDelay), 2L,
        "The duration of amber lights (seconds).");
      minimumNearbyCarsBeforeSwapping = Config.Bind("GameRules.TrafficLights", nameof(minimumNearbyCarsBeforeSwapping), 2,
        "If there are fewer than this many cars nearby, don't swap just yet.");
      distanceToCountForNearbyCars = Config.Bind("GameRules.TrafficLights", nameof(distanceToCountForNearbyCars), 2L);
      MaximumIdleTimeAtTrafficLightBeforeMaxWeight = Config.Bind("GameRules.TrafficLights", nameof(MaximumIdleTimeAtTrafficLightBeforeMaxWeight), 30L);
      IdleTimeAtTrafficLightWeightMultiplier = Config.Bind("GameRules.TrafficLights", nameof(IdleTimeAtTrafficLightWeightMultiplier), 1f);
      americanRedLightRules = Config.Bind("GameRules.TrafficLights", nameof(americanRedLightRules), false);
      greenLightsIgnoreCollisions = Config.Bind("GameRules.TrafficLights", nameof(greenLightsIgnoreCollisions), false);
      // Building Spawning
      FailedHouseSpawnCooldown = Config.Bind("GameRules.BuildingSpawning", nameof(FailedHouseSpawnCooldown), 2L);
      FailedDestinationRetryDelay = Config.Bind("GameRules.BuildingSpawning", nameof(FailedDestinationRetryDelay), 20L);
      MaxFailedBuildingSpawnsBeforeIgnoringWeights = Config.Bind("GameRules.BuildingSpawning", nameof(MaxFailedBuildingSpawnsBeforeIgnoringWeights), 5,
        new ConfigDescription("Max Failed Building Spawns Before Ignoring Tile Weights.", new AcceptableValueRange<int>(0, 100)));
      MaxFailedDoubleCarparkSpawnsBeforeCovertingToSingle = Config.Bind("GameRules.BuildingSpawning", nameof(MaxFailedDoubleCarparkSpawnsBeforeCovertingToSingle), 10,
        new ConfigDescription("Max Failed Double Carpark Spawns Before Converting To a Single Carpark.", new AcceptableValueRange<int>(0, 100)));
      MinimumTimeBetweenDestinationSpawns = Config.Bind("GameRules.BuildingSpawning", nameof(MinimumTimeBetweenDestinationSpawns), 10L);
      DelayBetweenSameGroupHouseSpawns = Config.Bind("GameRules.BuildingSpawning", nameof(DelayBetweenSameGroupHouseSpawns), 10L);
      houseContributionMultiplier = Config.Bind("GameRules.BuildingSpawning", nameof(houseContributionMultiplier), 1f);
      // Suburb Spawning
      MinimumSuburbCountScale = Config.Bind("GameRules.SuburbSpawning", nameof(MinimumSuburbCountScale), 0.7f);
      MinimumSuburbCountExponent = Config.Bind("GameRules.SuburbSpawning", nameof(MinimumSuburbCountExponent), 1.2f);
      MaximumSuburbCountScale = Config.Bind("GameRules.SuburbSpawning", nameof(MaximumSuburbCountScale), 0.4f);
      MaximumSuburbCountExponent = Config.Bind("GameRules.SuburbSpawning", nameof(MaximumSuburbCountExponent), 1.4f);
      MinimumSpawnAttemptsForSuburbMultiplier = Config.Bind("GameRules.SuburbSpawning", nameof(MinimumSpawnAttemptsForSuburbMultiplier), 5, "Min value: 0");
      MaximumSpawnAttemptsForSuburbMultiplier = Config.Bind("GameRules.SuburbSpawning", nameof(MaximumSpawnAttemptsForSuburbMultiplier), 10, "Min value: 0");
      MaximumDelayedBuildingSuburbCountMultiplier = Config.Bind("GameRules.SuburbSpawning", nameof(MaximumDelayedBuildingSuburbCountMultiplier), 4L);
      // Big Pins
      MaxOvercrowdTime = Config.Bind("GameRules.BigPins", nameof(MaxOvercrowdTime), 90L);
      GracePeriodTime = Config.Bind("GameRules.BigPins", nameof(GracePeriodTime), 2L,
        "The chunk of time at the end of the overcrowd timer that is not displayed.");
      OvercrowdTimerAcceleration = Config.Bind("GameRules.BigPins", nameof(OvercrowdTimerAcceleration), 0.02f);
      OvercrowdTimerCarArrivalDeceleration = Config.Bind("GameRules.BigPins", nameof(OvercrowdTimerCarArrivalDeceleration), 0.5f);
      OvercrowdTimerReturnSpeed = Config.Bind("GameRules.BigPins", nameof(OvercrowdTimerReturnSpeed), 2f);
      PercentageToReduceTimerOnCarArrival = Config.Bind("GameRules.BigPins", nameof(PercentageToReduceTimerOnCarArrival), 10L,
        "The percentage to reduce the overcrowding timer at the instant a vehicle picks up a pin. Note this is actually a percentage and goes from 0 to 100.");
      MinimumAmountToReduceTimerOnCarArrival = Config.Bind("GameRules.BigPins", nameof(MinimumAmountToReduceTimerOnCarArrival), 0L,
        "The minimum amount to reduce the timer, in virtualized seconds, when a vehicle picks up a pin.");
      MaximumAmountToReduceTimerOnCarArrival = Config.Bind("GameRules.BigPins", nameof(MaximumAmountToReduceTimerOnCarArrival), 3L,
        "The maximum amount to reduce the timer, in virtualized seconds, when a vehicle picks up a pin.");
      MinimumOvercrowdTimerSpeed = Config.Bind("GameRules.BigPins", nameof(MinimumOvercrowdTimerSpeed), 0L);
      MaximumOvercrowdTimerSpeed = Config.Bind("GameRules.BigPins", nameof(MaximumOvercrowdTimerSpeed), 1L);
      // Demand
      DemandMultiplierForBuildings = Config.Bind("GameRules.Demand", nameof(MinimumSuburbCountScale), 0.8f);
      DemandMultiplierForUpgradedBuildings = Config.Bind("GameRules.Demand", nameof(DemandMultiplierForUpgradedBuildings), 1.6f);
      AverageCarsPerDay = Config.Bind("GameRules.Demand", nameof(AverageCarsPerDay), 1.55f);
      DelayBeforeFirstPinOfDestination = Config.Bind("GameRules.Demand", nameof(DelayBeforeFirstPinOfDestination), 4L);
    }
    /**
     * Entrypoint for Harmony plugins.
     */
    public void Start()
    {
      harmony.PatchAll(assembly);
    }
    /**
     * If using ScriptEngine to hotswap, make sure to deallocate / destroy patches.
     */
    public void OnDestroy()
    {
      harmony?.UnpatchSelf();
    }
  }
}
