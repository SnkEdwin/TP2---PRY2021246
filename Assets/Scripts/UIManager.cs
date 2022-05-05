using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public GameObject optionsPanel;

    private void Awake()
    {
        optionsPanel.gameObject.SetActive(false);
    }
    public void OptionPanel()
    {
        GameManager.sharedInstance.currentGameState = GameManager.GameState.menu;
        optionsPanel.SetActive(true);
    }
    public void Return()
    {
        GameManager.sharedInstance.currentGameState = GameManager.GameState.inGame;
        optionsPanel.SetActive(false);
    }
    public void AnotherOption()
    {

    }
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
    public void QuitGame()
    {
        Application.Quit();  
    }
}
