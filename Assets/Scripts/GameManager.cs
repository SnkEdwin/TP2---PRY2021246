using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Se incializa la aplicación y luego se crea los estados del juego
/// Se asigna al controlador del jugador a su objeto del juego
/// Después, una vez cargado se maneja los estados dependiendo del contexto en el que está
/// </summary>
public class GameManager : MonoBehaviour
{   //Poner los estados del juego

   
    public enum GameState
    {
        menu,
        inGame,
        gameOver
    }
    // Start is called before the first frame update

    //Inicializamos el enum
    public GameState currentGameState = GameState.menu;
    public static GameManager sharedInstance;

    private PlayerController controller;
    private void Awake()
    {   
        if (sharedInstance ==null) sharedInstance = this;
    }


    
    void Start()
    {
        controller = GameObject.Find("Player").GetComponent<PlayerController>();
        
    }

    // Update is called once per frame
    void Update()
    {   
        if (currentGameState == GameState.inGame)
        {
            Time.timeScale = 1f;
            AudioController.sharedInstance.audio.mute = false;
        }
        else if (currentGameState == GameState.menu)  {
            Time.timeScale = 0f;
            AudioController.sharedInstance.audio.mute = true;
        }
        else if (currentGameState == GameState.gameOver){
            Time.timeScale = 0f;
            
        }

        if (Input.GetKeyDown(KeyCode.S) && currentGameState != GameState.inGame)
        {
            StartGame();
        }
        else if (Input.GetKeyDown(KeyCode.P) )
        {
            BackToMenu(); 
        }

    }
    public void StartGame()
    {
        SetGameState(GameState.inGame);
    }
    public void GameOver()
    {
        SetGameState(GameState.gameOver);
    }
    public void BackToMenu()
    {
        SetGameState(GameState.menu);
    }
    //
    private void SetGameState(GameState newGameState)
    {
        if (newGameState == GameState.menu)
        {
            //TODO
        }
        else if (newGameState == GameState.inGame)
        {
            controller.StartGame(); 
        } 
        else if (newGameState == GameState.gameOver)
        {

        }
        this.currentGameState = newGameState;
    }

}
