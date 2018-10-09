#r "paket:
nuget Fake.Core.Environment >= 5.7.2
nuget Fake.Core.Process >= 5.7.2
nuget Fake.DotNet.Cli >= 5.7.2
nuget Fake.DotNet.NuGet >= 5.7.2
nuget Fake.IO.FileSystem >= 5.7.2 //"

open System
open System.IO

open Fake.DotNet
open Fake.DotNet.NuGet.Restore
open Fake.IO

open Microsoft.Win32

// Really bootstrap
let dotnetPath = "dotnet" |> Fake.Core.Process.tryFindFileOnPath

let dotnetOptions (o : DotNet.Options) =
  match dotnetPath with
  | Some f -> { o with DotNetCliPath = f }
  | None -> o

DotNet.restore (fun o ->
  { o with Packages = [ "./packages" ]
           Common = dotnetOptions o.Common }) "./Build/dotnet-nuget.fsproj"
// Restore the NuGet packages used by the build and the Framework version
RestoreMSSolutionPackages id "./AltCover.sln"

let build = """// generated by dotnet fake run .\Build\setup.fsx
#r "paket:
nuget Fake.Core.Target >= 5.7.2
nuget Fake.Core.Environment >= 5.7.2
nuget Fake.Core.Process >= 5.7.2
nuget Fake.DotNet.AssemblyInfoFile >= 5.7.2
nuget Fake.DotNet.Cli >= 5.7.2
nuget Fake.DotNet.MSBuild >= 5.7.2
nuget Fake.DotNet.NuGet >= 5.7.2
nuget Fake.DotNet.Testing.NUnit >= 5.7.2
nuget Fake.DotNet.Testing.OpenCover >= 5.7.2
nuget Fake.DotNet.Testing.XUnit2 >= 5.7.2
nuget Fake.IO.FileSystem >= 5.7.2
nuget Fake.Testing.ReportGenerator >= 5.7.2
nuget coveralls.io >= 1.4.2
nuget FSharpLint.Core prerelease
nuget Markdown >= 2.2.1
nuget NUnit >= 3.11.0
nuget YamlDotNet >= 5.2.0 //"
#r "System.IO.Compression.FileSystem.dll"
#r "System.Xml"
#r "System.Xml.Linq"
#load "actions.fsx"
#load "targets.fsx"
#nowarn "988"

do ()"""

File.WriteAllText("./Build/build.fsx", build)