using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Este es el controlador de puntaje, se encarga del aumentar el valor del puntaje y mostrarlo en la interfaz del juego
/// Lógica del negocio:
/// Cada vez que se agarra una moneda, el puntaje deberá de aumentarse en 100.
/// Se registra la cantidad de puntaje cuando acaba una dificultad
/// </summary>
public class Puntaje : MonoBehaviour
{
    public static Puntaje puntaje;
    public float puntos;
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
    
    /// <summary>
    /// Se asigna el texto del objeto de puntaje y lo muestra en la interfaz del juego
    /// </summary>
    private void Update()
    {
        if (enMarcha)
        {
            textMeshPro.text = puntos.ToString("0");
        }

    }

    /// <summary>
    /// Suma los puntos conseguidos por conseguir una moneda
    /// </summary>
    /// <param name="puntosEntrada"></param>
    public void SumarPuntos(float puntosEntrada)
    {
        puntos += puntosEntrada;
    }
    
    /// <summary>
    /// Se guarda el puntaje para la siguiente escena
    /// </summary>
    private void OnDestroy()
    {
        saveData();
    }
    
    
    /// <summary>
    ///  Se resetea los puntos cuando pasas de dificultad
    /// </summary>
    public void IsOver()
    {
        
        puntos = 0;
        PlayerPrefs.SetFloat(pointsPrefsName,puntos);
    }
    
    /// <summary>
    /// Dejar de contar los puntos
    /// </summary>
    public void IsGameOver()
    {
        enMarcha = false;
    }
    
    /// <summary>
    /// Guarda el puntaje conseguido en el nivel
    /// </summary>
    private void saveData()
    {
        PlayerPrefs.SetFloat(pointsPrefsName,puntos);
    }

    /// <summary>
    /// Obtiene el puntaje del nivel anterior
    /// </summary>
    private void loadData()
    {
        puntos = PlayerPrefs.GetFloat(pointsPrefsName, 0);
    }

    /// <summary>
    /// Vuelve al contador en 0
    /// y se guarda el valor en la etiqueta pointsPrefsName="Point"
    /// cuando sales de la aplicación
    /// </summary>
    private void OnApplicationQuit()
    {
        puntos = 0;
        PlayerPrefs.SetFloat(pointsPrefsName,puntos);
    }
}
