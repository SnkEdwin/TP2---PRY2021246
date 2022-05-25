using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/// <summary>
/// Se encarga de abrir las ventanas modal de dificultad y de nivel completado, 
/// </summary>
public class ModalViewController : MonoBehaviour
{
  
    public GameObject modalWindow;

    //Variables de la ventana de Resultados
    public GameObject resultsWindow;
    public TextMeshProUGUI dificultad;
    public TextMeshProUGUI minijuego;
    public TextMeshProUGUI metrica;
    
    /// <summary>
    /// Se activa una ventana modal
    /// </summary>
    private void Start()
    {
        modalWindow.SetActive(false);
    }
    
    /// <summary>
    /// Se activa una ventana modal cuando se presiona un boton asignado
    /// </summary>
    public void clicked(Button button)
    {
        
        modalWindow.SetActive(true);
    }
    
    
    /// <summary>
    /// Cierra la ventana modal
    /// </summary>
    public void Close()
    {
        modalWindow.SetActive(false);
    }

    [SerializeField]
    /// <summary>
    /// Cambia el valor de alguno de los botones y el manager de resultado al siguiente posible
    /// </summary>
    /// <param name="value">Valor que se desea cambiar al siguiente</param>
    public void CallForNextValue(string value)
    {
        switch (value)
        {
            case "dificultad":
                dificultad.text = ResultsManager.instance.UpdateResultFilters(value,true);
                break;
            case "minijuego":
                minijuego.text = ResultsManager.instance.UpdateResultFilters(value,true);
                break;
            case "metrica":
                metrica.text = ResultsManager.instance.UpdateResultFilters(value,true);
                break;
            default:
                break;
        }
        GraphManager.instance.LoadGraph();
    }

    /// <summary>
    /// Determina el estado de la ventana de resultados
    /// </summary>
    /// <param name="state">Estado de la ventana(prendido, apagado)</param>
    public void SetStateResults(bool state)
    {
        //LLamar al manager de resultados
        if (state == true)
        {
            dificultad.text = ResultsManager.instance.UpdateResultFilters("dificultad", false);
            minijuego.text = ResultsManager.instance.UpdateResultFilters("minijuego", false);
            metrica.text = ResultsManager.instance.UpdateResultFilters("metrica", false);
        }
        resultsWindow.SetActive(state);
        GraphManager.instance.LoadGraph();
    }
}
