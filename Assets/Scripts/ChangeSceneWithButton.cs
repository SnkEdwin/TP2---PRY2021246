using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 


public class ChangeSceneWithButton : MonoBehaviour
{
    
    /// <summary>
    /// Carga la escena correspondiente 
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    /// <summary>
    /// Sales de la aplicación cuando se presiona el boton de salir
    /// </summary>
    public void Exit()
    {
        Application.Quit();
    }
}
