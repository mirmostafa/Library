open System
//System.Console.WriteLine "Hello World"

//let five = 5

//System.Console.WriteLine five

//let write = printfn "Hello %s"
//write "Mohammad"

//let names = ["Mohammad"; "Sara"; "Mina"]
//names |> List.iter write


//let rec factorial x: int = 
//    if x <= 1 then 1
//    else x * factorial (x - 1)

//Console.WriteLine(factorial 50)

//let add_2 x = x + 2
//let mul_2 x = x * 2
//let add_mul_2 = mul_2 << add_2
//let mul_add_2 = mul_2 >> add_2
//Console.WriteLine (add_mul_2 3)
//Console.WriteLine (mul_add_2 3)
//let r1 = (add_mul_2 3)
//printfn "%i" r1
//printfn "%i" (mul_add_2 3)

//let upper s = String.collect (fun c -> sprintf "%c, " c) s
//printf  "%s" (upper "Hello")

let o = String.init 10
Console.WriteLine o

Console.ReadKey |> ignore