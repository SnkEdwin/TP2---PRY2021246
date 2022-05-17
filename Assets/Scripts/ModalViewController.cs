using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Se encarga de abrir las ventanas modal de dificultad y de nivel completado, 
/// </summary>
public class ModalViewController : MonoBehaviour
{
  
    public GameObject modalWindow;
    
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
}
