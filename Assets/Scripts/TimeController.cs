using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Este el controlador del tiempo que utiliza el script de TIMEMANAGER para guardar información del tiempo y se realiza su proceso.
/// Lógica de negocio:
/// EL tiempo aumenta hasta acabar la dificultad de un minijuego y se registra el tiempo en que se demoró en acabarlo.
/// </summary>
public class TimeController : MonoBehaviour
{
  
    
    [SerializeField] Text tiempo;
    public static TimeController timeController;
    public float restante;
    private bool enMarcha;
    public string minSeg;
    

    
    /// <summary>
    /// Carga los datos de la escena anterior y continua incrementandose
    /// </summary>
    void Awake()
    {
        
        if (timeController == null) timeController = this;
        enMarcha = true;
        loadData();
    }

    // Update is called once per frame
    /// <summary>
    /// Se incrementa el valor del tiempo en segundos y lo muestra en la interfaz del juego
    /// </summary>
    void Update()
    {

        if (enMarcha)
        {
            restante += Time.deltaTime;
            int tempMin = Mathf.FloorToInt(restante / 60) ;
            int tempSeg = Mathf.FloorToInt(restante % 60);
            minSeg = string.Format("{00:00}:{1:00}", tempMin, tempSeg);
            tiempo.text = minSeg;
            saveData(restante);
        }
    }
    
    
    /// <summary>
    /// Resetea el tiempo a 0 
    /// </summary>
    public void IsOver()
    {
        TimeManager.setTimePrefs(0);
    }

    /// <summary>
    /// Guarda la información del tiempo 
    /// </summary>
    /// <param name="n"></param>
    private void saveData(float n)
    {
        TimeManager.setTimePrefs(n);
    }

    /// <summary>
    /// Se recupera la información del tiempo
    /// </summary>
    private void loadData()
    {
        
        restante = TimeManager.getTimePrefs();
    }
    
    /// <summary>
    /// Cambia el valor "enMarcha" a falso para que el tiempo
    /// no siga continuando
    /// </summary>
    public void IsGameOver()
    {
        enMarcha = false;
    }
    
    /// <summary>
    /// Resetea el valor del tiempo en 0 al salir de la aplicación
    /// </summary>
    private void OnApplicationQuit()
    {
        TimeManager.setTimePrefs(0);
    }

}
