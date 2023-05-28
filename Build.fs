open System
open System.IO
open Fake.Core
open Fake.DotNet
open Fake.Core.TargetOperators
open Fake.IO
open Farmer
open Farmer.Builders
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Tools
open Helpers
initializeContext()

//-----------------------------------------------
// Information about the project to be used at NuGet and in AssemblyInfo files
// --------------------------------------------------------------------------------------

let deployDir = Path.getFullName "./deploy"
let release = ReleaseNotes.load "RELEASE_NOTES.md"
let unitTestsPath = Path.getFullName "./tests/Fumble.Tests/"

let buildDir  = "./build/"

// Git configuration (used for publishing documentation in gh-pages branch)
// The profile where the project is posted
let gitHome = "https://github.com/tforkmann"
// The name of the project on GitHub
let gitName = "Fumble"

// The name of the project
// (used by attributes in AssemblyInfo, name of a NuGet package and directory in 'src')
let project = "Fumble"

let projectUrl = sprintf "%s/%s" gitHome gitName

// Short summary of the project
// (used as description in AssemblyInfo and as a short summary for NuGet package)
let summary = "Thin F# API for Fumble"

let copyright = "Copyright \169 2021"
let iconUrl = "https://raw.githubusercontent.com/tforkmann/Fumble/main/Fumble_logo.png"
let licenceUrl = "https://github.com/tforkmann/Fumble/blob/main/LICENSE.md"
let configuration = DotNet.BuildConfiguration.Release

// Longer description of the project
// (used as a description for NuGet package; line breaks are automatically cleaned up)
let description = """Thin F# API for Sqlite for easy data access to sqlite database with functional seasoning on top."""
// List of author names (for NuGet package)
let authors = [ "Tim Forkmann"]
let owner = "Tim Forkmann"
// Tags for your project (for NuGet package)
let tags = "Thin F# API for Sqlite for easy data access to sqlite database with functional seasoning on top"

// --------------------------------------------------------------------------------------
// PlatformTools
// --------------------------------------------------------------------------------------
let platformTool tool winTool =
    let tool = if Environment.isUnix then tool else winTool
    match ProcessUtils.tryFindFileOnPath tool with
    | Some t -> t
    | _ ->
        let errorMsg =
            tool + " was not found in path. " +
            "Please install it and make sure it's available from your path. " +
            "See https://safe-stack.github.io/docs/quickstart/#install-pre-requisites for more info"
        failwith errorMsg

let nodeTool = platformTool "node" "node.exe"
let yarnTool = platformTool "yarn" "yarn.cmd"
let npmTool = platformTool "npm" "npm.cmd"

// --------------------------------------------------------------------------------------
// Standard DotNet Build Steps
// --------------------------------------------------------------------------------------
let install = lazy DotNet.install DotNet.Versions.FromGlobalJson
let inline withWorkDir wd =
    DotNet.Options.lift install.Value
    >> DotNet.Options.withWorkingDirectory wd

let runTool cmd args workingDir =
    let arguments = args |> String.split ' ' |> Arguments.OfArgs
    RawCommand (cmd, arguments)
    |> CreateProcess.fromCommand
    |> CreateProcess.withWorkingDirectory workingDir
    |> CreateProcess.ensureExitCode
    |> Proc.run
    |> ignore

let runDotNet cmd workingDir =
    let result =
        DotNet.exec (DotNet.Options.withWorkingDirectory workingDir) cmd ""
    if result.ExitCode <> 0 then failwithf "'dotnet %s' failed in %s" cmd workingDir
// --------------------------------------------------------------------------------------
// Clean Build Results
// --------------------------------------------------------------------------------------

Target.create "Clean" (fun _ ->
    !!"src/**/bin"
    |> Shell.cleanDirs
    !! "src/**/obj/*.nuspec"
    |> Shell.cleanDirs

    Shell.cleanDirs [buildDir; "temp"; "docs/output"; deployDir;]
)

Target.create
    "UpdateTools"
    (fun _ ->
        run dotnet "tool update fantomas-tool" __SOURCE_DIRECTORY__
        run dotnet "tool update fake-cli" __SOURCE_DIRECTORY__
        run dotnet "tool update paket" __SOURCE_DIRECTORY__

        )

open System.Text.RegularExpressions
module Util =

    let visitFile (visitor: string->string) (fileName: string) =
        File.ReadAllLines(fileName)
        |> Array.map (visitor)
        |> fun lines -> File.WriteAllLines(fileName, lines)

    let replaceLines (replacer: string->Match->string option) (reg: Regex) (fileName: string) =
        fileName |> visitFile (fun line ->
            let m = reg.Match(line)
            if not m.Success
            then line
            else
                match replacer line m with
                | None -> line
                | Some newLine -> newLine)

// --------------------------------------------------------------------------------------
// Build a NuGet package

Target.create "Build" (fun _ ->
    !! "src/**/*.fsproj"
    |> Seq.filter (fun s ->
        let name = Path.GetDirectoryName s
        not (name.Contains "docs"))
    |> Seq.iter (fun s ->
        let dir = Path.GetDirectoryName s
        DotNet.build id dir)
)

Target.create "UnitTests" (fun _ ->
    runDotNet "run" unitTestsPath
)

Target.create "PrepareRelease" (fun _ ->
    Git.Branches.checkout "" false "main"
    Git.CommandHelper.directRunGitCommand "" "fetch origin" |> ignore
    Git.CommandHelper.directRunGitCommand "" "fetch origin --tags" |> ignore

    Git.Staging.stageAll ""
    Git.Commit.exec "" (sprintf "Bumping version to %O" release.NugetVersion)
    Git.Branches.pushBranch "" "origin" "main"

    let tagName = string release.NugetVersion
    Git.Branches.tag "" tagName
    Git.Branches.pushTag "" "origin" tagName
)

Target.create "Pack" (fun _ ->
    let nugetVersion = release.NugetVersion

    let pack project =
        let projectPath = sprintf "src/%s/%s.fsproj" project project
        let args =
            let defaultArgs = MSBuild.CliArguments.Create()
            { defaultArgs with
                      Properties = [
                          "Title", project
                          "PackageVersion", nugetVersion
                          "Authors", (String.Join(" ", authors))
                          "Owners", owner
                          "PackageRequireLicenseAcceptance", "false"
                          "Description", description
                          "Summary", summary
                          "PackageReleaseNotes", ((String.toLines release.Notes).Replace(",",""))
                          "Copyright", copyright
                          "PackageTags", tags
                          "PackageProjectUrl", projectUrl
                          "PackageIconUrl", iconUrl
                          "PackageLicenseUrl", licenceUrl
                      ] }

        DotNet.pack (fun p ->
            { p with
                  NoBuild = false
                  Configuration = configuration
                  OutputPath = Some "build"
                  MSBuildParams = args
              }) projectPath

    pack "Fumble"
)

let getBuildParam = Environment.environVar
let isNullOrWhiteSpace = String.IsNullOrWhiteSpace

// Workaround for https://github.com/fsharp/FAKE/issues/2242
let pushPackage _ =
    let nugetCmd fileName key = sprintf "nuget push %s -k %s -s nuget.org" fileName key
    let key =
        //Environment.environVarOrFail "nugetKey"
        match getBuildParam "nugetkey" with
        | s when not (isNullOrWhiteSpace s) -> s
        | _ -> UserInput.getUserPassword "NuGet Key: "
    IO.Directory.GetFiles(buildDir, "*.nupkg", SearchOption.TopDirectoryOnly)
    |> Seq.map Path.GetFileName
    |> Seq.iter (fun fileName ->
        Trace.tracef "fileName %s" fileName
        let cmd = nugetCmd fileName key
        runDotNet cmd buildDir)
Target.create "Push" (fun _ -> pushPackage [] )

let docsSrcPath = Path.getFullName "./src/docs"
let docsDeployPath = "docs"

Target.create "InstallDocs" (fun _ ->

    runTool yarnTool "install --frozen-lockfile" docsSrcPath
    runDotNet "restore" docsSrcPath )

Target.create "PublishDocs" (fun _ ->
    let docsDeployLocalPath = (docsSrcPath </> "deploy")
    [ docsDeployPath; docsDeployLocalPath] |> Shell.cleanDirs
    runTool yarnTool"webpack-cli -p" docsSrcPath
    Shell.copyDir docsDeployPath docsDeployLocalPath FileFilter.allFiles
)


Target.create "RunDocs" (fun _ -> runTool npmTool "webpack-dev-server" docsSrcPath)

Target.runOrDefault "Build"


let dependencies = [

    "Clean"
        // ==> "UpdateTools"
        ==> "UnitTests"

    "Clean"
        // ==> "UpdateTools"
        ==> "Build"
        ==> "UnitTests"
        ==> "PrepareRelease"
        ==> "Pack"
        ==> "Push"

    "InstallDocs"
        ==> "RunDocs"

    "InstallDocs"
        ==> "PublishDocs"
]

[<EntryPoint>]
let main args =
    try
        match args with
        | [| target |] -> Target.runOrDefault target
        | _ -> Target.runOrDefault "Run"
        0
    with e ->
        printfn "%A" e
        1
