using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/// <summary>
/// Se encarga de detectar cuando una moneda
/// es recogido por el jugador y utiliza las funciones del script "CoinContController"
/// </summary>
public class CoinController : MonoBehaviour
{
    [SerializeField] private float cantidadPuntos;
    [SerializeField] private Puntaje puntaje;
    
    /// <summary>
    /// Detecta cuando se colisiona la moneda con el jugador
    /// aumenta el puntaje en 100 cada vez que agarra una moneda
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           
            puntaje.SumarPuntos(cantidadPuntos);
            Destroy(gameObject);
            CoinContController.coinContController.cont += 1;
            CoinContController.coinContController.saveData(CoinContController.coinContController.cont);
        }
    }
}
