using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class LOGIN : MonoBehaviour
{
    public GameObject username;
    public GameObject password;
    public GameObject UsernameError;
    public GameObject PasswordError;
    private string Username;
    private string Password;
    private string[] Lines;
    private string DecryptedPass;
    bool UN = false;
    bool PW = false;


    void Start()
    {
        UsernameError.SetActive(false);
        PasswordError.SetActive(false);
    }
    public void LogInButton()
    {
   
        if (Username != "")
        {
            if (System.IO.File.Exists(@"C:/LOGIN DATA/" + Username + ".txt"))
            {
                UN = true;
                Lines = System.IO.File.ReadAllLines(@"C:/LOGIN DATA/" + Username + ".txt");
            } else
            {
                Debug.LogWarning("Username Invalid");
                Error();
            }

        } else
        {
            Debug.LogWarning("Username field is empty");
            Error();
        }
        if (Password != "")
        {
            if (System.IO.File.Exists(@"C:/LOGIN DATA/" + Username + ".txt"))
            {
                int i = 1;
                foreach (char c in Lines[2])
                {

                    i++;
                    char Decrypted = (char)(c / i);
                    DecryptedPass += Decrypted.ToString();
                }

                if (Password == DecryptedPass)
                {
                    PW = true;
                } else
                {
                    Debug.LogWarning("Password Incorrect");
                    Error();
                }
            } else
            {
                Debug.LogWarning("Password Incorrect");
                Error();
            }
        } else
        {
            Debug.LogWarning("Password Field is empty");
            Error();
        }
        if (UN == true && PW == true)
        {
            username.GetComponent<InputField>().text = "";
            password.GetComponent<InputField>().text = "";
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (Password != "" && Username != "")
            {
                LogInButton();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (username.GetComponent<InputField>().isFocused)
            {
                password.GetComponent<InputField>().Select();
            }
            if (password.GetComponent<InputField>().isFocused)
            {
                username.GetComponent<InputField>().Select();
            }
        }

        Username = username.GetComponent<InputField>().text;
        Password = password.GetComponent<InputField>().text;
    }
    public void Error()
    {
        if (UN == false)
        {
            UsernameError.SetActive(true);
        }
        if (PW == false)
        {
            PasswordError.SetActive(true);
        }
    }
    public void ForgotPassword()
    {
        if (Username != "")
        {
            if (System.IO.File.Exists(@"C:/LOGIN DATA/" + Username + ".txt"))
            {
                UN = true;
                Lines = System.IO.File.ReadAllLines(@"C:/LOGIN DATA/" + Username + ".txt");
            }
            else
            {
                Debug.LogWarning("Username Invalid");
                Error();
            }
        }
        else
        {

        }
    }
    
}
