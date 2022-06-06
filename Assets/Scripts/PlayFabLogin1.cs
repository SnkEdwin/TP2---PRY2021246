using System.Collections;
using System.Collections.Generic;
using System.Threading;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Este es el script para iniciar sesión mediante llamada al API del PlayFab
/// Como también, funciones que se crearon para asignarlos a los botones correspondientes
/// </summary>
public class PlayFabLogin1 : MonoBehaviour
{
   
    public InputField emailInput;
    public InputField passwordInput;
    public Text errorMessage;
    public void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        { 

            PlayFabSettings.TitleId = "2A8BF"; // Please change this value to your own titleId from PlayFab Game Manager 

        }
    }

    /// <summary>
    /// Esta funcionalidad es la que se encarga de mandar los datos necesarios para el inicio de sesión.
    /// como tambien, antes de mandar la información, se valida primero estos campos
    /// </summary>
    public void LoginButton()
    {
        if ( emailInput.text.Trim()=="" || passwordInput.text.Trim()=="")
        {
            errorMessage.text = "LLene todos los campos requeridos";
            return;
        }
        
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInput.text.Trim(),
            Password = passwordInput.text.Trim()
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    /// <summary>
    /// Funcionalidad cuando la operación de inicio de sesión es exitosa.
    /// Manda al usuario al menú de opciones
    /// </summary>
    /// <param name="obj"></param>
    public void OnLoginSuccess(LoginResult obj)
    {
        SceneManager.LoadScene("Menu_Options");
       
    }
    
    /// <summary>
    /// En caso de que la operación de inicio de sesión es fallida, se mostrará el mensaje de error
    /// </summary>
    /// <param name="obj"></param>
    private void OnLoginFailure(PlayFabError obj)
    {
        
        errorMessage.text = "Correo electrónico y/o contraseña incorrecta";
        
        
    }
    
    
}
