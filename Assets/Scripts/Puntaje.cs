using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Puntaje : MonoBehaviour
{
    public static Puntaje puntaje;
    private float puntos;
    private TextMeshProUGUI textMeshPro;
    private string pointsPrefsName = "Point";
    private bool start = true;
    private void Awake()
    {
        
        if (start)
        {
            start = false;
        }
        else
        {
            loadData();
        }
        
        
    }

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
       
    }
    
    private void Update()
    {
        //puntos += Time.deltaTime;
        textMeshPro.text = puntos.ToString("0");
    }

    public void SumarPuntos(float puntosEntrada)
    {
        puntos += puntosEntrada;
    }
    
    private void OnDestroy()
    {
        saveData();
    }

    public void IsOver()
    {
        puntos = 0;
        PlayerPrefs.DeleteKey(pointsPrefsName);
    }
    
    private void saveData()
    {
        PlayerPrefs.SetFloat(pointsPrefsName,puntos);
    }

    private void loadData()
    {
        puntos = PlayerPrefs.GetFloat(pointsPrefsName, 0);
    }

    private void OnApplicationQuit()
    {
        puntos = 0;
        PlayerPrefs.SetFloat(pointsPrefsName,puntos);
    }
}
