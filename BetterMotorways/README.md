# Development / Tutorial
Small-ish tutorial to get you started with BepInEx modding framework. You can use this repository code for your base, or follow this tutorial to create one from scratch.  
Using BepInEx is relative straightforward and fast.  
It took ~1h to create BetterMotorways - altho a lot of that was debugging errors.

> Mini Motorways is an __32-bit__ game and uses unity __mono__ runtime. Other games might be different, you need to install correct version of BepInEx for those games. BepInEx documentation is your friend here.

### Table of Contents
[Getting Started](#getting-started)  
[Development Environment](#development-environment)  
[Mod Entrypoint](#mod-entrypoint)  
[Configuration](#configuration)  

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
## Build events
Few setrtings to make your developing a bit faster / easier.  
We want to copy our generated `.dll` and copy straight to BepInEx plugins folder.  
Go to Project -> Properties -> Build Events -> Post Build Event

> Change "Bepinex_PATH" to the patch were you have bepinex. Should be direct path, ex:
`C:\Program Files (x86)\Steam\steamapps\common\Mini Motorways\Bepinex\`

If you aren't using ScriptEngine (default)
```bash
xcopy "$(TargetDir)" "Bepinex_PATH\plugins\$(ProjectName)\" /q /s /y /i
```
If you are using ScriptEngine. Debug version creates hotswapped version. Release creates both.
```bash
if $(ConfigurationName) == Debug (xcopy "$(TargetPath)" "Bepinex_PATH\scripts\" /q /s /y /i) ELSE (xcopy "$(TargetDir)" "Bepinex_PATH\plugins\$(ProjectName)\" /q /s /y /i & xcopy "$(TargetPath)" "Bepinex_PATH\scripts\" /q /s /y /i)
```
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
This is totally optional but I like to have an file structure:
```
|-- GameClasses
  |-- Class1.cs
  |-- Class2.cs
|-- Patches
  |-- Patch1.cs
|-- Main.cs
```
Game classes are referenced to classes found in game assbembly. Use dnSpy to check these classes in `App.dll`  
Patches will have files that will have our own custom code that will patch game functions.

## Finding an function to patch
You will need to analyze the game code with dnSpy to find functions you would like to edit. I'll show an example to remove checks on vehicle collisions.  

Under `Motorways.Models.RoadChunkModel` there is an function  
`public bool InboundVehicleCollidesWithTraversingVehicle(RoadChunkModel.InboundVehicle inboundVehicle)`.

To parse down the logic of the above:
```c#
class Motorways
{
  class Models
  {
    class RoadChunkModel
    {
      public bool InboundVehicleCollidesWithTraversingVehicle
      (RoadChunkModel.InboundVehicle inboundVehicle)
      {
        // Original game code here
      }
    }
  }
}
```

Lets create an file under GameClasses named: `Models.cs` as it will contain patches for `Motorways.Models`
```c#
namespace BetterMotorways.GameClasses // Your namespace will vary.
{
  class Models
  {
    
  }
}
```
Then as the function is inside class `RoadChunkModel` we will create our own patching class to it, place this inside the created models class.
```c#
using HarmonyLib;
using Motorways.Models;

//...
[HarmonyPatch(typeof(RoadChunkModel))]
class RoadChunk
{

}
```
And inside our patching class we will add our function patch.

```c#
[HarmonyPatch("InboundVehicleCollidesWithTraversingVehicle")]
public static bool Prefix(RoadChunkModel __instance, RoadChunkModel.InboundVehicle inboundVehicle, ref bool __result)
{
  __result = false;
  return false;
}
```
This might be the hardest part, reference to Harmony syntax [here](https://harmony.pardeike.net/articles/patching-prefix.html).
Main points of above code:
- We are creating an Prefix (code that runs before the original code)
- We return `false` from our prefix, it will skip the original code.
- As the original code returned an bool, we want to return something aswell.
  - `ref bool __result` is an reference to the result (Harmony syntax)
  - Setting this to the desired value will patch the original function.

Outcome is that everytime the game now tries to check if an vechicle is colliding with an other vehicle we return `false`.

Your `Models.cs` should look something like this:
```c#
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
        __result = false;
        return false;
      }
    }
  }
}
```

## Centalizing your patches
As we are following the class structure of the original game, you will create bunch of classes.
> In the long run it is easier to update your code as it follows the same hierarchy as the original game code.

But our patches are now spreaded in multiple classes. This is where the Patches folder comes to use.
Create new file to Patches i'll create an file named Simulation.cs

```c#
namespace BetterMotorways.Patches
{
  class Simulation
  {
    public static bool VehiclesCollide()
    {
      return false;
    }
  }
}
```
And now editing our code in Models.cs
```c#
__result = Patches.Simulation.VehiclesCollide();
return false;
```
In this simple example it does not seems neccecary to move our patching code under Patches folder, but in more complex patches where you would need communication between functions, it is easier to have them in the same class.

# Configuration
We have now created an really simple patch that will everytime return false when the game check if an vehicle is colliding. Now we should give the user an option to select if they want to use our patch.

### Creating an ConfigEntry
First lets add an config option under your Mod Entrypoint in this case its in `Main.cs`
```c#
using BepInEx.Configuration;

//...

/* Config Values */
public static ConfigEntry<bool> stopVehicleCollisions;

//...

public void Awake()
{
  stopVehicleCollisions = Config.Bind("Simulation", nameof(stopVehicleCollisions), false);
}
```
First we define an ConfigEntry named stopVehicleCollisions and inside `Awake` we give it an default value `false`

Now when an users runs our mod, it will create an config file under `BepInEx\config\YourModName.cfg`

### Using the ConfigEntry
Let's go back to our patching code under `Simulation.cs`  
We could just reference our config value, and return true or false based on the users option.

```c#
public static bool VehiclesCollide()
{
  return Main.stopVehicleCollisions.Value;
}
```
However, this could cause issues. Our Prefix is still returning false, and it will skip the original code.
```c#
[HarmonyPatch("InboundVehicleCollidesWithTraversingVehicle")]
public static bool Prefix(RoadChunkModel __instance, RoadChunkModel.InboundVehicle inboundVehicle, ref bool __result)
{
  __result = Patches.Simulation.VehiclesCollide();
  return false;
}
```

To fix this, we need to alter the code a bit. This is totally optional and the above code should work just fine. But to avoid future issues we should revert to use the original code when an option is turned off.  

`Simulation.cs`
```C#
public static bool VehiclesCollide(ref bool isCollide)
{
  if (Main.stopVehicleCollisions.Value)
  {
    isCollide = false;
    return false; // Skipping original code.
  }
  return true; // Return true for our prefix - original code will run.
}
```
`Models.cs`
```C#
[HarmonyPatch("InboundVehicleCollidesWithTraversingVehicle")]
public static bool Prefix(RoadChunkModel __instance, RoadChunkModel.InboundVehicle inboundVehicle, ref bool __result)
{
  return Patches.Simulation.VehiclesCollide(ref __result);
}
```