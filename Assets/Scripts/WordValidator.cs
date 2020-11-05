using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class WordValidator
{
    PieceManager pieceManager;
    Trie trie = new Trie();
    List<string> validWords;

    public void Setup()
    {
        LoadWords();
    }

    private void LoadWords()
    {
        validWords = new List<string>();

        string path = (Application.dataPath + "/StreamingAssets/valid_words.txt");
        foreach (string word in File.ReadAllLines(path))
            validWords.Add(word);
        LoadLoadedWordsIntoTrie(validWords);
    }

    private void LoadLoadedWordsIntoTrie(List<string> listOfValidWords)
    {
        foreach (string word in validWords)
            trie.Insert(word);
    }
}
