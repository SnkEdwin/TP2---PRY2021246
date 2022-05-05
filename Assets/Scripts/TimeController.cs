using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
  
    
    [SerializeField] Text tiempo;
    public static TimeController timeController;
    public float restante;
    private bool enMarcha;

    private string timePrefsName = "Time";

    void Awake()
    {
        loadData();
        if (timeController == null) timeController = this;
        enMarcha = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (enMarcha)
        {
            restante += Time.deltaTime;
            //if (restante <= 0.5)
            //{
            //    enMarcha = true;
            //    GameManager.sharedInstance.currentGameState = GameManager.GameState.gameOver;
            //}
            //int tempMin = Mathf.FloorToInt(restante / 60);
            //int tempSeg = Mathf.FloorToInt(restante % 60);
            tiempo.text = string.Format("{0}",(int)restante);
        }
    }

    public void disminucion (float a)
    {
        restante = restante - a;
    }
    public void aumento (float a)
    {
        restante = restante + a;
    }

    private void OnDestroy()
    {
        saveData();
    }
    public  void IsOver()
    {
        
        restante = 0;
    }

    private void saveData()
    {
        PlayerPrefs.SetFloat(timePrefsName,restante);
    }

    private void loadData()
    {
        restante = PlayerPrefs.GetFloat(timePrefsName, 0);
    }

    public void IsGameOver()
    {
        enMarcha = false;
    }
    
    private void OnApplicationQuit()
    {
        restante = 0;
        PlayerPrefs.SetFloat(timePrefsName,restante);
        
    }

}
