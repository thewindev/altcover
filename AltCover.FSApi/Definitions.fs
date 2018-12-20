#if RUNNER
namespace AltCover
#else
module internal AltCover.Internals

#endif

open System
open System.Linq

[<assembly:CLSCompliant(true)>]
[<assembly:System.Runtime.InteropServices.ComVisible(false)>]
do ()

module DotNet =
  let private Arg name s = (sprintf """/p:AltCover%s="%s" """ name s).Trim()
  let private ListArg name (s : String seq) =
    (sprintf """/p:AltCover%s="%s" """ name <| String.Join("|", s)).Trim()

  let private IsSet s =
    s
    |> String.IsNullOrWhiteSpace
    |> not

  let private FromList name (s : String seq) = (ListArg name s, s.Any())
  let private FromArg name s = (Arg name s, IsSet s)
  let private Join(l : string list) = String.Join(" ", l)

#if RUNNER
  let ToTestArgumentList
      (prepare : AltCover.FSApi.PrepareParams)
      (collect : AltCover.FSApi.CollectParams) =
#else
  let ToTestArgumentList
      (prepare : AltCover_Fake.DotNet.Testing.AltCover.PrepareParams)
      (collect : AltCover_Fake.DotNet.Testing.AltCover.CollectParams) =
#endif

    [ FromArg String.Empty "true"
      FromArg "XmlReport" prepare.XmlReport
      (Arg "OpenCover" "false", not prepare.OpenCover)
      FromList "FileFilter" prepare.FileFilter
      FromList "PathFilter" prepare.PathFilter
      FromList "AssemblyFilter" prepare.AssemblyFilter
      FromList "AssemblyExcludeFilter" prepare.AssemblyExcludeFilter
      FromList "TypeFilter" prepare.TypeFilter
      FromList "MethodFilter" prepare.MethodFilter
      FromList "AttributeFilter" prepare.AttributeFilter
      FromList "CallContext" prepare.CallContext
      FromList "DependencyList" prepare.Dependencies
      FromArg "LcovReport" collect.LcovReport
      FromArg "Cobertura" collect.Cobertura
      FromArg "Threshold" collect.Threshold
      (Arg "LineCover" "true", prepare.LineCover)
      (Arg "BranchCover" "true", prepare.BranchCover) ]
    |> List.filter snd
    |> List.map fst

#if RUNNER
  let ToTestArguments
    (prepare : AltCover.FSApi.PrepareParams)
    (collect : AltCover.FSApi.CollectParams) =
#else
  let ToTestArguments
      (prepare : AltCover_Fake.DotNet.Testing.AltCover.PrepareParams)
      (collect : AltCover_Fake.DotNet.Testing.AltCover.CollectParams) =
#endif
    ToTestArgumentList prepare collect |> Join