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
    public string minSeg;
    private string timePrefsName = "Time";

    void Awake()
    {
        
        if (timeController == null) timeController = this;
        enMarcha = true;
        loadData();
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
            int tempMin = Mathf.FloorToInt(restante / 60) ;
            int tempSeg = Mathf.FloorToInt(restante % 60);
            minSeg = string.Format("{00:00}:{1:00}", tempMin, tempSeg);
            tiempo.text = minSeg;
        }
    }

    /*public void disminucion (float a)
    {
        restante = restante - a;
    }
    public void aumento (float a)
    {
        restante = restante + a;
    }*/

    private void OnDestroy()
    {
        saveData();
    }
    public  void IsOver()
    {
        
        //restante = 0;
        //PlayerPrefs.SetFloat(timePrefsName,restante);
        timeController.setTimePrefs(0);
    }

    private void saveData()
    {
        //PlayerPrefs.SetFloat(timePrefsName,restante);
        Debug.Log(restante);
        timeController.setTimePrefs(restante);
    }

    private void loadData()
    {
        //restante = PlayerPrefs.GetFloat(timePrefsName, 0);
        restante = timeController.getTimePrefs();
        if (restante == null)
        {
            restante = 0;
        }
        else
        {
            Debug.Log(restante);
        }
        
        
    }

    public void IsGameOver()
    {
        enMarcha = false;
    }
    
    private void OnApplicationQuit()
    {
        //restante = 0;
        //PlayerPrefs.SetFloat(timePrefsName,restante);
        timeController.setTimePrefs(0);
    }
    
    public void setTimePrefs(float n)
    {
        restante = n;
    }
    
    public float getTimePrefs()
    {
        return restante;
    }
    
    
}
