
module TestBuilder
type PlantDb =
    { MandateId: int
      Name: string
      Kst: string
      MacAddress: string
      PiStaticIP: string
      LocationPlantCombo: string
      HasMapping: bool
      SPSPlantName: string
      PlcAddress: string
      SPSGroup: string
      DataBlock: string
      Rack: int option
      Slot: float option
      Port: string }

let plant =[
    { MandateId = 1
      Name =""
      Kst =""
      MacAddress =""
      PiStaticIP =""
      LocationPlantCombo =""
      HasMapping = true
      SPSPlantName =""
      PlcAddress =""
      SPSGroup =""
      DataBlock =""
      Rack = None
      Slot = None
      Port ="" }

]
open Fumble.InsertBuilder
open Fumble.SqliteDeconstructor
let test  =
    insert<PlantDb> {
        table "Plant"
}

printfn "%s" (test |> Deconstructor.insert |> fst)
// printfn "%A" (test |> Deconstructor.create)
