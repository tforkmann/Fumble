
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
      Rack: string
      Slot: string
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
      Rack =""
      Slot =""
      Port ="" }

]
open Fumble.InsertBuilder
open Fumble.SqliteDeconstructor
let test =
    insert {
        table "Plant"
        values plant
}

printfn "%s" (test |> Deconstructor.insert |> fst)
