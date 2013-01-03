
module Program

open System.IO

(* For brevity, this makes all sorts of 
   (un-)documented assumptions about correctness of input *)

[<EntryPoint>]
let main argv = 

    let dict = 
        File.ReadLines argv.[0] 
        |> (Seq.map (fun str -> str.ToCharArray () |> Array.toList))
        |> Trie.addAll Trie.empty 

    printf "Read dictionary with %d entries" <| Trie.count dict
  
    let grid = 
        File.ReadLines argv.[1]
        |> Seq.mapi (fun row str -> str.Split [| '\t' |]
                                    |> Array.toList
                                    // Assume that the grid is well formatted and contains only a single char per \t
                                    |> List.mapi (fun col char -> (row, col), char.Chars 0))
        |> Seq.concat
        |> Map.ofSeq

    printfn "Read grid: "
    Map.iter (fun (row, col) char -> printfn "(%d, %d): %c" row col char) grid

    ignore <| System.Console.ReadLine ()

    0