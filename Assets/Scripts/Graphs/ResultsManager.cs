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
                foreach (Resultado item in GetLastResults())
                {
                    Debug.LogWarning("resultado " + item.fecha + " | dificultad = " + item.dificultad + " | precision = " + item.precision);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            int randomPuntaje = Random.Range(0, 1000);
            AddResult(System.DateTime.Now.ToString(), 1, 1, randomPuntaje, 3, 0.1f);
        }
    }

    public void AddResult(string fecha, int minijuego, int dificultad, int puntaje, int tiempo, float precision)
    {
        Resultado resultado = new Resultado(fecha,minijuego,dificultad,puntaje,tiempo,precision);

        loadedResults.Add(resultado);
    }

    public List<Resultado> GetLastResults()
    {
        List<Resultado> lastResults = new List<Resultado>();

        if (loadedResults.Count > maxResults)
        {
            for (int i = loadedResults.Count - 1; i >= loadedResults.Count - maxResults; i--)
            {
                lastResults.Add(loadedResults[i]);
            }
        } else
        {
            lastResults = loadedResults;
        }
        

        return lastResults;
    }
}
