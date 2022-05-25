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

public class ResultsManager : MonoBehaviour
{
    public List<Resultado> loadedResults;
    public int maxResults;

    private string[] dificultadTexto = new string[] { "facil", "mediana", "dificil" };
    private int dificultad = 0;
    private string[] minijuegoTexto = new string[] { "Sucesiones", "Sumas y Restas", "Figuras Geometricas" };
    private int minijuego = 0;
    private string[] metricaTexto = new string[] { "Precision", "Tiempo" };
    private int metrica = 0;

    public static ResultsManager instance;

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
            int randomTiempo = Random.Range(0, 200);
            float randomPrecision = Random.Range(0, 1f);
            AddResult(System.DateTime.Now.ToString(),randomMinijuego, randomDificultad, randomPuntaje, randomTiempo, randomPrecision);
        }
    }

    public void AddResult(string fecha, int minijuego, int dificultad, int puntaje, int tiempo, float precision)
    {
        Resultado resultado = new Resultado(fecha, minijuego, dificultad, puntaje, tiempo, precision);

        loadedResults.Add(resultado);
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
