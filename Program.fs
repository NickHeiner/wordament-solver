// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.
module Program

[<EntryPoint>]
let main argv = 
    let dict = 
        System.IO.File.ReadLines argv.[1] 
        |> (Seq.map (fun str -> [for char in str -> char]))
        |> Trie.addAll Trie.empty 

    0