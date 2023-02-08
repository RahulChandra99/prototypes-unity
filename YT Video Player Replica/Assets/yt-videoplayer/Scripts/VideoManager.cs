using UnityEngine;

public class VideoManager : MonoBehaviour
{
    public GameObject videoPlayerPanel;


    private void Awake()
    {
        videoPlayerPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.touchCount != 0 || Input.GetMouseButtonDown(0))
        {
            videoPlayerPanel.SetActive(true);
            CancelInvoke("HideUI");
        }
        else
        {
            Invoke("HideUI", 6);

        }

       
    }

    void HideUI()
    {
        videoPlayerPanel.SetActive(false);
    }

}
