using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordExtractor : MonoBehaviour
{
    private Vector2Int lastInputCoordinates = new Vector2Int(0,0);
    private WordValidator wordValidator = new WordValidator();
    private Trie trie = new Trie();
    private int letterCount = 88;
    private int score = 0;
    private bool isFirstWord = true;

    public Board mainBoard;
    public Board rackBoard;
    public PieceManager pieceManager;
    public TextMeshProUGUI lettCountTextField;
    public TextMeshProUGUI scoreTextField;
    public Text resultTextField;

    void ObtainWordsFromRow(Vector2Int inputCoordinate, Board board)
    {
        switch (inputCoordinate.x)
        {
            case int n when (n == 0):
                {
                    if (board.allTiles[inputCoordinate.x + 1, inputCoordinate.y].isOccupied)
                        PrintFromRow(new Vector2Int(inputCoordinate.x, inputCoordinate.y), board);
                    else
                        ObtainWordsFromColumn(new Vector2Int(inputCoordinate.x, inputCoordinate.y), board);
                    break;
                }
            case int n when (n > 0 && n < 11):
                {
                    if ((board.allTiles[inputCoordinate.x - 1, inputCoordinate.y].isOccupied) || (board.allTiles[inputCoordinate.x + 1, inputCoordinate.y].isOccupied))
                    {
                        while (board.allTiles[inputCoordinate.x - 1, inputCoordinate.y].isOccupied)
                        {
                            inputCoordinate.x--;
                            if (inputCoordinate.x < 1)
                                break;
                        }
                        PrintFromRow(new Vector2Int(inputCoordinate.x, inputCoordinate.y), board);
                    }
                    else
                        ObtainWordsFromColumn(new Vector2Int(inputCoordinate.x, inputCoordinate.y), board);
                    break;
                }
            case int n when (n == 11):
                {
                    if (board.allTiles[inputCoordinate.x - 1, inputCoordinate.y].isOccupied)
                    {
                        while (board.allTiles[inputCoordinate.x - 1, inputCoordinate.y].isOccupied)
                        {
                            // Transverse all the way left
                            if (inputCoordinate.x < 0)
                                return;
                            inputCoordinate.x--;
                        }
                        PrintFromRow(new Vector2Int(inputCoordinate.x, inputCoordinate.y), board);
                    }
                    else
                        ObtainWordsFromColumn(new Vector2Int(inputCoordinate.x, inputCoordinate.y), board);
                    break;
                }
        }
    }

    void ObtainWordsFromColumn(Vector2Int inputCoordinate, Board board)
    {
        while (board.allTiles[inputCoordinate.x, inputCoordinate.y].isOccupied)
        {
            inputCoordinate.y++;
            if (inputCoordinate.y == 12)
                break;
        }
        PrintFromColumn(new Vector2Int(inputCoordinate.x, inputCoordinate.y-1), board);
    }

    void PrintFromRow(Vector2Int leftCoordinate, Board board)
    {
        string resultingWord = "";
        int neighbour = 0;
        Vector2Int coordinate = leftCoordinate;
        while (board.allTiles[coordinate.x, coordinate.y].isOccupied)
        {
            resultingWord += board.allTiles[coordinate.x, coordinate.y].currentPiece.alphabet;
            if (FindNeighboursRow(coordinate.x, coordinate.y, board))
                neighbour++;
            coordinate.x++;
            if (coordinate.x == 12)
                break;
        }
        // Check for adjacent neighbours
        if (neighbour > 0 || isFirstWord)
        {
            bool valid = trie.Search(resultingWord);
            PrintResult(resultingWord, valid);
            AfterValidationTileMovementRow(leftCoordinate, board, valid);
            if(valid)
                isFirstWord = false;
        }
        else
        {
            bool temp = false;
            resultTextField.text += "<color=red>******* WORDS MUST BE ADJACENT *******" + resultingWord + "</color> \n";
            AfterValidationTileMovementRow(leftCoordinate, board, temp);
        }
    }

    void PrintFromColumn(Vector2Int TopCoordinate, Board board)
    {
        string resultingWord = "";
        int neighbour = 0;
        Vector2Int coordinate = TopCoordinate;
        while (board.allTiles[coordinate.x, coordinate.y].isOccupied)
        {
            resultingWord += board.allTiles[coordinate.x, coordinate.y].currentPiece.alphabet;
            if (FindNeighboursColumn(coordinate.x, coordinate.y, board)) 
                neighbour++;
            coordinate.y--;
            if (coordinate.y < 0)
                break;
        }
        if (neighbour > 0 || isFirstWord)
        {
            bool valid = trie.Search(resultingWord);
            PrintResult(resultingWord, valid);
            AfterValidationTileMovementColumn(TopCoordinate, board, valid);
            if (valid)
                isFirstWord = false;
        }
        else
        {
            bool temp = false;
            resultTextField.text += "<color=red>******* WORDS MUST BE ADJACENT *******" + resultingWord + "</color> \n";
            AfterValidationTileMovementColumn(TopCoordinate, board, temp);
        }
    }

    bool FindNeighboursRow(int x, int y, Board board)
    {
        switch (y)
        {
            case int n when (n == 0):
                {
                    if (board.allTiles[x, y + 1].isFixed)
                        return true;
                    break;
                }
            case int n when (n > 0 && n < 11):
                {
                    if ((board.allTiles[x, y+1].isFixed) || (board.allTiles[x, y-1].isFixed))
                        return true;
                    break;
                }
            case int n when (n == 11):
                {
                    if (board.allTiles[x, y - 1].isFixed)
                        return true;
                    break;
                }
        }
        return false;
    }

    bool FindNeighboursColumn(int x , int y, Board board)
    {
        switch (x)
        {
            case int n when (n == 0):
                {
                    if (board.allTiles[x + 1, y].isFixed)
                        return true;
                    break;
                }
            case int n when (n > 0 && n < 11):
                {
                    if ((board.allTiles[x + 1, y].isFixed) || (board.allTiles[x - 1, y].isFixed))
                        return true;
                    break;
                }
            case int n when (n == 11):
                {
                    if (board.allTiles[x - 1, y].isFixed)
                        return true;
                    break;
                }
        }
        return false;
    }

    void AfterValidationTileMovementRow(Vector2Int coordinate, Board board, bool result)
    {
        if (result == false)
        {
            while (board.allTiles[coordinate.x, coordinate.y].isOccupied == true)
            {
                board.allTiles[coordinate.x, coordinate.y].currentPiece.ResetPiece();   
                coordinate.x++;
                if (coordinate.x == 12)
                    break;
            }
        }
        else
        {
            while (board.allTiles[coordinate.x, coordinate.y].isOccupied == true)
            {
                board.allTiles[coordinate.x, coordinate.y].isFixed = true;
                coordinate.x++;
                if (coordinate.x == 12)
                    break;
            }
        }
    }

    void AfterValidationTileMovementColumn(Vector2Int coordinate, Board board, bool result)
    {
        if (result == false)
        {
            while (board.allTiles[coordinate.x, coordinate.y].isOccupied == true)
            {
                board.allTiles[coordinate.x, coordinate.y].currentPiece.ResetPiece();
                coordinate.y--;
                if (coordinate.y < 0)
                    break;
            }
        }
        else
        {
            while (board.allTiles[coordinate.x, coordinate.y].isOccupied == true)
            {
                board.allTiles[coordinate.x, coordinate.y].isFixed = true;
                coordinate.y--;
                if (coordinate.y < 0)
                    break;
            }
        }
    }

    void PrintResult(string word, bool result)
    {
        if (result)
            resultTextField.text += "•  <color=yellow>" + word + "</color> " + " is a valid word\n";
        else
            resultTextField.text += "•  <color=red>" + word + "</color> " + " is not a valid word\n";
    }

    void RefillRackBoard(Board board)
    {
        int emptyTilesNo = pieceManager.CheckEmptyTiles(rackBoard);
        pieceManager.Setup(board, emptyTilesNo);
        letterCount -= emptyTilesNo;
        score += emptyTilesNo;
        lettCountTextField.text = "TILES LEFT : " + letterCount.ToString();
        scoreTextField.text = "SCORE : " + score.ToString();
    }

    public void UpdatedCoordinates(Vector2Int lastCoordinates)
    {
        lastInputCoordinates = lastCoordinates;
        Debug.Log(lastInputCoordinates);
    }

    public void Submit()
    {
        ObtainWordsFromRow(lastInputCoordinates, mainBoard);
        RefillRackBoard(rackBoard);
    }
}
