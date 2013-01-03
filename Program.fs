
module Program

open System.IO

(* For brevity, this makes all sorts of 
   (un-)documented assumptions about correctness of input *)

let readDict = 
    File.ReadLines
    >> (Seq.map (fun str -> str.ToCharArray () |> Array.toList))
    >> Trie.addAll Trie.empty 

let readGrid = 
    File.ReadLines
    >> Seq.mapi (fun row str -> str.Split [| '\t' |]
                                |> Array.toList
                                // Assume that the grid is well formatted and contains only a single char per \t
                                |> List.mapi (fun col char -> (row, col), char.Chars 0))
    >> Seq.concat
    >> Map.ofSeq

let findWords grid dict = []

[<EntryPoint>]
let main argv = 

    let dict = readDict argv.[0]
    printf "Read dictionary with %d entries" <| Trie.count dict
  
    let grid = readGrid argv.[1]
    printfn "Read grid: "
    Map.iter (fun (row, col) char -> printfn "(%d, %d): %c" row col char) grid

    let words = findWords grid dict
    printfn "Found %d words: " <| Seq.length words
    Seq.iter printfn words

    ignore <| System.Console.ReadLine ()

    0