using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    #region Fields

    public GameObject tilePrefab;
    public int rowLength;
    public int columnHeight;

    [HideInInspector]
    public Tile[,] allTiles;

    #endregion

    #region Methods

    public void Create()
    {
        allTiles = new Tile[rowLength, columnHeight];
        for (int y = 0; y < columnHeight; y++)
        {
            for (int x = 0; x < rowLength; x++)
            {
                // Create new Tile
                GameObject newTile = Instantiate(tilePrefab, transform);

                // Its Position
                RectTransform rectTransform = newTile.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((x*70) +35, (y*70) +35);

                // Setup
                allTiles[x, y] = newTile.GetComponent<Tile>();
                allTiles[x, y].Setup(new Vector2Int(x, y), this);    
            }
        }
        #endregion
    }



}
