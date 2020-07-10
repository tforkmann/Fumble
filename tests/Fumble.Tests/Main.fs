module ExpectoTemplate
open Expecto


let config =
        { defaultConfig with
            runInParallel = false }
[<EntryPoint>]
let main argv =
    runTestsInAssembly config argv
