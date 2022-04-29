using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DificultyManager
{

    static int dificulty = 0;


    public static void SetDificulty(int n)
    {
        dificulty = n;
    }

    public static int GetDificulty()
    {
        return dificulty;
    }
}
