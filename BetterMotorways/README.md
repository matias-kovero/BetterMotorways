# Development / Tutorial
Small-ish tutorial to get you started with BepInEx modding framework. You can use this repository code for your base, or follow this tutorial to create one from scratch.  
Using BepInEx is relative straightforward and fast.  
It took ~1h to create BetterMotorways - altho a lot of that was debugging errors.

> Mini Motorways is an __32-bit__ game and uses unity __mono__ runtime. Other games might be different, you need to install correct version of BepInEx for those games. BepInEx documentation is your friend here.

### Table of Contents
[Getting Started](#getting-started)  
[Development Environment](#development-environment)  
[Mod Entrypoint](#mod-entrypoint)  

# Getting started
- Install BepInEx ([Github](https://github.com/BepInEx/BepInEx) & [Documentation](https://docs.bepinex.dev/master/articles/user_guide/installation/index.html))
- Install dnSpy ([Github](https://github.com/dnSpy/dnSpy))
- Install AssemblyPublicizer ([Github](https://github.com/CabbageCrow/AssemblyPublicizer)) _*Optional_

> Publicized Assemblies are needed if you want IDE support for private functions.

At this point you should have BepInEx extracted to your game folder.   
In this case under `..\Steam\steamapps\common\Mini Motorways`

# Development Environment
Open Visual Studio (or preferred IDE) and  
Create New Project with:
-  `Class Library (.NET Framework)`
-  Framework target: `.NET Framework 4.X` (I took 4.5)

This is because:
- Managed folder does __not__ contain `netstandard.dll` we should target `netframework`
- `..\Mini Motorways\Mini Motorways_Data\Managed\mscorlib.dll` version is `4.0.0.0`

More info: [BepInEx documentation](https://docs.bepinex.dev/master/articles/dev_guide/plugin_tutorial/1_setup.html)  
Notes: Use dnSpy to check version of the `.dll`

## Add Dependecies / References
Create an Libs folder in your solution folder and copy `.dll` there.  
Following `.dlls` are mandatory:
```js
UnityEngine.dll // Found under `<Game Name>_Data\Managed`
UnityEngine.CoreModule.dll //`<Game Name>_Data\Managed`
BepInEx.dll // Found under `BepInEx\core`
0Harmony.dll // `BepInEx\core`
BepInEx.Harmony.dll // `BepInEx\core`
```
Game specific `.dlls`
```js
App.dll // Found under `<Game Name>_Data\Managed`
```
Notes: Use dnSpy to locate the `.dll` that contains game code. If you want support for private functions use Assebly Publicize on this and add the publicized `.dll` to references.

Then on your Visual Studio right click References -> Add References -> Browse.  
Select all `.dll` under the _Libs_ file you just copied your files and press OK.  
Then individually click these .dlls and select `Copy Local: False`

# Mod Entrypoint
Open your entrypoint `.cs` file. Named `Class1.cs` or `Main.cs`. It is the `.cs` that is in the solution root.

Here is an basic snippet to get everything working.

```c#
using System.Reflection;
using BepInEx;
using HarmonyLib;

namespace BetterMotorways
{
  [BepInPlugin(GUID, MODNAME, VERSION)]
  public class Main : BaseUnityPlugin
  {
    public const string
      MODNAME = "MOD_NAME",
      AUTHOR = "AUTHOR_NAME",
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
      // If we are going to use config, we need to define them here.
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
```
This snippet does not have any configs. If you want to use configs, check my [source](./Main.cs)

# Creating Patches
TODO
