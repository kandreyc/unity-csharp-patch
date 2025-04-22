Unity C# Patch
================
[![release](https://github.com/kandreyc/unity-csharp-patch/actions/workflows/release.yml/badge.svg)](https://github.com/kandreyc/unity-csharp-patch/actions/workflows/release.yml)
![Static Badge](https://img.shields.io/badge/Unity-2022%2B-black.svg?style=flat&logo=unity&color=%231f1f1f)

Unleash the full potential by being in sync with the latest C# versions that are configured individually for each ``.asmdef``.

How it works:
-------------
1. **Editor Patching**: The `UnityEditorPatch` is responsible for updating the built-in dotnet SDK within the Unity editor. Unity, by default, ships with dotnet version `6.0.21`.

2. **Language Version Tracking**: `UnityPackage` keeps track of the C# language version specified in your `csc.rsp` file that is located alongside each `.asmdef`. This is required to help Unity to understand which C# version it should use while compiling the code of your `.asmdef`.

3. **Project File Adjustment**: Finally, `UnityPackage` modifies the `.csproj` file to reflect the C# version specified in the `csc.rsp`. This alerts your IDE to the correct language version to use, ensuring it can provide you with all the relevant language features.

Supported AssemblyDefinition locations:
--------
1. Assets folder
2. Embedded Packages - everything that is located in Packages/ folder of your project.
3. Local Packages - everything that is located anywhere on your pc with specified path with `file:` prefix in manifest.

How to Install:
---------------
> [!NOTE]
> OS support: Mac / Linux / Windows.
> You can backup your editor just in case, but should be fine.
>
> Patch will modify the Editor installation, so all the projects that are using it will be affected (default for unity C# version will be used for compilation, but from newer (patched) dotnet sdk)
> 
> Tested on target platforms: Mac / Windows / Linux / iOS / Android / WebGL 

1. Add the package via git url ``https://github.com/kandreyc/unity-csharp-patch.git#v1.3.0``
2. Ensure Unity Editor is closed.
3. Ensure latest dotnet sdk is installed. [Download Page](https://dotnet.microsoft.com/en-us/download)
4. Open terminal at folder ``EditorPatch~`` inside the added package.
5. Patch the editor (administrative privileges are required):
```
$ dotnet UnityEditorPatch.dll apply --editor '/Applications/Unity/Hub/Editor/2022.3.21f1' --allow-prerelease
```
where ``--editor`` - path to the unity editor

where ``--allow-prerelease`` - (optional) allows to use prerelease dotnet sdk

In case if you want to revert the patch:
```
$ dotnet UnityEditorPatch.dll revert --editor '/Applications/Unity/Hub/Editor/2022.3.21f1'
```
where ``--editor`` - path to the unity editor

6. Open the Unity Editor with your project and create a `csc.rsp` file alongside desired `.asmdef` with the following content:
```
-langVersion:13
-nullable:enable
```
where:

- ``langVersion`` (optional) - C# version you want to be used for this ``.asmdef``. Values are ``10``, ``11``, ``12``, ``13``, ``preview``
- ``nullable`` (optional) - allows to use nullables like ``string?`` without defining ``#nullable enable/disable`` in each file where it used. Values are ``enable``, ``disable``

7. Refresh the Editor. All required magic should be done here.
8. Enjoy!

Language Support
----------------
C# | Feature | Support
-|-|:-----:
preview | [`field` keyword](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-14#the-field-keyword) | Yes
preview | [`partial` events and instance constructors](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-14#more-partial-members) | Yes
preview | [`nameof` unbound generic types support for](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-14#unbound-generic-types-and-nameof) | Yes
preview | [Extension members](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-14#extension-members) | Yes
preview | [Null conditional assignment](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-14#null-conditional-assignment) | Yes
preview | [Simple lambda parameters](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-14#simple-lambda-parameters-with-modifiers) | Yes
preview | [Implicit span conversions](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-14#implicit-span-conversions) | Yes
13 | [`params` collections](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-13#params-collections) | Yes
13 | [New lock type and semantics](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-13#new-lock-object) | No
13 | [New escape sequence - \\e](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-13#new-escape-sequence) | Yes
13 | [Method group natural type improvements](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-13#method-group-natural-type) | Yes
13 | [Implicit indexer access in object initializers](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-13#implicit-index-access) | Yes
13 | [Enable ref locals and unsafe contexts in iterators and async methods](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-13#ref-and-unsafe-in-iterators-and-async-methods) | Yes
13 | [Enable ref struct types to implement interfaces](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-13#ref-struct-interfaces) | No
13 | [Allow ref struct types as arguments for type parameters in generics](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-13#allows-ref-struct) | No
13 | [Partial properties and indexers](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-13#more-partial-members) | Yes
13 | [Overload resolution priority](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-13#overload-resolution-priority) | PolySharp
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
This project was inspired and motivated by two key repositories:
1. [CsprojModifier](https://github.com/Cysharp/CsprojModifier)
2. [UnityRoslynUpdater](https://github.com/DaZombieKiller/UnityRoslynUpdater)

While the **UnityRoslynUpdater** serves its purpose by upgrading the C# version across all projects, my goal was to allow using custom C# version only where it is required.
So that my package gives the control of what assemblies should be allowed to use newer C# version, which helps to prevent naming conflicts with embedded/thirdparty libs/sdks, or affect the projects that you don't want to 

Inspired by **CsprojModifier**, I've automated modifying ``.csproj`` files based on ``.asmdef`` properties, making it possible for your IDE to use the newest C# features.
