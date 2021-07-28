using System.Reflection;
using BepInEx;
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
      VERSION = "0.0.1";

    internal readonly Harmony harmony;
    internal readonly Assembly assembly;

    public static readonly bool isDebug = true;
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
