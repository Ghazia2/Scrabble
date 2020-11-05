using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Fields

    public PieceManager pieceManager;
    public Board board;
    public Board rack;
    public GameObject gameOverScreen;

    private WordValidator wordValidator = new WordValidator();

    #endregion

    private void Start()
    {
        board.Create();
        rack.Create();
        pieceManager.Setup(rack,12);
        wordValidator.Setup();
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
    }

    public void Replay()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
