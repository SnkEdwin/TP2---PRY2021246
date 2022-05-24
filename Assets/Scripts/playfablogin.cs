
using PlayFab; 

using PlayFab.ClientModels; 

using UnityEngine; 

using Unity; 

  

public class playfablogin : MonoBehaviour 

{ 

    private string userEmail; 

    private string userPassword; 

    private string userName; 

    public GameObject loginPanel; 

  

    public void Start() 

    { 

        //Note: Setting title Id here can be skipped if you have set the value in Editor Extensions already. 

        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        { 

            PlayFabSettings.TitleId = "2A8BF"; // Please change this value to your own titleId from PlayFab Game Manager 

        } 

        //var request = new LoginWithCustomIDRequest { CustomId = «GettingStartedGuide«, CreateAccount = true }; 

        //PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure); 

        if (PlayerPrefs.HasKey("EMAIL")) 

        { 

            userEmail = PlayerPrefs.GetString("EMAIL"); 

            userPassword = PlayerPrefs.GetString("PASSWORD"); 

            userName = PlayerPrefs.GetString("NAME"); 

            var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword }; 

            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure); 

        } 

    } 

  

    private void OnRegisterSuccess(RegisterPlayFabUserResult result) 

    { 

        Debug.Log("Enhorabuena, te registraste correctamente"); 

        PlayerPrefs.SetString("EMAIL", userEmail); 

        PlayerPrefs.SetString("PASSWORD", userPassword); 

        PlayerPrefs.SetString("NAME", userName); 

        loginPanel.SetActive(false); 

    } 

  

    private void OnRegisterFailure(PlayFabError error) 

    { 

        Debug.Log(error.GenerateErrorReport()); 

    } 

  

    private void OnLoginSuccess(LoginResult result) 

    { 

        Debug.Log("Congratulations, you made your first successful API call!"); 

        PlayerPrefs.SetString("EMAIL", userEmail); 

        PlayerPrefs.SetString("PASSWORD", userPassword); 

        PlayerPrefs.SetString("NAME", userName);

        Debug.Log(userName);
        Debug.Log(userEmail);
        Debug.Log(userPassword);
        
        loginPanel.SetActive(false); 

    } 

  

    private void OnLoginFailure(PlayFabError error) 

    { 

        var registerRequest = new RegisterPlayFabUserRequest { Email = userEmail, Password = userPassword, Username = userName }; 

        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFailure); 

    } 

  

    public void GetUserEmail(string emailIn) 

    { 

        userEmail = emailIn; 

    } 

  

    public void GetUserPassword(string passwordIn) 

    { 

        userPassword = passwordIn; 

    } 

  

    public void GetUserName(string nameIn) 

    { 

        userName = nameIn; 

    } 

  

    public void OnClickLogin() 

    { 

        var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword }; 

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure); 

    } 

} 