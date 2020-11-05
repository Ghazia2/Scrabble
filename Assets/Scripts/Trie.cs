using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trie
{
    static readonly int ALPHABET_SIZE = 26;

    #region NodeClass
    private class TrieNode
    {
        public TrieNode[] children = new TrieNode[ALPHABET_SIZE];
        public bool isEndOfWord;

        public TrieNode() // Constructor
        { 
            isEndOfWord = false;
            for (int i = 0; i < ALPHABET_SIZE; i++)
                children[i] = null;
        }
    }
    #endregion

    static TrieNode rootNode = new TrieNode();

    public void Insert(string wordToBeParsed)
    {
        TrieNode current = rootNode;
        int index;

        for (int i = 0; i < wordToBeParsed.Length; i++)
        {
            // ASCII A = 101
            index = wordToBeParsed[i] - 'A';
            if(current.children[index] == null)
                current.children[index] = new TrieNode();

            current = current.children[index];
        }
        current.isEndOfWord = true;
    }

    public bool Search(string wordToBeValidated)
    {
        TrieNode current = rootNode;
        int index;

        for (int i = 0; i < wordToBeValidated.Length; i++)
        {
            index = wordToBeValidated[i] - 'A';
            if (current.children[index] == null)
                return false;

            current = current.children[index];
        }
        return (current != null && current.isEndOfWord);
    }
}
