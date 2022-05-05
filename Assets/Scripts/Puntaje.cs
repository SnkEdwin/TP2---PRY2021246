using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Puntaje : MonoBehaviour
{
    public static Puntaje puntaje;
    private float puntos;
    private bool enMarcha;
    private TextMeshProUGUI textMeshPro;
    private string pointsPrefsName = "Point";
    
    private void Awake()
    {

        loadData();
        if (puntaje == null) puntaje = this;
        enMarcha = true;
    }

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
       
    }
    
    private void Update()
    {
        if (enMarcha)
        {
            textMeshPro.text = puntos.ToString("0");
        }

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
        PlayerPrefs.SetFloat(pointsPrefsName,puntos);
    }
    public void IsGameOver()
    {
        enMarcha = false;
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
