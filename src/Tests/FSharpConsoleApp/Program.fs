let hello = printfn "Hello %s"
hello "Mohammad"

let names = ["Mohammad"; "Sara"; "Mina"]
names |> List.iter hello