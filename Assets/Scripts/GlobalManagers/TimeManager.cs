using UnityEngine;

/// <summary>
/// El TIMEMANAGER se encarga de guardar el tiempo del controlador TimeController
/// </summary>
public static class TimeManager
{
    
    static float restante;
    
    /// <summary>
    /// Se guarda la información del tiempo 
    /// </summary>
    /// <param name="n"></param>
    public static void setTimePrefs(float n)
    {
        restante = n;
    }
    
    /// <summary>
    /// Retorna la información del tiempo
    /// </summary>
    /// <returns></returns>
    public static float getTimePrefs()
    {
        return restante;
    }
}