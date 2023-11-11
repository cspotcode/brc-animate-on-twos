# Bomb Rush Cyberfunk Mod/Plugin Template

I took YuriLewd's Bomb Rush Cyberfunk plugin template, configured it for VSCode, added scripts to automate annoying tasks, wrote this README, hosted as a Github Template repo.

## See also

Youtube guide: https://www.youtube.com/watch?v=KopYonyplXs  
Links to project template: https://github.com/mroshaw/UnityModVSTemplate/tree/main/Templates/ProjectTemplates/C%23/Unity%20Mod%20(BepInEx)

BepInEx guide: https://docs.bepinex.dev/articles/dev_guide/plugin_tutorial/index.html

Guide to ripping and running the game in-editor: https://github.com/cspotcode/bomb-rush-cyberfunk-modding/

## Assumptions

This guide assumes you can use git w/github.

This guide was written for Visual Studio Code, but it should work in Rider and Visual Studio.  Only a few things are
specific to VSCode and should be easy to avoid.

## Setup

Install dotnet: https://dotnet.microsoft.com/en-us/download

Install VSCode: https://code.visualstudio.com/

Install Unity Editor 2021.3.27f1: https://unity.com/releases/editor/whats-new/2021.3.27

Click "Use this template" to clone this code into your own repository.  Clone it, open it in VSCode.

Install recommended extensions for this project.  VSCode should auto-prompt for this(?) `.vscode/extensions.json`

Find and replace (Ctrl+Shift+H) `SafeProjectName` with a C# namespace for your plugin to live in, something like `DDRCypherMinigame` or whatever.

Open `AssemblyInfo.cs` and set a GUID at a minimum, also customize the other fields.  Ctrl+Shift+P, "Copy new UUID to clipboard", paste it in.

Open `MyPlugin.cs` and customize strings.

Download pdb2mdb, put it at `./scripts/pdb2mdb.exe`: https://docs.bepinex.dev/articles/advanced/debug/plugins_vs.html

Generate publicized game dll.  PowerShell script `./scripts/generate-publicized-assemblies.ps1` may do the trick. If it's confused about install location of BRC,
pass as `./scripts/generate-publicized-assemblies.ps1 -brcInstallDirectory PATH_HERE` or jump to next step to fix `.csproj` variables and then come back.

Press Ctrl+Shift+B to build.  If there are errors about non-`publicized` missing assemblies,
then the paths in `.csproj` are wrong. Open the `.csproj` file and check
the `<Reference>` elements.  They refer to variables defined in `<PropertyGroup>` at the top of the file.  Usually modifying the variables is the easiest
way to fix the build.

## Debugging

You can optionally attach a debugger to the game while it's running your mods, allowing you to set breakpoints, inspect objects, etc.

This requires additional setup, but I wrote a script to do it for you.

The script `./scripts/convert-to-debug-build.ps1` will convert your game to a debug build.

_**BE CAREFUL**, back up your stuff, the script might break everything!_

See also: I followed this guide: https://github.com/dnSpy/dnSpy/wiki/Debugging-Unity-Games#turning-a-release-build-into-a-debug-build  

With the game running, in VSCode run command "Attach Unity Debugger."  In Rider use "Run"->"Attach to Unity Process"

VSCode's debugger appears to have a bug where breakpoints can get stuck enabled. That is, I hit a breakpoint, try to remove it and resume, but the game keeps hitting the breakpoint every frame.  Restarting the debugger might fix it. (Ctrl+Shift+F5)

See also: https://code.visualstudio.com/docs/csharp/debugging

## Thanks

YuriLewd for sharing this template and other guidance on Discord

LazyDuchess's helpful README explaining BepInEx.AssemblyPublicizer: https://github.com/LazyDuchess/BRC-PhotoStorage#building-from-source

NotNite for information about attaching a debugger and other guidance

All the modders on Team Reptile's Discord