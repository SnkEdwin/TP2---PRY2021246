using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphManager : MonoBehaviour
{

    public Sprite pointSprite;
    public RectTransform graphContainer;

    //public float graphHeight;

    public RectTransform labelTemplateX;
    public RectTransform labelTemplateY;

    public List<string> labelsX;
    public List<string> labelsY;
    public List<int> debugValuesINT;

    // Start is called before the first frame update
    void Update()
    {
        //List<int> values = new List<int>() { 5, 25, 1000, 45, 30, 22, 17 };//, 15, 13, 17, 25, 37, 40, 36, 33 };
        //ShowGraph(values);
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadGraph();
        }
    }

    public void LoadGraph()
    {
        //Conseguir Resultados filtrados <- Cambiar luego a filtrado por especificaciones
        List<Resultado> results = new List<Resultado>();
        results = ResultsManager.instance.GetLastResults();

        //Crear y asignar valores
        List<int> values = new List<int>();
        labelsX.Clear();
        labelsY.Clear();

        foreach (Resultado item in results)
        {
            values.Add(item.puntaje);
            labelsX.Add(item.fecha);
            labelsY.Add(item.fecha);
        }

        debugValuesINT = values;
        ShowGraph(values);
    }

    private GameObject CreateCircle(Vector2 position)
    {
        GameObject circle = new GameObject("Circle", typeof(Image));
        circle.transform.SetParent(graphContainer, false);
        circle.GetComponent<Image>().sprite = pointSprite;
        RectTransform rectTransform = circle.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

        return circle;
    }

    private void ShowGraph(List<int> valueList)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float graphWidth = graphContainer.sizeDelta.x;
        float yMaximum = 0f;
        float yMinimum = 0f;
        float offset = 0.0f;

        float xSize = graphWidth / (valueList.Count + 1);
        foreach (int value in valueList)
        {
            if (value > yMaximum)
            {
                yMaximum = value;
            }
            if (value < yMinimum)
            {
                yMinimum = value;
            }
        }

        yMaximum = yMaximum + ((yMaximum - yMinimum) * offset);
        yMinimum = yMinimum - ((yMaximum - yMinimum) * offset);
        

        GameObject previousCircle = null;

        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = xSize + i * xSize;
            float yPosition = ((valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));

            if (previousCircle != null)
            {
                CreateDotConnection(previousCircle.GetComponent<RectTransform>().anchoredPosition,
                                    circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            previousCircle = circleGameObject;

            RectTransform labelX = Instantiate(labelTemplateX);

            labelX.SetParent(graphContainer);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -2.5f);
            labelX.GetComponent<TextMeshProUGUI>().text = labelsX[i];
        }

        for (int i = 0; i <= labelsY.Count; i++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / labelsY.Count;
            labelY.anchoredPosition = new Vector2(-20f, normalizedValue*graphHeight);
            labelY.GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(normalizedValue * yMaximum).ToString();
        }
    }

    private void CreateDotConnection(Vector2 posA, Vector2 posB)
    {
        GameObject connection = new GameObject("Connection", typeof(Image));
        connection.transform.SetParent(graphContainer, false); 
        connection.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        
        RectTransform rectTransform = connection.GetComponent<RectTransform>();
        Vector2 dir = (posB - posA).normalized;
        float distance = Vector2.Distance(posA, posB);

        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = posA + dir * distance * 0.5f;

        float rotation = Mathf.Atan2(dir.y, dir.x) * 180 / Mathf.PI;
        rectTransform.localEulerAngles = new Vector3(0, 0,rotation);

    }
}
