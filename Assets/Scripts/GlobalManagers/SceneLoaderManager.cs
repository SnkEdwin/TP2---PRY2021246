using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNewScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void LoadNewScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void SetDificulty(int n)
    {
        DificultyManager.SetDificulty(n);
    }
}
