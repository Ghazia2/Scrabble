using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class BasePiece : EventTrigger
{
    #region Field

    private Board mainBoard;
    private LetterGenerator letterGen = new LetterGenerator();

    protected Tile originalTile = null;
    protected Tile currentTile = null;
    protected Tile targetTile = null;

    protected RectTransform rectTransform = null;
    protected PieceManager pieceManager;
    protected WordExtractor wordExtractor;
    private TextMeshProUGUI tileText;

    public Vector2Int targetedCoordinates;
    public char alphabet;

    #endregion

    #region Methods

    private void Start()
    {
        tileText = GetComponentInChildren<TextMeshProUGUI>();
        alphabet = letterGen.ReturnLetter();
        tileText.text = alphabet.ToString();

        mainBoard = GameObject.FindGameObjectWithTag("MainBoard").GetComponent<Board>();
        wordExtractor = GameObject.FindGameObjectWithTag("WordExtractor").GetComponent<WordExtractor>();
    }

    public virtual void Setup(Color32 spriteColor, PieceManager newPieceManager)
    {
        // Contructor
        pieceManager = newPieceManager;
        GetComponent<Image>().color = spriteColor;
        rectTransform = GetComponent<RectTransform>();
    }

    public void PlaceTile(Tile NewTile) 
    {
        // Now that the Tile is created, its time to place it somewhere
        currentTile = NewTile;
        originalTile = NewTile; 
        currentTile.currentPiece = this;
        currentTile.isOccupied = true;

        transform.position = NewTile.transform.position;
        gameObject.SetActive(true);
    }

    protected virtual void Move()
    {
        this.currentTile.isOccupied = false; 
        this.currentTile = targetTile;
        currentTile.currentPiece = this;

        transform.position = currentTile.transform.position;
        wordExtractor.UpdatedCoordinates(targetedCoordinates);
        this.currentTile.isOccupied = true;
        targetTile = null;
    }

    public void ResetPiece()
    {
        if(!this.currentTile.isFixed)
        {
            this.currentTile.isOccupied = false;
            currentTile.currentPiece = null;
            currentTile = originalTile;
            currentTile.currentPiece = this;
            this.gameObject.transform.position = originalTile.transform.position;
            this.currentTile.isOccupied = true;
        }
    }

    #endregion

    #region Events

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        transform.position += (Vector3)eventData.delta;

        // Checks if the tile is available  
        for (int y = 0; y < 12; y++)
        {
            for (int x = 0; x < 12; x++)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(mainBoard.allTiles[x,y].rectTransfrom, Input.mousePosition))
                {
                    currentTile.isOccupied = false;
                    targetTile = mainBoard.allTiles[x,y];
                    targetedCoordinates = new Vector2Int(x, y);
                    return;
                }
                targetTile = null;
            }
        }
    }
           
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        if((!targetTile) || targetTile.isOccupied || currentTile.isFixed) 
        {
            currentTile.isOccupied = true;
            transform.position = currentTile.gameObject.transform.position;
            return;
        }
        Move();
    }

    #endregion
}
