using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    //-------------------------------------------

    public static GameManager instance;

    public GameObject missile;
    public GameObject antiMissile;
    public GameObject[] towers;
    public GameObject[] buildings;
    public GameObject shotLimit;

    public GameObject mainMenuCanvas;
    public GameObject gameOverCanvas;
    public GameObject levelCanvas;

    public TMPro.TextMeshProUGUI levelText;

    public enum GameState
    {
        mainMenu,
        inGame,
        changeLevel,
        pause,
        gameOver
    }
    
    private GameState gameState;
    private int level;
    private int missileCount, missilesPerStage;
    private int towersLeft, buildingsLeft;
    private float missileTime, lastMissileTime;

    //-------------------------------------------

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    //-------------------------------------------

    private void Start()
    {
        init();
        gameState = GameState.mainMenu;
        mainMenuCanvas.SetActive(true);
    }

    //-------------------------------------------

    private void Update()
    {
        if (gameState == GameState.inGame)
        {
            if (lastMissileTime > missileTime && missileCount < missilesPerStage)
            {
                Instantiate(missile);
                lastMissileTime = 0;
                missileCount++;
            }
            lastMissileTime += Time.deltaTime;

            if (Input.GetMouseButtonUp(0))
            {
                instantiateAntiMissile();
            }

            if (missileCount == missilesPerStage && checkForNextLevel())
            {
                gameState = GameState.changeLevel;
                Invoke("startNextLevel", 2f);
            }
        }
    }

    //-------------------------------------------

    private void init()
    {
        level = 1;
        missileCount = 0;
        missilesPerStage = 10;
        missileTime = 2f;
        lastMissileTime = 0f;
        towersLeft = 3;
        buildingsLeft = 6;

        destroyAliveObjects();
    }

    //-------------------------------------------

    private void destroyAliveObjects()
    {
        GameObject[] aliveMissiles = GameObject.FindGameObjectsWithTag("Missile");
        foreach (var aliveMissile in aliveMissiles)
        {
            Destroy(aliveMissile);
        }

        GameObject[] aliveAntiMissiles = GameObject.FindGameObjectsWithTag("Antimissile");
        foreach (var aliveAntiMissile in aliveAntiMissiles)
        {
            Destroy(aliveAntiMissile);
        }

        resetTowers();

        foreach (var building in buildings)
        {
            building.gameObject.GetComponent<BuildingLogic>().init();
        }
    }

    //-------------------------------------------

    private void resetTowers()
    {
        foreach (var tower in towers)
        {
            tower.transform.Find("TowerTop").gameObject.GetComponent<TowerTopLogic>().init();
        }
    }

    //-------------------------------------------

    private void instantiateAntiMissile()
    {
        // Get the nearest tower to the mouse position
        Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (target.y < shotLimit.transform.position.y)
            return;

        int nearestTower = -1;
        float closestDistance = Mathf.Infinity;
        for (int i = 0; i < towers.Length; i++)
        {
            if (towers[i].transform.Find("TowerTop").gameObject.GetComponent<TowerTopLogic>().getIsAlive())
            {
                if (Vector2.Distance(towers[i].transform.Find("Cannon").transform.position, target) < closestDistance)
                {
                    nearestTower = i;
                    closestDistance = Vector2.Distance(towers[i].transform.position, target);
                }
            }
        }

        if (nearestTower > -1)
        {
            Transform towerTransform = towers[nearestTower].transform;
            if (towerTransform.Find("TowerTop").GetComponent<TowerTopLogic>().getAmmoLeft() <= 0)
                return;

            Instantiate(antiMissile, towerTransform.Find("Cannon").transform.Find("ShotPosition").transform.position, Quaternion.identity);
            towerTransform.Find("TowerTop").GetComponent<TowerTopLogic>().shot();
        }
    }

    //-------------------------------------------

    public GameState getGameState()
    {
        return gameState;
    }

    //-------------------------------------------

    public void removeTower()
    {
        towersLeft--;
    }

    //-------------------------------------------

    public void removeBuilding()
    {
        buildingsLeft--;
        if (buildingsLeft <= 0)
        {
            setGameOver();
        }
    }

    //-------------------------------------------

    private bool checkGameOver()
    {
        return gameState == GameState.gameOver;
    }

    //-------------------------------------------

    public void pauseGame()
    {
        if (gameState != GameState.inGame)
            return;

        Time.timeScale = 0f;
        gameState = GameState.pause;
    }

    //-------------------------------------------

    public void resumeGame()
    {
        if (gameState != GameState.pause)
            return;

        Time.timeScale = 1f;
        gameState = GameState.inGame;
    }

    //-------------------------------------------

    public void startGame()
    {
        if (gameState == GameState.mainMenu || gameState == GameState.gameOver)
        {
            gameOverCanvas.SetActive(false);
            mainMenuCanvas.SetActive(false);
            init();
            showLevel();
            gameState = GameState.inGame;
        }
    }

    //-------------------------------------------

    public void setGameOver()
    {
        gameState = GameState.gameOver;
        gameOverCanvas.SetActive(true);
    }

    //-------------------------------------------

    private void showLevel()
    {
        levelText.text = $"Nivel { level }";
        levelCanvas.SetActive(true);
        Invoke("hideLevelCanvas", 2f);
    }

    private void hideLevelCanvas()
    {
        levelCanvas.SetActive(false);
    }

    //-------------------------------------------

    private bool checkForNextLevel()
    {
        return (GameObject.FindGameObjectsWithTag("Missile").Length == 0);
    }

    //-------------------------------------------

    private void startNextLevel()
    {
        level++;
        missileCount = 0;
        missilesPerStage = missilesPerStage < 28 ? missilesPerStage + 2 : missilesPerStage;
        resetTowers();
        showLevel();
        gameState = GameState.inGame;
    }

    //-------------------------------------------

    public int getCurrentLevel()
    {
        return level;
    }

    //-------------------------------------------
}
