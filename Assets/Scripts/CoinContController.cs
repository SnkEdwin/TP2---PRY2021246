using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Se encarga de contar las monedas cuando el jugador los recoge en los niveles
/// Lógica de negocio:
/// Aumentan en 1 cuando el jugador los recoge y lo muestra en la interfaz del juego
/// </summary>
public class CoinContController : MonoBehaviour
{
    public float cont;
    [SerializeField]  Text contador;
    private bool enMarcha;
    public static CoinContController coinContController;
    private string coinPrefsName = "Coin";
    
   
   
    
    /// <summary>
    /// Se recupera la información de la escena anterior y
    /// se guarda nuevamente en las variables
    /// realizando el proceso de pase de datos de una escena a otra
    /// </summary>
    private void Awake()
    {

        loadData();
        if (coinContController == null) coinContController = this;
        enMarcha = true;
    }
    /// <summary>
    /// Se escribe la cantidad de monedas en la interfaz del juego
    /// </summary>
    void Update()
    {
        if (enMarcha)
        {
           contador.text = cont.ToString();
        }
    }
    
   /// <summary>
   /// Guarda la cantidad de monedas
   /// </summary>
   /// <param name="_cont"></param>
    public void saveData(float _cont)
    {
        this.cont = _cont;
        PlayerPrefs.SetFloat(coinPrefsName,cont);
    }
    
    /// <summary>
    /// Vuelve al contador en 0
    /// y se guarda el valor en la etiqueta coinPrefsName="Time"
    /// </summary>
    public  void IsOver()
    {
        
        cont = 0;
        PlayerPrefs.SetFloat(coinPrefsName,cont);
    }
    
    /// <summary>
    /// Dejar de contar las monedas
    /// </summary>
    public void IsGameOver()
    {
        enMarcha = false;
    }

    /// <summary>
    /// Obtiene el contador de monedas
    /// para la siguiente escena
    /// </summary>
    private void loadData()
    {
        cont = PlayerPrefs.GetFloat(coinPrefsName, 0);
    }
    /// <summary>
    /// Vuelve al contador en 0
    /// y se guarda el valor en la etiqueta coinPrefsName="Time"
    /// cuando sales de la aplicación
    /// </summary>
    private void OnApplicationQuit()
    {
        cont = 0;
        PlayerPrefs.SetFloat(coinPrefsName,cont);
    }
}
