using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;


public class REGISTER : MonoBehaviour
{
    public GameObject username;
    public GameObject email;
    public GameObject password;
    public GameObject confPassword;
    public GameObject EmailPopup;
    public GameObject UsernamePopup;
    public GameObject PasswordPopup;
    public GameObject ConfPassPopup;
    public GameObject CompletePopup;
    private string Username;
    private string Email;
    private string Password;
    private string ConfPassword;
    private string form;
    private bool EmailValid = false;
    bool UN = false;
    bool EM = false;
    bool PW = false;
    bool CP = false;
    private string[] Characters = {"a","b","c","d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", 
                                   "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                                   "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "_"};

    
    void Start()
    {
        EmailPopup.SetActive(false);
        UsernamePopup.SetActive(false);
        PasswordPopup.SetActive(false);
        ConfPassPopup.SetActive(false);
        CompletePopup.SetActive(false);
    }
    public void RegisterButton()
    {
        

        if (Username != "")
        {
            if (!System.IO.File.Exists(@"C:/LOGIN DATA/" + Username + ".txt"))
            {
                UN = true;
            }
            else
            {
                Debug.LogWarning("Username Taken");
                Error();
            }
        }
        else
        {
            Debug.LogWarning("Username Field is empty");
            Error();
        }
        if (Email != "")
        {
            EmailValidation();
            if (EmailValid)
            {
                if (Email.Contains("@"))
                {
                    if (Email.Contains("."))
                    {
                        EM = true;
                    } else
                    {
                        Debug.LogWarning("Email is Incorrect");
                        Error();
                    }
                } else
                {
                    Debug.LogWarning("email is Incorrect");
                    Error();
                }
            }else
            {
                Debug.LogWarning("Email is Incorrect");
                Error();
            }
        } else
        {
            Debug.LogWarning("Email field is empty");
            Error();
        }
     
        if (Password != "")
        {
            if (Password.Length > 5)
            {
                PW = true;
            }
            else
            {
                Debug.LogWarning("Password must be atleast 6 characters long");
                Error();
            }
        } else
        {
            Debug.LogWarning("Password field is empty");
            Error();
        }

        if (ConfPassword != "")
        {
            if (ConfPassword == Password)
            {
                CP = true;
            } else
            {
                Debug.LogWarning("Passwords dont match");
                Error();
            }
        } else
        {
            Debug.LogWarning("Confirm Password field is empty");
            Error();
        }
        if (UN == true && EM == true && PW == true && CP == true)
        {
            
            bool Clear = true;
            int i = 1;
            foreach(char c in Password)
            {
                if (Clear)
                {
                    Password = "";
                    Clear = false;
                }
                i++;
                char Encrypted = (char)(c * i);
                Password += Encrypted.ToString();
            }
            form = (Username + "\n" + Email + "\n" + Password);
            System.IO.File.WriteAllText(@"C:/LOGIN DATA/" + Username + ".txt", form);
            username.GetComponent<InputField>().text = "";
            email.GetComponent<InputField>().text = "";
            password.GetComponent<InputField>().text = "";
            confPassword.GetComponent<InputField>().text = "";
            print("registration complete");
            CompletePopup.SetActive(true);
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))

        {
            if (username.GetComponent<InputField>().isFocused)
            {
                email.GetComponent<InputField>().Select();
            }
            if (email.GetComponent<InputField>().isFocused)
            {
                password.GetComponent<InputField>().Select();
            }
            if (password.GetComponent<InputField>().isFocused)
            {
                confPassword.GetComponent<InputField>().Select();
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (Password != "" && Email != "" && Username != "" && ConfPassword != "")
            {
                RegisterButton();
            }
        }

        Username = username.GetComponent<InputField>().text;
        Email = email.GetComponent<InputField>().text;
        Password = password.GetComponent<InputField>().text;
        ConfPassword = confPassword.GetComponent<InputField>().text;
    }

    void EmailValidation()
    {
        bool SW = false;
        bool EW = false;

        for(int i = 0;i<Characters.Length;i++)
        {
            if (Email.StartsWith(Characters[i]))
            {
                SW = true;
            }
        }
        for (int i = 0; i < Characters.Length; i++)
        {
            if (Email.EndsWith(Characters[i]))
            {
                EW = true;
            }
        }
        if (SW == true && EW == true)
        {
            EmailValid = true;
        }
        else
        {
            EmailValid = false;
        }
    }
    public void Error()
    {
        if (EM == false)
        {
            EmailPopup.SetActive(true);
        }
        if (UN == false)
        {
            UsernamePopup.SetActive(true);
        }
        if (PW == false)
        {
            PasswordPopup.SetActive(true);
        }
        if (CP == false)
        {
            ConfPassPopup.SetActive(true);
        }
       
  
    }
}
