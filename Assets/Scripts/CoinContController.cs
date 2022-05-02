using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinContController : MonoBehaviour
{
    public float cont;
    [SerializeField]  Text contador;
    private bool enMarcha;
    public static CoinContController coinContController;
    private string coinPrefsName = "Coin";
    private bool start = true;
    private void Awake()
    {
        if (start)
        {
            start = false;
        }
        else
        {
            loadData();
        }
        if (coinContController == null) coinContController = this;
        enMarcha = true;
    }
    void Update()
    {
        if (enMarcha)
        {
           contador.text = cont.ToString();
        }
    }
    
    public void saveData(float _cont)
    {
        this.cont = _cont;
        PlayerPrefs.SetFloat(coinPrefsName,cont);
    }
    
    public  void IsOver()
    {
        cont = 0;
    }

    private void loadData()
    {
        cont = PlayerPrefs.GetFloat(coinPrefsName, 0);
    }
    private void OnApplicationQuit()
    {
        cont = 0;
        PlayerPrefs.SetFloat(coinPrefsName,cont);
    }
}
