using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;



public class VideoPlayerManager : MonoBehaviour
{
    public List<GameObject> videos = new List<GameObject>();


    [Header("PlayBack Buttons")]
    public Button nextBtn;
    public Button prevBtn;
    public Button playBtn;
    public Button pauseBtn;

    public Text currentTime;
    public Text totalTime;

    public Text title;
    public Text numberVideo;


    private void Awake()
    {


        videos[0].SetActive(true);
        videos[1].SetActive(false);
        videos[2].SetActive(false);

        pauseBtn.gameObject.SetActive(true);
        playBtn.gameObject.SetActive(false);

    }

    private void Update()
    {
        GreyOutButtons();

        ForTimer();



    }



    private void ForTimer()
    {

        if (videos[0].activeInHierarchy)
        {
            numberVideo.text = "1";
            title.text = "Video1_title";

            string minutes = Mathf.Floor((int)videos[0].GetComponent<VideoPlayer>().time / 60).ToString("00");
            string seconds = ((int)videos[0].GetComponent<VideoPlayer>().time % 60).ToString("00");
            currentTime.text = minutes + ":" + seconds;

            string minutes1 = Mathf.Floor((int)videos[0].GetComponent<VideoPlayer>().length / 60).ToString("00");
            string seconds1 = ((int)videos[0].GetComponent<VideoPlayer>().length % 60).ToString("00");
            totalTime.text = minutes1 + ":" + seconds1;

            

        }

        else if (videos[1].activeInHierarchy)
        {
            numberVideo.text = "2";

            title.text = "Video2_title";

            string minutes = Mathf.Floor((int)videos[1].GetComponent<VideoPlayer>().time / 60).ToString("00");
            string seconds = ((int)videos[1].GetComponent<VideoPlayer>().time % 60).ToString("00");
            currentTime.text = minutes + ":" + seconds;

            string minutes1 = Mathf.Floor((int)videos[1].GetComponent<VideoPlayer>().length / 60).ToString("00");
            string seconds1 = ((int)videos[1].GetComponent<VideoPlayer>().length % 60).ToString("00");
            totalTime.text = minutes1 + ":" + seconds1;
        }
        if (videos[2].activeInHierarchy)
        {
            numberVideo.text = "3";

            title.text = "Video3_title";

            string minutes = Mathf.Floor((int)videos[2].GetComponent<VideoPlayer>().time / 60).ToString("00");
            string seconds = ((int)videos[2].GetComponent<VideoPlayer>().time % 60).ToString("00");
            currentTime.text = minutes + ":" + seconds;

            string minutes1 = Mathf.Floor((int)videos[2].GetComponent<VideoPlayer>().length / 60).ToString("00");
            string seconds1 = ((int)videos[2].GetComponent<VideoPlayer>().length % 60).ToString("00");
            totalTime.text = minutes1 + ":" + seconds1;
        }
    }

    private void GreyOutButtons()
    {
        if (videos[2].activeInHierarchy)
        {
            ColorBlock nextColor = nextBtn.colors;
            Debug.Log("last avtive - grey out");
            //grey out the button when no video present
            nextColor.normalColor = new Color32(255, 255, 255, 87);
            nextColor.highlightedColor = new Color32(255, 255, 255, 87);
            nextColor.pressedColor = new Color32(255, 255, 255, 87);
            nextColor.selectedColor = new Color32(255, 255, 255, 87);
            nextBtn.colors = nextColor;


        }

        else if (videos[0].activeInHierarchy)
        {
            ColorBlock prevColor = prevBtn.colors;

            //grey out the button when no video present
            prevColor.normalColor = new Color32(255, 255, 255, 87);
            prevColor.highlightedColor = new Color32(255, 255, 255, 87);
            prevColor.pressedColor = new Color32(255, 255, 255, 87);
            prevColor.selectedColor = new Color32(255, 255, 255, 87);
            prevBtn.colors = prevColor;

        }

        else
        {
            ColorBlock nextColor = nextBtn.colors;
            nextColor.highlightedColor = new Color32(255, 255, 255, 255);
            nextColor.normalColor = new Color32(255, 255, 255, 255);
            nextColor.pressedColor = new Color32(255, 255, 255, 255);
            nextColor.selectedColor = new Color32(255, 255, 255, 255);
            nextBtn.colors = nextColor;

            ColorBlock prevColor = prevBtn.colors;
            prevColor.highlightedColor = new Color32(255, 255, 255, 255);
            prevColor.normalColor = new Color32(255, 255, 255, 255);
            prevColor.pressedColor = new Color32(255, 255, 255, 255);
            prevColor.selectedColor = new Color32(255, 255, 255, 255);
            prevBtn.colors = prevColor;
        }
    }

    public void Play()
    {
        if (videos[0].activeInHierarchy)
        {
            videos[0].GetComponent<VideoPlayer>().Play();
        }

        else if (videos[1].activeInHierarchy)
        {
            videos[1].GetComponent<VideoPlayer>().Play();

        }
        if (videos[2].activeInHierarchy)
        {
            videos[2].GetComponent<VideoPlayer>().Play();

        }

        pauseBtn.gameObject.SetActive(true);
        playBtn.gameObject.SetActive(false);
    }

    public void Pause()
    {
        if (videos[0].activeInHierarchy)
        {
            videos[0].GetComponent<VideoPlayer>().Pause();
        }

        else if (videos[1].activeInHierarchy)
        {
            videos[1].GetComponent<VideoPlayer>().Pause();

        }
        if (videos[2].activeInHierarchy)
        {
            videos[2].GetComponent<VideoPlayer>().Pause();

        }

        playBtn.gameObject.SetActive(true);
        pauseBtn.gameObject.SetActive(false);
    }

    public void NextBtn()
    {
        pauseBtn.gameObject.SetActive(true);
        playBtn.gameObject.SetActive(false);

        if (videos[0].activeInHierarchy)
        {

            videos[1].SetActive(true);

            videos[0].SetActive(false);
            videos[2].SetActive(false);
        }

        else if (videos[1].activeInHierarchy)
        {
            videos[2].SetActive(true);

            videos[1].SetActive(false);
            videos[0].SetActive(false);
        }
        if (videos[2].activeInHierarchy)
        {

        }
    }

    public void PrevBtn()
    {
        pauseBtn.gameObject.SetActive(true);
        playBtn.gameObject.SetActive(false);

        if (videos[2].activeInHierarchy)
        {

            videos[1].SetActive(true);

            videos[0].SetActive(false);
            videos[2].SetActive(false);
        }

        else if (videos[1].activeInHierarchy)
        {

            videos[0].SetActive(true);

            videos[1].SetActive(false);
            videos[2].SetActive(false);
        }
        if (videos[0].activeInHierarchy)
        {

        }
    }




}


