using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayfabManager : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public TMP_InputField email;
    public TMP_InputField password;

    public void RegisterBtn()
    {
        if (password.text.Length < 6)
        {
            messageText.text = "Password length must be greater than 6 character";
            return;
        }
        var request = new RegisterPlayFabUserRequest
        {
            Email = email.text,
            Password = password.text,
            RequireBothUsernameAndEmail = false

        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);

        email.text = "";
        password.text = "";
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text = "User Registered , Login with your details";
    }

    void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;
        
    }
    
    public void LoginBtn()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = email.text,
            Password = password.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess,OnError);
        
        
    }

    void OnLoginSuccess(LoginResult result)
    {
        email.text = "";
        password.text = "";
        messageText.text = "Login Successful";
        Invoke("LoadGame", 1.5f);
    }

    void LoadGame()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }

    public void ResetPasswordBtn()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = email.text,
            TitleId = "FACE0"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        messageText.text = "Password Reset mail sent!";
        
        email.text = "";
        
    }
}
