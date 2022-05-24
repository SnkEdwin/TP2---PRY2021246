using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Se encarga de guardar y entregar la información de los aciertos y fallos para los minijuegos.
/// Se utiliza en el script de PlayerController
/// </summary>
public static class ContController
{
    static float contFailure=0;
    static float contSuccess=0;

    /// <summary>
    /// Se guarda el contador de fallos cada vez que golpea un bloque incorrecto
    /// </summary>
    /// <param name="n"></param>
    public static void setContFailure(float n)
    {
        contFailure = n;
    }
    /// <summary>
    /// Retorna el contador de fallos
    /// </summary>
    /// <param name="n"></param>
    public static float getContFailure()
    {
        return contFailure;
    }
    /// <summary>
    /// Se guarda el contador de aciertos cada vez que golpea un bloque correcto
    /// </summary>
    /// <param name="n"></param>
    public static void setContSuccess(float n)
    {
        contSuccess = n;
    }
    
    /// <summary>
    /// Retorna el contador de aciertos
    /// </summary>
    /// <returns></returns>
    public static float getContSuccess()
    {
        return contSuccess;
    }
    
}
