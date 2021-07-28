using UnityEngine;

namespace BetterMotorways.Patches
{
  class Helpers
  {
    public static void DebugLine(string str = "", bool pref = true, bool warn = false)
    {
      if (Main.isDebug)
      {
        if (warn) Debug.LogWarning($"{(pref ? $"[{typeof(Main).Namespace}] " : "")}{str}");
        else Debug.Log($"{(pref ? $"[{typeof(Main).Namespace}] " : "")}{str}");
      }
    }
  }
}
