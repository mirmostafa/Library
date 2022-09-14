System.Console.WriteLine "Hello World"

let five = 5

System.Console.WriteLine five

let write = printfn "Hello %s"
write "Mohammad"

let names = ["Mohammad"; "Sara"; "Mina"]
names |> List.iter write

