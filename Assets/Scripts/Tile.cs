using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    #region Fields

    [HideInInspector]
    public Vector2Int boardPosition = Vector2Int.zero;
    [HideInInspector]
    public Board board = null;
    [HideInInspector]
    public RectTransform rectTransfrom = null;
    public BasePiece currentPiece = null;
    public bool isOccupied = false;
    public bool isFixed = false;

    #endregion  

    #region Methods

    public void Setup(Vector2Int newBoardPosition, Board newBoard)
    {
        boardPosition = newBoardPosition;
        board = newBoard;

        rectTransfrom = GetComponent<RectTransform>();
    }

    #endregion
}
