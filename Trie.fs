(* Simple trie to store strings by Nick Heiner <nth23@cornell.edu> 
   Adapted from https://gist.github.com/2231228
*)

module Trie

(* true if a member ends at this node * mapping from next char => children *)
type trie<'a when 'a : comparison> = Node of bool * Map<'a, trie<'a>>    
    
let empty = Node (false, Map.empty)

(* Add a string to the trie *)
let rec add items (Node (isValid, children)) = 
    match items with
      | [] -> Node (true, children)
      | hd::tl -> 
        let newTrie = if Map.containsKey hd children
                      then (Map.find hd children) 
                      else empty
                      |> add tl 
        let newChildren = Map.add hd newTrie children
        Node (isValid, newChildren)
    
let add' trie items = add items trie

let addAll trie seqs = Seq.fold add' trie seqs

let addAll' seqs trie = addAll trie seqs

(* This is not quite right... *)

let rec isValidSeq (Node (isValid, children)) = function
    | [] -> isValid
    | hd::tl -> if Map.containsKey hd children
                then isValidSeq (Map.find hd children) tl
                else false

let isValidSeq' items trie = isValidSeq trie items

let rec isValidPrefix (Node (isValid, children)) = function
    | [] -> true
    | hd::tl -> if Map.containsKey hd children
                then isValidPrefix (Map.find hd children) tl
                else false

let isValidPrefix' items trie = isValidPrefix trie items

let rec count (Node (isValid, children)) = 
    (if isValid then 1 else 0)
        +
    (children
    |> Map.toList
    |> List.map (snd >> count)
    |> List.fold (+) 0)