using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;

public class ResultsManager : MonoBehaviour
{
    public List<Resultado> loadedResults;
    public int maxResults;

    private string[] dificultadTexto = new string[] { "Fácil", "Mediana", "Difícil" };
    private int dificultad = 0;
    private string[] minijuegoTexto = new string[] { "Sucesiones", "Sumas y Restas", "Figuras Geométricas" };
    private int minijuego = 0;
    private string[] metricaTexto = new string[] { "Precisión", "Tiempo" };
    private int metrica = 0;

    public static ResultsManager instance;

    public void SaveResults()
    {
        List<Resultado> results = new List<Resultado>();
        
        foreach (Resultado item in loadedResults)
        {
            results.Add(item);
        }

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Resultados", JsonConvert.SerializeObject(results) }
            }
        };

        PlayFabClientAPI.UpdateUserData(request,OnResultSend,OnResultError);
    }
    
    
    
    void OnResultSend(UpdateUserDataResult result)
    {
        Debug.Log("Resultados Guardados Exitosamente");
    }

    void OnResultError(PlayFabError error)
    {
        Debug.Log("Error al cargar los resultados");
    
    }

   
    

    // Start is called before the first frame update
    void Start()
    {
        if (ResultsManager.instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            loadedResults = new List<Resultado>();

            //Load results from database

            //
        } else
        {
            Destroy(this);
        }
        GetResults();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("TOdos los resultados");
            foreach (Resultado item in loadedResults)
            {
                Debug.Log("resultado " + item.fecha + " | dificultad = " + item.dificultad + " | precision = " + item.precision);
            }

            if (loadedResults.Count > maxResults)
            {
                Debug.LogWarning("Resultados recientes");
                foreach (Resultado item in GetFilteredResults())
                {
                    Debug.LogWarning("resultado " + item.fecha + " | dificultad = " + item.dificultad + " | precision = " + item.precision);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            int randomDificultad = Random.Range(0, 3);
            int randomMinijuego = Random.Range(0, 3);

            int randomPuntaje = Random.Range(0, 1000);
            int randomTiempo = Random.Range(0, 120);
            float randomPrecision = Random.Range(0, 1f);
            AddResult(System.DateTime.Now.ToString("MM/dd\nH:mm"),randomMinijuego, randomDificultad, randomPuntaje, randomTiempo, randomPrecision);
        }

        
    }
    
    public void GetResults()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnResultsDataRecieved, OnResultError);
    }
    
    private void OnResultsDataRecieved(GetUserDataResult resultado)
    {
        Debug.Log("Recieved results data!");
        if (resultado.Data != null && resultado.Data.ContainsKey("Resultados"))
        {
            
            List<Resultado> results= JsonConvert.DeserializeObject<List<Resultado>>(resultado.Data["Resultados"].Value);
            Debug.Log(results.Count);
            for (int i = 0; i < results.Count; i++)
            {
                
                AddResult(results[i].fecha, results[i].minijuego, results[i].dificultad, results[i].puntaje,
                    results[i].tiempo, results[i].precision);
            }
        }
    }

    public void AddResult(string fecha, int minijuego, int dificultad, int puntaje, int tiempo, float precision)
    {
        Resultado resultado = new Resultado(fecha, minijuego, dificultad, puntaje, tiempo, precision);
        loadedResults.Add(resultado);
        SaveResults();
    }

    public List<Resultado> GetFilteredResults()
    {
        List<Resultado> filteredResults = new List<Resultado>();
        List<Resultado> lastResults = new List<Resultado>();

        foreach (Resultado item in loadedResults)
        {
            if (item.dificultad != dificultad || item.minijuego != minijuego)
            {
                continue;
            } else
            {
                filteredResults.Add(item);
            }
        }

        if (filteredResults.Count > maxResults)
        {
            for (int i = filteredResults.Count - 1; i >= filteredResults.Count - maxResults; i--)
            {
                lastResults.Add(filteredResults[i]);
            }
        } else
        {
            lastResults = filteredResults;
        }
        
        return lastResults;
    }

    public int FilterNext(int value, int maxValue)
    {
        if (value < maxValue)
        {
            value++;
        } else
        {
            value = 0;
        }
        return value;
    }

    public string UpdateResultFilters(string filters, bool goNext)
    {
        string temp = "";
        switch (filters)
        {
            case "dificultad":
                if (goNext)
                {
                    dificultad = FilterNext(dificultad, dificultadTexto.Length - 1);
                }
                temp = dificultadTexto[dificultad];
                break;
            case "minijuego":
                if (goNext)
                {
                    minijuego = FilterNext(minijuego, minijuegoTexto.Length - 1);
                }
                temp = minijuegoTexto[minijuego];
                break;
            case "metrica":
                if (goNext)
                {
                    metrica = FilterNext(metrica, metricaTexto.Length - 1);
                }
                temp = metricaTexto[metrica];
                break;
            default:
                break;
        }

        return temp;
    }

    public int GetMetric()
    {
        return metrica;
    }
}
