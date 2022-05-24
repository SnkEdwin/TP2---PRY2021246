using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ImpactController : MonoBehaviour
{
    public AudioSource clip;
    
    /// <summary>
    /// Detecta la colision entre bloque y jugador
    /// activa el sonido correspondiente de correcto e incorrecto
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<AudioSource>().Play(); 
            
        }
    }
}
