using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resultado
{
    public string fecha;
    public int minijuego;
    public int dificultad;
    public int puntaje;
    public int tiempo;
    public float precision;

    public Resultado()
    {
        fecha = "";
        minijuego = 0;
        dificultad = 0;
        puntaje = -1;
        tiempo = -1;
        precision = -1;
    }
    public Resultado(string fecha, int minijuego, int dificultad,
                int puntaje, int tiempo, float precision)
    {
        this.fecha = fecha;
        this.minijuego = minijuego;
        this.dificultad = dificultad;
        this.puntaje = puntaje;
        this.tiempo = tiempo;
        this.precision = precision;
    }
}

