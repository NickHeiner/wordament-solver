
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

let neighborsOf grid row col =
    let findBound getCoordMember search = 
        grid 
        |> Map.toSeq
        |> Seq.map fst 
        |> Seq.map getCoordMember 
        |> search
    let maxRow = findBound fst Seq.max
    let minRow = findBound fst Seq.min
    let maxCol = findBound snd Seq.max
    let minCol = findBound snd Seq.min

    seq {for rowOffset in -1 .. 1 do
            for colOffset in -1 .. 1 do
                yield row + rowOffset, col + colOffset}
    |> Seq.filter (fun (row, col) -> minRow <= row && row <= maxRow && minCol <= col && col <= maxCol)

let wordsStartingFrom grid isValidWord isValidPrefix coord = 
    let rec wordsStartingFromWith soFar (row, col) =
        seq {
            let soFarSet = Set.ofSeq soFar
            let currPrefix = (List.map (fun coords -> Map.find coords grid) soFar)
            if isValidWord currPrefix 
            then yield currPrefix
            if isValidPrefix currPrefix
            then yield! neighborsOf grid row col 
                |> Seq.filter (fun coord -> not <| Set.contains coord soFarSet)
                |> Seq.map (wordsStartingFromWith ((row, col)::soFar)) 
                |> Seq.concat
        }
    wordsStartingFromWith [] coord
    |> Set.ofSeq
    |> Set.toSeq

let findWords isValidWord isValidPrefix grid = 
    grid
    |> Map.toSeq
    |> Seq.map (fst >> (wordsStartingFrom grid isValidWord isValidPrefix))
    |> Seq.concat

// from http://fssnip.net/5u
let implode (xs:char list) =    
    let sb = System.Text.StringBuilder(xs.Length)
    xs |> List.iter (sb.Append >> ignore)
    sb.ToString()

[<EntryPoint>]
let main argv = 

    let dict = readDict argv.[0]
    printf "Read dictionary with %d entries" <| Trie.count dict
  
    let grid = readGrid argv.[1]
    printfn "Read grid: "
    Map.iter (fun (row, col) char -> printfn "(%d, %d): %c" row col char) grid

    let words = 
        findWords (Trie.isValidSeq dict) (Trie.isValidPrefix dict) grid
        |> Seq.map implode
    printfn "Found %d words: " <| Seq.length words

    ignore <| System.Console.ReadLine ()

    Seq.iter (printfn "%s") words

    ignore <| System.Console.ReadLine ()

    0