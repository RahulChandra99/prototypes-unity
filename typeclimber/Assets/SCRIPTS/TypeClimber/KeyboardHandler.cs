using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardHandler : MonoBehaviour
{
    // Start is called before the first frame update
    TouchScreenKeyboard keyboard;

    public TMPro.TextMeshProUGUI text;
    public TMPro.TextMeshProUGUI DebugText;
    public TMPro.TextMeshProUGUI HeightTxt;
    public RectTransform imagerectTransfrom;
    void Start()
    {
        OpenKeyboard();
    }

    public void OpenKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }

    // Update is called once per frame
    void Update()
    {
        if (text)
        {
           text.text =  keyboard.text;
        }

        if(keyboard.status == TouchScreenKeyboard.Status.Done)
        {
            DebugText.text = "keyboard Input is done";
            imagerectTransfrom.anchoredPosition = Vector2.Lerp(imagerectTransfrom.anchoredPosition, new Vector2(0, 10), .3f);
        }
        else

        if(keyboard.status == TouchScreenKeyboard.Status.Visible)
        {
            DebugText.text = "keyboard is Active";
            HeightTxt.text = "height is "+GetHeightofKeyboard().ToString();
            imagerectTransfrom.anchoredPosition =  Vector2.Lerp( imagerectTransfrom.anchoredPosition,new Vector2(0,GetHeightofKeyboard()+250),.3f);
        }
        else

        if (keyboard.status == TouchScreenKeyboard.Status.LostFocus)
        {
            DebugText.text = "keyboard Focus is lost";
            imagerectTransfrom.anchoredPosition = Vector2.Lerp(imagerectTransfrom.anchoredPosition, new Vector2(0, 0), .3f);
        }
        else

        if (keyboard.status == TouchScreenKeyboard.Status.Canceled)
        {
            DebugText.text = "keyboard is cancelled";
            imagerectTransfrom.anchoredPosition = Vector2.Lerp(imagerectTransfrom.anchoredPosition, new Vector2(0, 0), .3f);
        }


    }


    public float GetHeightofKeyboard()
    {
#if UNITY_ANDROID
        using (AndroidJavaClass UnityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject View = UnityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer").Call<AndroidJavaObject>("getView");

            using (AndroidJavaObject Rct = new AndroidJavaObject("android.graphics.Rect"))
            {
                View.Call("getWindowVisibleDisplayFrame", Rct);

                return Screen.height - Rct.Call<int>("height");
            }
        }
#elif UNITY_IOS
        return (int)TouchScreenKeyboard.area.height;
#elif UNITY_EDITOR
        return 15;
#endif
        return 0;
    }
}
