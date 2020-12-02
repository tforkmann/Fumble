
namespace Fumble

module GenericDeconstructor =
    open System
    type InsertQuery<'a> = {
        Schema : string option
        Table : string
        Values : 'a list
    }


    let private _insert evalInsertQuery (q:InsertQuery<_>) fields outputFields =
        let query : string = evalInsertQuery fields outputFields q
        let pars =
            q.Values
            |> List.map (Reflection.getValues >> List.zip fields)
            |> List.collect (fun values ->
                values |> List.map (fun (key,value) -> sprintf "%s" key))
            |> String.concat ", "
        query, pars
    let insert evalInsertQuery (q:InsertQuery<'a>) =
        let fields = typeof<'a> |> Reflection.getFields
        _insert evalInsertQuery q fields []
    let private _create evalCreateQuery (q:InsertQuery<_>) fields (properties:Type list) outputFields =
        let query : string = evalCreateQuery fields outputFields q
        let pars =
            q.Values
            |> List.map (Reflection.getValues >> List.zip fields)
            |> List.collect (fun values ->
                values |> List.mapi (fun i (key,value) ->
                    let property = properties.[i].ToString()
                    sprintf "%s %s" key property))
            |> String.concat ", "
        query, pars
    let create evalCreateQuery (q:InsertQuery<'a>) =
        let properties = typeof<'a> |> Reflection.getPropertyTypes
        let fields = typeof<'a> |> Reflection.getFields
        printfn "properties %A" properties
        _create evalCreateQuery q fields properties []
