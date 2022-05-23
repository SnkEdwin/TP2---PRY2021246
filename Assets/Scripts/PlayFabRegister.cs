using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayFabRegister : MonoBehaviour
{
    public InputField userInput;
    public InputField emailInput;
    public InputField passwordInput;
    public InputField birthdayInput;
    public Text message;
  
    public void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        { 

            PlayFabSettings.TitleId = "2A8BF"; // Please change this value to your own titleId from PlayFab Game Manager 

        }
    }
    
    public void RegisterButton()
    {
        if (passwordInput.text.Length <6)
        {
            message.text = "La constraseña debe ser mayor o igual a 6 y menos de 100 carácteres";
            return;
        }
        
        var request = new RegisterPlayFabUserRequest
        {
            // TODO:  TAMBIEN DEBE ACEPTAR COMO INPUT LA FECHA DE NACIMIENTO
            Username = userInput.text,
            Email = emailInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = false
        };
        message.text = "Cargando...";
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
        
       
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        
        var request1 = new LoginWithEmailAddressRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text,
        };
        Thread.Sleep(1000);
        PlayFabClientAPI.LoginWithEmailAddress(request1,OnLoginSuccess,OnError);

        
    }
    public void OnLoginSuccess(LoginResult obj)
    {
        
        SaveBirthday();
        SceneManager.LoadScene("Menu_Options");
    }


    private void SaveBirthday()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string,string>
            {
                {"Fecha de nacimiento", birthdayInput.text}
            }
        };
        
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);

    }

    private void OnDataSend(UpdateUserDataResult obj)
    {
        Debug.Log("Succesful user data send!");
    }

    private void OnError(PlayFabError error)
    {
        message.text = error.ErrorMessage;
        Debug.Log(error.GenerateErrorReport());
    }

   
}
