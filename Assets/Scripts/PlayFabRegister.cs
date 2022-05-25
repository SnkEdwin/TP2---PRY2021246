using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
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
    private Regex regex = new Regex("^(?:[012]?[0-9]|3[01])[/](?:0?[1-9]|1[0-2])[/](?:[0-9]{2}){1,2}$");
    private DateTime dt;
    private bool isValid;
    public void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        { 

            PlayFabSettings.TitleId = "2A8BF"; // Please change this value to your own titleId from PlayFab Game Manager 

        }
    }
    
    public void RegisterButton()
    {
        
        if (userInput.text.Trim()=="" || emailInput.text.Trim()=="" || passwordInput.text.Trim()=="" || birthdayInput.text.Trim()=="")
        {
            message.text = "LLene todos los campos requeridos";
            return;
        }
        if (passwordInput.text.Length<6)
        {
            message.text = "La contraseña debe ser mínimo de 6 y máximo de 100 carácteres";
            return;
        }
        if (userInput.text.Length<4)
        {
            message.text = "El nombre de el usuario debe ser mínimo de 4 y máximo de 100 carácteres";
            return;
        }
        if (!regex.IsMatch(birthdayInput.text.Trim()))
        {
            
            message.text = "Formato ingresado no válido";
            return;
        }
       
        var request = new RegisterPlayFabUserRequest
        {
            Username = userInput.text.Trim(),
            Email = emailInput.text.Trim(),
            Password = passwordInput.text.Trim(),
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
        
       
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
       
        var request1 = new LoginWithEmailAddressRequest
        {
            Email = emailInput.text.Trim(),
            Password = passwordInput.text.Trim()
        };
        message.text = "Se ha registrado satisfactoriamente";
        Thread.Sleep(2000);
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
        
        switch (error.Error)
        {
            case PlayFabErrorCode.EmailAddressNotAvailable:
                message.text = "El correo ya se encuentra registrado";
                break;
            default:
                message.text = error.ErrorMessage;
                break;
            
        }

        
    }

   
}
