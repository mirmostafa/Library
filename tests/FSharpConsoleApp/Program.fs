module Program

open System

let a (b) =
    try
        let e =  5 / 0
        Console.WriteLine e
        Some(e)
        with
        | ex ->
            Console.WriteLine ex
            None