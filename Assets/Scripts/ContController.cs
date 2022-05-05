using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ContController
{
    static float contFailure=0;
    static float contSuccess=0;

    public static void setContFailure(float n)
    {
        contFailure = n;
    }
    
    public static float getContFailure()
    {
        return contFailure;
    }
    
    public static void setContSuccess(float n)
    {
        contSuccess = n;
    }
    
    public static float getContSuccess()
    {
        return contSuccess;
    }
    
}
