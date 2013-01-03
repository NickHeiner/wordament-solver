﻿(* Simple trie to store strings by Nick Heiner <nth23@cornell.edu> 
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

let addAll trie seqs = List.fold add' empty seqs