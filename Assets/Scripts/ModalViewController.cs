using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModalViewController : MonoBehaviour
{
  
    public GameObject modalWindow;
    private void Start()
    {
        modalWindow.SetActive(false);
    }

    public void clicked(Button button)
    {
        
        modalWindow.SetActive(true);
    }
    
    public void Close()
    {
        modalWindow.SetActive(false);
    }
}
