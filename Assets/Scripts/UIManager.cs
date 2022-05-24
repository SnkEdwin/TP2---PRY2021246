using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Este es el controlador para las opciones que tendrá el menú de pausa
/// Tendrá 3 opciones: Volver al juego, Ir al menú principal ty salir de la aplicación
/// </summary>
public class UIManager : MonoBehaviour
{
    public GameObject optionsPanel;

    private void Awake()
    {
        optionsPanel.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Activa el menú de pausa
    /// </summary>
    public void OptionPanel()
    {
        GameManager.sharedInstance.currentGameState = GameManager.GameState.menu;
        optionsPanel.SetActive(true);
    }
    
    /// <summary>
    /// Regresa al juego
    /// </summary>
    public void Return()
    {
        GameManager.sharedInstance.currentGameState = GameManager.GameState.inGame;
        optionsPanel.SetActive(false);
    }
    
    
    /// <summary>
    /// Se cargará el menú principal y resetea los valores de puntaje, tiempo, contadores de monedas, aciertos y fallos
    /// </summary>
    /// <param name="loadScene"></param>
    public void GoMainMenu(string loadScene)
    {
        
        SceneManager.LoadScene(loadScene);
        TimeController.timeController.IsGameOver();
        TimeController.timeController.IsOver();
        CoinContController.coinContController.IsOver();
        CoinContController.coinContController.IsGameOver();
        
        ContController.setContFailure(0);
        ContController.setContSuccess(0);
        print("contFailure: "+ContController.getContFailure());
        print("contSuccess: "+ContController.getContSuccess());
        Puntaje.puntaje.IsOver();
        Puntaje.puntaje.IsGameOver();
        
    }
    
    /// <summary>
    /// Sale de la aplicación
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();  
    }
}
