using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LetterGenerator
{
    public List<char> Letters;

    public char ReturnLetter()
    {
        char randomAlphabet;
        int index;
        if (Letters == null) 
        { 
            Letters = new List<char>();
            for (char c = 'A'; c <= 'Z'; c++)
            {
                for (int x = 0; x < LetterCount(c); x++)
                {
                    this.Letters.Add(c);
                }
            }
            ShuffleAlphabetList(Letters);
        }
        index = Random.Range(0, Letters.Count);
        randomAlphabet = Letters[index];
        Letters.RemoveAt(index);
        return randomAlphabet;
    }

    int LetterCount(char c)
    {
        Dictionary<char,int> LetterWithCount = new Dictionary<char, int>()
            {
                { 'E', 13 }, { 'A', 9 }, { 'I', 8 }, { 'O', 8 },
                { 'N', 5 }, { 'R', 6 }, { 'T', 7 }, { 'L', 4  },
                { 'S', 5 }, { 'U', 4 }, { 'D', 5 }, { 'G', 3 },
                { 'B', 2 }, { 'C', 2 }, { 'M', 2 }, { 'P', 2 },
                { 'F', 2 }, { 'H', 3 }, { 'V', 2 }, { 'W', 2 },
                { 'Y', 2 }, { 'K', 1 }, { 'J', 1 }, { 'X', 1 },
                { 'Q', 1 }, { 'Z', 1 },
            };

        return LetterWithCount[c];
    }

    void ShuffleAlphabetList(List<char> letterList)
    {
        for (int i = 0; i < letterList.Count; i++)
        {
            char temp = letterList[i];
            int randomIndex = Random.Range(i, letterList.Count);
            letterList[i] = letterList[randomIndex];
            letterList[randomIndex] = temp;
        }
    }
}
