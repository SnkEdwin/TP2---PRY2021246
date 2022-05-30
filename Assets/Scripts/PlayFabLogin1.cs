using System.Collections;
using System.Collections.Generic;
using System.Threading;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public void OnLoginSuccess(LoginResult obj)
    {
        SceneManager.LoadScene("Menu_Options");
       
    }
    
    private void OnLoginFailure(PlayFabError obj)
    {
        
        errorMessage.text = "Correo electrónico y/o contraseña incorrecta";
        
        
    }
    
    
}
