using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    //-------------------------------------------

    public GameObject pauseCanvas;

    //-------------------------------------------

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.instance.getGameState() == GameManager.GameState.inGame)
        {
            pauseGame();
        }
    }

    //-------------------------------------------

    public void pauseGame()
    {
        GameManager.instance.pauseGame();
        pauseCanvas.SetActive(true);
    }

    //-------------------------------------------

    public void resumeGame()
    {
        pauseCanvas.SetActive(false);
        GameManager.instance.resumeGame();
    }

    //-------------------------------------------
}
