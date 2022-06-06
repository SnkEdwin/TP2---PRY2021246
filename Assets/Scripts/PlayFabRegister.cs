using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



/// <summary>
/// Este es el script para registrar al usuario mediante llamada al Api de PlayFab.
/// En él, se presenta los campos de entrada que se recibe en los campos de entrada de usuario, email, contraseña,  fecha de nacimiento, y mensajes de validaciones 
/// </summary>
public class PlayFabRegister : MonoBehaviour
{
    public InputField userInput;
    public InputField emailInput;
    public InputField passwordInput;
   // public InputField birthdayInput;
    public InputField nameInput2;
    public Text message;
    private bool isValid;
    
    // FECHA INPUT
    public TMPro.TMP_Dropdown dayInput;
    //private List<int> listDay = new List<int>();
    private List<string> dayStringList30Days = new List<string>(){
        "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20",
        "21", "22", "23", "24", "25", "26", "27", "28", "29", "30"
    };
    private List<string> dayStringList31Days = new List<string>(){
        "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20",
        "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31"
    };
    private List<string> dayStringList28Days = new List<string>(){
        "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20",
        "21", "22", "23", "24", "25", "26", "27", "28"
    };
    private List<string> dayStringList29Days = new List<string>(){
        "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20",
        "21", "22", "23", "24", "25", "26", "27", "28","29"
    };
    // Validadores de las fechas
    private bool a1;
    private bool a2;
    private bool a3;
    private bool a4;
    
    public TMPro.TMP_Dropdown monthInput;
    private List<int> listMonth= new List<int>();
    private List<string> monthStringList = new List<string>();
    
    public TMPro.TMP_Dropdown yearInput;
    private List<int> listYear= new List<int>();
    private List<string> yearStringList = new List<string>();
    
    private string yearBisiesto;
    private int yearBisiestoInt;

    private string date;
    public void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        { 

            PlayFabSettings.TitleId = "2A8BF"; // Please change this value to your own titleId from PlayFab Game Manager 
            //nameInput2.text = "";

        }
        dayInput.ClearOptions();
        RellenarDias();
        RellenarMes();
        RellenarAños();
        
    }

   

    /// <summary>
    /// Esta funcionalidad es la que se encarga de mandar los datos necesarios para el registro de un usuario,
    /// como tambien, antes de mandar la información, se valida primero estos campos
    /// </summary>
    public void RegisterButton()
    {
        
        
        date = string.Format("{0}/{1}/{2}", dayInput.captionText.text, monthInput.captionText.text,
            yearInput.captionText.text);
        message.text = "Cargando...";
        if (userInput.text.Trim()==""|| emailInput.text.Trim()=="" || passwordInput.text.Trim()=="" ||nameInput2.text=="")
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
        var request = new RegisterPlayFabUserRequest
        {
            Username = userInput.text,
            Email = emailInput.text.Trim(),
            Password = passwordInput.text.Trim(),
            RequireBothUsernameAndEmail = false

        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
        
       
    }

    /// <summary>
    /// Funcionalidad cuando la operación de registro es exitosa, se muestra el mensaje de registro exitoso
    /// </summary>
    /// <param name="result"></param>
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
        SceneManager.LoadScene("Loguin");
        
    }


    
    /// <summary>
    /// Funcionalidad para guardar la fecha de nacimiento y nombre 
    /// </summary>
    private void SaveBirthday()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string,string>
            {
                {"Fecha de nacimiento", date},
                {"Nombre", nameInput2.text}
            }
            
        };
        
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);

    }

    
    
    private void OnDataSend(UpdateUserDataResult obj)
    {
        Debug.Log("Succesful user data send!");
    }

    /// <summary>
    /// Funcionalidad cuando la operación de registro es fallido
    /// </summary>
    /// <param name="error"></param>
    private void OnError(PlayFabError error)
    {
        
        switch (error.Error)
        {
            case PlayFabErrorCode.EmailAddressNotAvailable:
                message.text = "El correo ya se encuentra registrado";
                break;
            case PlayFabErrorCode.UsernameNotAvailable:
                message.text = "El nombre de usuario ya está siendo utilizado por otro";
                break;
            default:
                //Debug.Log(error);
                message.text = error.ErrorMessage;
                break;
            
        }
    }

    /// <summary>
    /// Esta funcionalidad se encarga de validar la información del día. Cuando se cambia los valores de mes y año
    /// se cambia la lista de los días. 
    /// </summary>
    public void RellenarDias()
    {
        
        yearBisiesto = yearInput.captionText.text;
        if (monthInput.captionText.text=="4" || monthInput.captionText.text=="6" || monthInput.captionText.text=="9" || monthInput.captionText.text=="11")
        {
            if (a1)
            {
                // NO SE AGREGA DE NUEVO EL CONTENIDO
            }
            else
            {
                // busca el valor seleccionado anteriormente
                string text2 = dayInput.captionText.text;
                if (text2=="31")
                {
                    text2 = "30";
                }
                // se limpia el dropdown
                dayInput.ClearOptions();
                // Rellenar todos los 30 dias 
                dayInput.AddOptions(dayStringList30Days);
                // se asigna el valor seleccionado anteriormente en el nuevo dropdown
                dayInput.value = dayInput.options.FindIndex(options => options.text == text2);
                // Se valida si el mes que se selecciona tiene tambien 30 días ya no vuelva a agregarse los dias
                a1 = true;
                a2 = false;
                a3 = false;
                a4 = false;
            }
            
        }
        else if (monthInput.captionText.text == "2")
        {
            yearBisiestoInt= Int32.Parse(yearBisiesto);
            if ( (yearBisiestoInt % 4 == 0 && yearBisiestoInt % 100 !=0) || yearBisiestoInt % 400 == 0)
            {
                if (a3)
                {
                    // NO SE AGREGA DE NUEVO EL CONTENIDO
                }
                else
                {
                    // busca el valor seleccionado anteriormente
                    string text3 = dayInput.captionText.text;
                    if (text3=="31" || text3=="30")
                    {
                        text3 = "29";
                    }
                    // se limpia el dropdown
                    dayInput.ClearOptions();
                    // se agrega los 29 días 
                    dayInput.AddOptions(dayStringList29Days);
                    // se asigna el valor seleccionado anteriormente en el nuevo dropdown
                    dayInput.value = dayInput.options.FindIndex(options => options.text == text3);
                    // Se valida si el año es bisiesto y ya no vuelva a agregarse de nuevo
                    a1 = false;
                    a2 = false;
                    a4 = false;
                    a3 = true;
                }
                
            }
            else
            {
                if (a4)
                {
                    // NO SE AGREGA DE NUEVO EL CONTENIDO
                }
                else
                {
                    // busca el valor seleccionado anteriormente
                    string text4 = dayInput.captionText.text;
                    if (text4=="30" || text4=="31" || text4=="29")
                    {
                        text4 = "28";
                    }
                    // se limpia el dropdown
                    dayInput.ClearOptions();
                    // se agrega los 28 días 
                    dayInput.AddOptions(dayStringList28Days);    
                    // se asigna el valor seleccionado anteriormente en el nuevo dropdown
                    dayInput.value = dayInput.options.FindIndex(options => options.text == text4);
                    // Se valida si el año no es bisiesto y ya no vuelva a agregarse de nuevo
                    a4 = true;
                    a3 = false;
                    a2 = false;
                    a1 = false;
                }
            }
        }
        else
        {
            
            if (a2)
            {
                // NO SE AGREGA DE NUEVO EL CONTENIDO
            }
            else
            {
                // busca el valor seleccionado anteriormente
                string text1 = dayInput.captionText.text;
                // se limpia el dropdown
                dayInput.ClearOptions();
                // Rellenar todos los 31 dias
                dayInput.AddOptions(dayStringList31Days);
                // se asigna el valor seleccionado anteriormente en el nuevo dropdown
                dayInput.value = dayInput.options.FindIndex(options => options.text == text1);
                // Se valida si el mes que se selecciona tiene tambien 31 días ya no vuelva a agregarse los dias
                a2 = true;
                a1 = false;
                a3 = false;
                a4 = false;
            }
                
        }
        
    }    
    
    /// <summary>
    /// Funcionalidad para rellenar la lista de los meses al iniciar la aplicación
    /// </summary>
    public void RellenarMes()
    {
        monthInput.ClearOptions();
        // Rellenar todos los meses
        // Comenzamos desde 1 hasta 12
        for (int i = 1; i <= 12; i++)
        {
            listMonth.Add(i);
        }
        
        monthStringList = listMonth.ConvertAll<string>(x=>x.ToString());
        monthInput.AddOptions(monthStringList);
        
    }    
    
    /// <summary>
    /// Funcionalidad para rellenar la lista de los años al iniciar la aplicación
    /// </summary>
    public void RellenarAños()
    {
        yearInput.ClearOptions();
        // Rellenar todos los años
        // Comenzamos desde 1940 hasta el año actual
        for (int i = 1940; i <= DateTime.Today.Year; i++)
        {
            listYear.Add(i);
        }
        
        yearStringList = listYear.ConvertAll<string>(x=>x.ToString());
        //yearStringList = new List<string>(){"adad","baba"};
        yearInput.AddOptions(yearStringList);
    }    
    
   
}
