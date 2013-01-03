
module Program

open System.IO

[<EntryPoint>]
let main argv = 
    let dict = 
        File.ReadLines argv.[0] 
        |> (Seq.map (fun str -> str.ToCharArray () |> Array.toList))
        |> Trie.addAll Trie.empty 

    printf "Read dictionary with %A entries" <| Trie.count dict

    ignore <| System.Console.ReadLine ()

    0