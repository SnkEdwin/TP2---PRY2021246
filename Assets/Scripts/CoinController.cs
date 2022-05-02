using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] private float cantidadPuntos;
    [SerializeField] private Puntaje puntaje;
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
