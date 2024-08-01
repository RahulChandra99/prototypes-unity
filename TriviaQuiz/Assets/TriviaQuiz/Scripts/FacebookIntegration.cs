using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using TMPro;
using UnityEngine.SceneManagement;

public class FacebookIntegration : MonoBehaviour
{
    public TextMeshProUGUI displayText;
    public string myName;
    public static FacebookIntegration Instance;
     void Awake()
 {
     if (Instance == null)
     {
         Instance = this;
     }
     else if (Instance != null)
     {
         Debug.Log("Instance already exists, destroying object!");
         Destroy(this);
     }
     
     
     
     if (!FB.IsInitialized)
     {
         // Initialize the Facebook SDK
         FB.Init(InitCallback, OnHideUnity);
     }
     else
     {
         // Already initialized, signal an app activation App Event
         FB.ActivateApp();
     }
 
         
     
 }
 
      void Start() {
          if (FB.IsLoggedIn)
          {
              //fbButton.SetActive(false);
              displayText.text = "Login Successful~";
          }
          else
          {
          //fbButton.SetActive(true);
 
          }
 
 
      }
 
     private void InitCallback()
 {
     if (FB.IsInitialized)
     {
         // Signal an app activation App Event
         FB.ActivateApp();
         // Continue with Facebook SDK
         // ...
     }
     else
     {
         Debug.Log("Failed to Initialize the Facebook SDK");
     }
 }
 
 private void OnHideUnity(bool isGameShown)
 {
     if (!isGameShown)
     {
         // Pause the game - we will need to hide
         Time.timeScale = 0;
     }
     else
     {
         // Resume the game - we're getting focus again
         Time.timeScale = 1;
     }
 }
     public void loginFunction()
     {
         if (FB.IsLoggedIn)
         {
             Debug.Log("User already logged in!");
         }
         else
         {
           
             var perms = new List<string>() { "public_profile", "email", "user_friends" };
             FB.LogInWithReadPermissions(perms, AuthCallback);
             
         }
     }
 
 private void AuthCallback(ILoginResult result)
     {
         if (FB.IsLoggedIn)
         {
             
             // AccessToken class will have session details
             var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
             // Print current access token's User ID
             Debug.Log(aToken.UserId);
             // Print current access token's granted permissions
             foreach (string perm in aToken.Permissions)
             {
                 Debug.Log(perm);
             }
             FB.API("/me?fields=first_name", HttpMethod.GET, CallbackData);
             //fbButton.SetActive(false);
 
         }
         else
         {
             //fbButton.SetActive(true);
             Debug.Log("User cancelled login");
         }
     }
 
 void CallbackData(IResult res)
 {
     if (res.Error != null)
     {
         Debug.Log("Error getting data");
     }
     else
     {
         displayText.text = "  You are now Logged in " + res.ResultDictionary["first_name"];
         myName = res.ResultDictionary["first_name"].ToString();
         Invoke("LoadGame", 2f);
     }
 }
 
 void LoadGame()
 {
     SceneManager.LoadSceneAsync("GameScene");
 }
}
