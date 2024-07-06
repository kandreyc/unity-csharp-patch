Unity C# Patch
================
[![release](https://github.com/kandreyc/unity-csharp-patch/actions/workflows/release.yml/badge.svg)](https://github.com/kandreyc/unity-csharp-patch/actions/workflows/release.yml)
![Static Badge](https://img.shields.io/badge/Unity-2022%2B-black.svg?style=flat&logo=unity&color=%231f1f1f)

This package allows to set custom C# version per ``.asmdef``.

How it works:
-------------
1. **UnityEditorPatch** replaces the dotnet sdk inside editor with a newer one (Unity itself already uses dotnet but with version ``6.0.21``).
2. **UnityPackage** tracks if ``.asmdef`` has specified information about C# launguage version (specified manually inside ``.asmdef``)
3. **UnityPackage** adds ``csc.rsp`` file with specified C# version. So that Unity knows how to compile the code of your ``.asmdef``
4. **UnityPackage** adjusts the ``.csproj`` with specified C# version. So that your IDE knows the C# version and unlocks all the features related to it.

How to Install:
---------------
> [!NOTE]  
> Backup your editor to not redownload it again if something will not work on your operation system or with your unity editor version.
>
> Patch will modify the Editor installation, so all the projects that are using it will be affected (default for unity C# version will be used for compilation, but from newer (patched) dotnet sdk)

1. Add the package via git url ``https://github.com/kandreyc/unity-csharp-patch.git#v1.0.0``
2. Ensure Unity Editor is closed.
3. Ensure latest stable dotnet sdk is installed. [Download Page](https://dotnet.microsoft.com/en-us/download)
4. Open terminal at folder ``EditorPatch~`` inside the added package.
5. Patch the editor (administrative privileges are required):
```
$ dotnet UnityEditorPatch.dll apply --editor '/Applications/Unity/Hub/Editor/2022.3.21f1'
```
where ``--editor`` - path to the unity editor

In case if you want to revert the patch:
```
$ dotnet UnityEditorPatch.dll revert --editor '/Applications/Unity/Hub/Editor/2022.3.21f1'
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

- ``langVersion`` (optional) - C# version you want to be used for this ``.asmdef``. Values are ``10``, ``11``, ``12``
- ``nullable`` (optional) - allows to use nullables like ``string?`` without defining ``#nullable enable/disable`` in each file where it used. Values are ``enable``, ``disable``

5. Refresh the Editor. All required magic should be done here.
6. Enjoy!

Language Support
----------------
C# | Feature | Support
-|-|:-----:
12 | [Primary constructors](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-12#primary-constructors) | Yes
12 | [Optional parameters in lambda expressions](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-12#default-lambda-parameters) | Yes
12 | [Alias for any type](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-12#alias-any-type) | Yes
12 | [Inline arrays](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-12#inline-arrays) | No
12 | [Collection expressions](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-12#collection-expressions) | Yes
12 | [Interceptors](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-12#interceptors) | No
11 | [Raw string literals](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-11#raw-string-literals) | Yes
11 | [`static abstract/static virtual` members in interfaces](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-11#generic-math-support) | No
11 | [Checked user defined operators](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-11#generic-math-support) | Yes
11 | [Relaxed shift operators](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-11#generic-math-support) | Yes
11 | [Unsigned right-shift operator](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-11#generic-math-support) | Yes
11 | [Generic attributes](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-11#generic-attributes) | Crash
11 | [UTF-8 string literals](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-11#utf-8-string-literals) | Yes
11 | [Newlines in string interpolations](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-11#newlines-in-string-interpolations) | Yes
11 | [List patterns](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-11#list-patterns) | Yes
11 | [File-local types](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-11#file-local-types) | Yes
11 | [Required members](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-11#required-members) | PolySharp
11 | [Auto-default structs](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-11#auto-default-struct) | Yes
11 | [Pattern match `Span<char>` or `ReadOnlySpan<char>` on a constant `string`](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-11#pattern-match-spanchar-or-readonlyspanchar-on-a-constant-string) | Yes
11 | [Extended nameof scope](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-11#extended-nameof-scope) | Yes
11 | [Numeric `IntPtr` and `UIntPtr`](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-11#numeric-intptr-and-uintptr) | Yes
11 | [`ref` fields](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-11#ref-fields-and-ref-scoped-variables) | No
11 | [`ref scoped` variables](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-11#ref-fields-and-ref-scoped-variables) | PolySharp
11 | [Improved method group conversion to delegate](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-11#improved-method-group-conversion-to-delegate) | Yes
10 | [Record structs](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-10#record-structs) | Yes
10 | [Improvements of structure types](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-10#improvements-of-structure-types) | Yes
10 | [Interpolated string handler](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-10#interpolated-string-handler) | PolySharp
10 | [Global using directives](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-10#global-using-directives) | Yes
10 | [File-scoped namespace declaration](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-10#file-scoped-namespace-declaration) | Yes
10 | [Extended property patterns](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-10#extended-property-patterns) | Yes
10 | [Lambda expression improvements](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-10#lambda-expression-improvements) | Yes
10 | [Constant interpolated strings](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-10#constant-interpolated-strings) | Yes
10 | [Record types can seal ToString](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-10#record-types-can-seal-tostring) | Yes
10 | [Assignment and declaration in same deconstruction](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-10#assignment-and-declaration-in-same-deconstruction) | Yes
10 | [Improved definite assignment](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-10#improved-definite-assignment) | Yes
10 | [CallerArgumentExpression attribute](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-10#callerargumentexpression-attribute-diagnostics) | PolySharp
10 | [Enhanced #line pragma](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-10#enhanced-line-pragma) | Yes

**Support:**
* **Yes** - feature works exactly as expected.
* **No** - requires runtime features or BCL changes that Unity does not have. Attempting to use the feature may result in compiler errors.
* **Crash** - requires runtime features that Unity does not have. Attempting to use the feature may compile, but will result in crashes.
* **PolySharp** - feature works when using [PolySharp](https://github.com/Sergio0694/PolySharp) and/or manually implementing missing APIs.

Motivation
==========
This repository is based on this awesome repos, which was the main motivation for my project:
1. [CsprojModifier](https://github.com/Cysharp/CsprojModifier)
2. [UnityRoslynUpdater](https://github.com/DaZombieKiller/UnityRoslynUpdater)

So the problem of [UnityRoslynUpdater](https://github.com/DaZombieKiller/UnityRoslynUpdater) is that it increases C# version for everything. So all the code in your project will be compiled with newer C# version.
This is fine for a small hobby project, but for medium to big production projects, where a lot of libraries / packages / sdks are used, it can become a problem, because they may use naming that has been reserved in a newer C# versions etc.

I've adjusted / extended it with [CsprojModifier](https://github.com/Cysharp/CsprojModifier)'s help to support custom C# version per ``.asmdef/.csproj`` file. So only your code will have increased C# version.

