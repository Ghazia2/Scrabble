using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PieceManager : MonoBehaviour
{
    private List<BasePiece> PieceList = null;

    public GameObject piecePrefab;
    public Board mainBoard;

    public void Setup(Board board, int listSize)
    {
        PieceList = CreatePieces(new Color32(80, 124, 159, 255), board, listSize);
        PlacePiece(0, 1, PieceList, board);
    }

    private List<BasePiece> CreatePieces(Color32 spriteColor, Board board, int howMany)
    {
        List<BasePiece> newPieces = new List<BasePiece>();
        for (int i = 0; i < howMany; i++)
        {
            // Create new Object
            GameObject newPieceObject = Instantiate(piecePrefab);
            newPieceObject.transform.SetParent(transform);

            // Set scale and position
            newPieceObject.transform.localScale = new Vector3(1, 1, 1);
            newPieceObject.transform.localRotation = Quaternion.identity;

            // Store new Object
            BasePiece newPiece = (BasePiece)newPieceObject.AddComponent<BasePiece>();
            newPieces.Add(newPiece);

        }
        return newPieces;
    }

    private void PlacePiece(int firstRowIndex, int secondRowIndex, List<BasePiece> pieces, Board board)
    {
        int index = 0;
        for (int y = 0; y < 2; y++)
        {
            for (int x = 0; x < 6; x++)
            {
                if (!board.allTiles[x, y].isOccupied)
                {
                    pieces[index].PlaceTile(board.allTiles[x, y]);
                    index++;
                }
            }
        }
    }

    public int CheckEmptyTiles(Board board)
    {
        int space = 0;
        for (int i = 0; i < 6; i++)
        {
            if (!board.allTiles[i, 0].isOccupied)
                space++;
            if (!board.allTiles[i, 1].isOccupied)
                space++;
        }
        return space;
    }
}