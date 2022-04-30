using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DificultyManager
{

    static int dificulty = 0;


    public static void SetDificulty(int n)
    {
        if (n > 2)
        {
            n = 2;
        }
        dificulty = n;
    }
    public static void NextDificulty()
    {
        if (dificulty < 2)
        {
            dificulty++;
        }
    }
    public static int GetDificulty()
    {
        return dificulty;
    }
}
