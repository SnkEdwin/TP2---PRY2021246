using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoFigManager : MonoBehaviour
{
    public static GeoFigManager sharedInstance;


    [Range(0, 2)]
    public int dificulty;

    //Se usara desde nivel 1 al 5 siendo el nivel 0 el boton de inicio
    public int currentLvl = 0;
    private bool playing = true;

    [Header("DATA VARIABLES")]
    public float timeSpend;
    public int mistakes;

    [Header("SCORE VARIABLES")]
    public int currentScore = 0;
    private float startTimer;
    public List<int> thresholds;
    private int currentThreshold = 0;
    public List<int> levelScores;
    public List<int> dificultyBonus;


    [Header("LEVELS GROUP")]
    public List<Transform> easy;
    public List<Transform> medium;
    public List<Transform> hard;
    

    [Header("TEMPORARY VARIABLES")]
    public List<Transform> loadedLevels;
    public List<GameObject> completedTargets;


    // Start is called before the first frame update
    void Start()
    {
        if (sharedInstance == null) sharedInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            if (currentLvl > 0)
            {
                CheckLevelCompletion();

                //Check time Thresholds
                if (currentThreshold < thresholds.Count - 1)
                {
                    if (Time.time - startTimer >= thresholds[currentThreshold])
                    {
                        currentThreshold++;
                    }
                }
            }
        }
    }

    public void StartGame()
    {
        //Asign Dificulty
        switch (dificulty)
        {
            case 0:
                loadedLevels = easy;
                break;
            case 1:
                loadedLevels = medium;
                break;
            case 2:
                loadedLevels = hard;
                break;
        }

        //Load First Level
        NextLevel();
    }


    public void CheckLevelCompletion()
    {
        for (int i = 0; i < loadedLevels[currentLvl].childCount; i++)
        {
            if (loadedLevels[currentLvl].GetChild(i).GetComponent<Figure_obj>() != null)
            {
                if (!loadedLevels[currentLvl].GetChild(i).GetComponent<Figure_obj>().done)
                {
                    return;
                }
            }
        }

        NextLevel();
    }

    public void AddLevelScore()
    {
        currentScore += levelScores[currentThreshold];
    }

    public void AddDificultyScore()
    {
        currentScore += dificultyBonus[dificulty];
    }

    public void NextLevel()
    {
        if (currentLvl < 5)
        {
            //Add Scores
            if (currentLvl != 0)
            {
                AddLevelScore();
            }

            //Add time to total time
            timeSpend += Time.time - startTimer;

            //Change Start Timer
            startTimer = Time.time;

            //Change Level
            loadedLevels[currentLvl].gameObject.SetActive(false);
            completedTargets.Clear();
            currentLvl++;
            loadedLevels[currentLvl].gameObject.SetActive(true);
        }
        else
        {
            //Add time to total time
            timeSpend += Time.time - startTimer;

            AddLevelScore();
            AddDificultyScore();
            playing = false;
            Debug.Log("Niveles Finalizados");
        }
    }
}
