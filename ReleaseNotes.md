Q. Never mind the fluff -- how do I get started?

A. Start with the Quick Start guide : https://github.com/SteveGilham/altcover/wiki/QuickStart-Guide

# 5.1.6xx (Ezoguma series release 4)
* `--dropReturnCode` ( `-DropReturnCode` in PowerShell, `ExposeReturnCode` with default value `true` in the Fake API) to not pass the return code of any nested process
* `--sourcelink` to give the source link URL for tracked files rather than the file path (untracked files still have the local file path)
* Visualizer support for sourcelink URLs

# 5.0.665 (Ezoguma series release 3)
* [BUGFIX] Restore visualizer support for OpenCover format (internal consistency check failure)
* [BUGFIX] Issue #52 -- fix OpenCover, LCov and Cobertura format output in the case of exclusion by path
* [BUGFIX] Issue #50 -- take Cecil 0.10.3 with the actual fix in it
* Some improvements to the throughput of coverage data, reducing time taken by the instrumented self-tests.

# 5.0.664 (Ezoguma series release 2)
* [BUGFIX] Issue #49 -- `dotnet test` integration : internally, escape the '\' character, which is  is helpfully treated by MSBuild as a path separator and flipped to be '/' on non-Windows platforms when introduced through `/p:AltCover*Filter` arguments.
* [BUGFIX] Issue #48 -- fix embedded-PDB detection to avoid false positives
* Updating consumed libraries and related changes.

# 5.0.663 (Ezoguma series release 1)
Bringing gifts, as is appropriate for the season
* [BUGFIX] Issue #46 -- handle the case of a left-behind `__Saved` directories by failing in a more obvious fashion (and offering a `/p:AltCoverForce=true` option to force-delete such a directory)
* Support instrumenting assemblies with embedded PDBs
  * [BREAKING] the `XUnit` assemblies have embedded PDBs, so will suddenly be caught up in instrumentation without a `-e xunit` or equivalent to exclude them
* [BREAKING] Complete API overhaul to properly address known problems and to try to future-proof everything against any similar issues -- see the Wiki [here for in-process execution](https://github.com/SteveGilham/altcover/wiki/The-AltCover-API,-plus-Fake-and-Cake-integration) and [here for FAKE scripting](https://github.com/SteveGilham/altcover/wiki/The-AltCover.Fake-package)

# 4.0.661 (Doruka series release 11)
* [BUGFIX] More forms of Issue #43 related to `yield return` synthetic methods.
* [BUGFIX] Issue #45 by re-working the static linkage of the recorder assembly using `ILMerge /internalise` (rather than `--standalone`).
* Updating consumed libraries and related changes.

# 4.0.660 (Doruka series release 10)
* [BUGFIX] Issue #43 Detect and skip simple recursive references when looking from a synthetic method to its containing method
* [Enhancement] Follow the Fake build system to use [`BlackFox.CommandLine`](https://github.com/vbfox/FoxSharp/tree/master/src/BlackFox.CommandLine) facilities to compose command lines with proper re-escaping/enquoting of items read from the command line following a `--`
* [BUGFIX] [API] A single string for a command line as in `AltCover.PrepareParams.CommandLine` would not work for anything more than a parameterless invocation.
  * Add new field `Command` to the F# & C# parameter APIs and to the MSBuild task taking an array or sequence of strings
  * Deprecate the `CommandLine` field (at compile time for C# API and MSBuild; warning at runtime for F#, because it's not possible to usefully mark a record field as [<Obsolete>])
  * In the cases where the deprecated `CommandLine` field is used (non-empty with the preferred `.Command` field being empty), use the operating-system specific facilities in `BlackFox.CommandLine` to decompose the string so as to separate out the executable from its arguments)
  * Expect a thorough and breaking rework in the next major version (5.0) release
* [BUGFIX] If an error is raised during instrumentation, then a message is logged saying that the exception details have been written to a file in the nominated output directory (for `--inplace` operations, this is where the unaltered binaries are saved off to).  If this happens during `dotnet test`, the tidy-up action actually moves everything from that directory back into the original location and deletes the output directory, moving the log file too.  If the operation could be `dotnet test`, then amend the message appropriately to say that the file may have been moved.
* Minor improvements to error handling in the Visualizer
* Minor improvements to the fix for issue #41

# 4.0.659 (Doruka series release 9)
* [BUGFIX] Issue #42 Remove the need for process-exit handling to rely on non-event-handler threads being scheduled, so allowing the coverage data to be flushed to disk, even on low-spec platforms like Raspberry Pi
* [BUGFIX] Issue #41 Reduce memory use in processing coverage data (runner mode/`dotnet test` scenarios)
* [BUGFIX] Exclusion by attribute now works for property-level attributes, and will exclude the getter and/or setter (if not already excluded)
* Various build process improvements/updates

# 4.0.655 (Doruka series release 8)
* [NEW PACKAGE] `altcover.fake` containing just helper types for FAKE scripts (v5.9.3 or later) : see Wiki entry [The `AltCover.Fake` package](https://github.com/SteveGilham/altcover/wiki/The-AltCover.Fake-package)
  * Module `AltCover_Fake.DotNet.Testing.AltCover` containing an `AltCover` task for driving any command-line version of AltCover, along with helper types
  * Module `AltCover_Fake.DotNet.DotNet` fluent extension methods for `Fake.DotNet.DotNet.TestOptions` using the `AltCover_Fake.DotNet...` helper types
  * dog-food these features in the AltCover build process
* [API] -- `AltCover.Api.CollectParams`
  * Make `[<NoComparison>]` explicit
  * Mark the `Default` member `[<Obsolete>]`; prefer new `Create()` instead
* [API] -- `AltCover.Api.PrepareParams`
  * Make `[<NoComparison>]` explicit
  * All `string array` members have been relaxed to become `string seq`
  * Mark the `Default` member `[<Obsolete>]`; prefer new `Create()` instead
* [API] -- `AltCover.Fake.Api`
  * `static member toolPath` for finding the location of an AltCover command-line executable in the same nuget package
* [API] -- `AltCover.Fake.DotNet`
  * Extend module with more `Fake.DotNet.DotNet.TestOptions` fluent extension methods `WithImportModule()` and `WithGetVersion()`

# 4.0.653 (Doruka series release 7)
* Properly resolve the strong-naming of the recorder assembly (a fix for Issue #38 closer to the original intent, compared with the "just-make-it-work" fix in 4.0.649)
* Look for dependencies -- mostly ASP .net Core assemblies -- under `%ProgramFiles%\dotnet\shared` or `/usr/share/dotnet/shared` as well as under `%USERPROFILE%/.nuget/packages`
* Look for dependencies under `%NUGET_PACKAGES%`
* Extend the `-d` argument parser to expand environment variables and to handle relative paths

# 4.0.649 (Doruka series release 6)
* [BUGFIX] Issue #37 -- handle release builds of C# `return <ternary>;` expressions which don't look like ternaries in their IL.  Fix some corner cases of NPath complexity, branch exit counts and branch visit counts revealed by these cases.
**NOTE** This form of ternary expression is the first case of a significant user defined branch within a sequence point; the `Compress-Branching -WithinSequencePoint` cmdlet at this release _will_ flatten these constructs.
* [BUGFIX] Accept coverlet's idiosyncratic OpenCover-style output into cmdlet and Visualizer operations -- it uses Boolean.ToString() at points, which generates capitalized `True` and `False` attribute values, which are not valid XSD `boolean`s
* [BUGFIX] Issue #38 -- in some .net framework cases, the strong-naming of the recorder assembly was broken; an approach that testing shows to be more reliable has been used.
* [API] Update to FAKE 5.8.5 and validate support from 5.0 up to current

# 4.0.644 (Doruka series release 5)
* [API] - FAKE 5.7.2 support -- FAKE integration now spans from v5.0 up to current (and should be reasonably future-proofed), and is now also offered on an experimental basis for the .net framework
* various refactorings and rearrangements, some on purely aesthetic grounds, some to improve the reliability of the travis-ci build, but with no functional impact

# 4.0.630 (Doruka series release 4)
* Add SourceLink to the build process (and the .nuspec)
* [BUGFIX] Work around an apparent change in behaviour in mono 5.14 that loses some recorded coverage in runner mode; the problem does not affect .net framework or .net core, but does impact those travis build self-tests that aren't on .net core
* [BUGFIX] Fix localization packaging in .netcore 2.1.4xx
* [BUGFIX] Source scrolling in the global tool version of the visualizer
* [API] Expose all cmdlet functionality as APIs and run the PowerShell cmdlets through the public API
* [Visualizer] add the XML document as a root of the tree view, and the current source file (if any) to the application title bar; clear source pane when new coverage file is selected.

# 4.0.618 (Doruka series release 3)
* [NEW PACKAGE] `altcover.visualizer` containing .net core/GTK3 Visualizer global tool implementation
  * .net core/GTK3 Visualizer also contained in the general-purpose and the API packages for direct `dotnet path/to/AltCover.Visualizer.dll` use
  * needs GTK+3 installed separately -- for Windows, see e.g. https://github.com/GtkSharp/GtkSharp/wiki/Installing-Gtk-on-Windows
* Improved error messages for the GTK# Visualizer
* Improved font handling for the GTK# Visualizer (now it updates immediately)
* [API] strong-name keys can now be meaningfully validated from .net core
* Minor improvements to reliability on mono

# 4.0.603 (Doruka series release 2)
* [BUGFIX] PowerShell and GetVersion tasks might produce empty output in 4.0.600 : now fixed
* [BUGFIX] pack the pwsh support into the API module (omission in 4.0.600)
* [API] Defaults provided for CSApi types `CollectArgs` and `PrepareArgs` equivalent to the F# defaults
* [API] `PrepareParams.Vaildate : unit -> string array`; and `CollectParams.Validate : bool -> string array` to do read-only parameter validation
* [API] The equivalent `public string[] PrepareArgs()` and `public string[] CollectArgs(bool afterPreparation)` for the CSApi types

# 4.0.600 (Doruka series release 1)
* [NEW PACKAGE] AltCover.Api exposing the shared API used by both the MSBuild tasks and the PowerShell `Invoke-AltCover` cmdlet, in native F# and with a C#-friendly adapter layer
  * Also included, integrations with Fake ( >= 5.2.0) and Cake ( >= 0.28.0), each in their separate assembly, to avoid any need to drag in unwanted extra dependencies
  * The PowerShell module and the MSBuild tasks
  * And the `dotnet test` integration
* [ALL PACKAGES] `Compress-Branching` cmdlet to transform OpenCover's liberal idea of significant branches towards AltCover's more restricted approach -- chose either or both of `-SameSpan` to unify branches that go from the same start, and take the same trajectory to the same end (OpenCover issue #786 being one instance of this) and `-WithinSequencePoint` to remove branches interior to a statement (compiler generated things like stashing of lambdas, the hidden conditional `Dispose()` after a `using`, or inside F# inlines -- OpenCover issues #657, #807 being instances of this)
* [BUGFIX] Issue # 31 -- In the case of `dotnet test` with multiple target frameworks make the coverage file `name.extension` go to `name.framework.extension`, be it supplied or be it defaulted.

For previous releases (3.5.x and earlier) [go here](https://github.com/SteveGilham/altcover/blob/master/ReleaseNotes%20-%20Previously.md)