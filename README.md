# Better Motorways
Mini Motorways is a strategy simulation game about designing the road map for a growing city.  
__Better Motorways__ is an BepInEx Mod allowing modification of the core game.

> __Disclaimer__:   
This mod modifies game runtime, it might be seen as hacking.  
Currently it seems there aren't any counter measurements for _"hacking"_
### Features

## Usage
Install [BepInEx](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.13) for 32bit (x86)  
> The game is 32bit, this is why you need to install 32bit BepInEx.  

Extract BepInEx to game folder.  
Located in: `..\Steam\steamapps\common\Mini Motorways`  
Your folder should look something like:
```
|-- BepInEx
|-- Mini Motorways_Data
|-- doorstop_config
|-- Mini Motorways.exe
|-- winhttp.dll
...
...
```
Install this mod under Releases. Extract the folder from the installed `.zip` 
and move it under `BepInEx\plugins\` 

Your plugins folder should look like
```
|-- BetterMotorways
  | -- BetterMotorways.dll
```
Run the game once and close it. This will create an config file under: `BepInEx\config`
Edit values under created config file and enjoy the mod.

## Development
Information on development [here](/BetterMotorways/).
