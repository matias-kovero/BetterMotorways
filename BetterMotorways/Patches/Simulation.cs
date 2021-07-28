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
}
