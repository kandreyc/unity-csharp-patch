Unity C# Patch
================
The main purpose of this package is to allow using newer C# version per ``.asmdef``.

This repository is based on this awesome repos, which was the main motivation for my project (this package itslef has no dependencies):
1. [CsprojModifier](https://github.com/Cysharp/CsprojModifier)
2. [UnityRoslynUpdater](https://github.com/DaZombieKiller/UnityRoslynUpdater)

So the problem of [UnityRoslynUpdater](https://github.com/DaZombieKiller/UnityRoslynUpdater) is that it increases C# version for everything. So all the code in your project will be compiled with newer C# version.
This is fine for a small hobby project, but for medium to big production projects, where a lot of libraries / packages / sdks are used, it can become a problem, because there can be variables / fields with names that has been reserved in a newer C# versions etc.

I've adjusted / extended it with [CsprojModifier](https://github.com/Cysharp/CsprojModifier)'s help to support custom C# version per ``.asmdef/.csproj`` file. So only your code will have increased C# version.

How it works:
-------------
1. **Patch console application** replaces the dotnet sdk inside editor with a newer one (Unity itself already uses dotnet but with version ``6.0.21``).
2. **Unity package** tracks if ``.asmdef`` has specified information about C# launguage version (specified manually inside ``.asmdef``)
3. **Unity package** adds ``csc.rsp`` file with specified C# version. So that now Unity knows how to compile the code of your ``.asmdef``
4. **Unity package** adjusts the ``.csproj`` with specified C# version. So that your IDE knows the C# version and unlocks all the features related to it.

How to Install:
---------------
> [!NOTE]  
> Backup your editor to not redownload it again if something will not work on your operation system or with your unity editor version.
>
> Not all language features will be available. the provided list of them is here [UnityRoslynUpdater](https://github.com/DaZombieKiller/UnityRoslynUpdater)
> 
> Patch will modify the Editor installation, so all the projects that are using it will be affected (default for unity C# version will be used for compilation, but from newer (patched) dotnet sdk)

1. Add the package via git url ``url``
2. Ensure Unity Editor is closed
3. Patch the editor. The patch app is located inside the package at ``path/to/patch_app``
```
$ ./UnityEditorPatch apply --sdk '/usr/local/share/dotnet' --editor '/Applications/Unity/Hub/Editor/2022.3.21f1'
```
where ``--sdk`` - path to the dotnet, ``--editor`` - path to the unity editor

In case if you want to revert the patch:
```
$ ./UnityRoslynUpdater revert --editor '/Applications/Unity/Hub/Editor/2022.3.21f1')
```
where ``--editor`` - path to the unity editor

4. Open the Unity Editor with your project and add this lines to the end of your ``.asmdef`` file:
```
  "unityCSharpPatch": {
    "langVersion": "12",
    "nullable": "enable"
  }
```
where:

- ``langVersion`` (optional) is the C# version you want to be used for this ``.asmdef``. Values are ``10``, ``11``, ``12``
- ``nullable`` (optional) allows to use nullables like ``string?`` without defining ``#nullable enable/disable`` in each file where it used. Values are ``enable``, ``disable``

5. Refresh the Editor. All required magic should be done here.
6. Enjoy!
