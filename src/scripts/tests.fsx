let inline tryUnbox<'a> (x:obj) =
        match x with
        | :? 'a as result -> Some (result)
        | :? option<'a> as result when result.IsSome -> result
        | :? option<'a> as result when result.IsNone -> None
        | _ ->
            None
let cast = Some "Error" |> tryUnbox<string>
let cast2 = None |> tryUnbox<string>
