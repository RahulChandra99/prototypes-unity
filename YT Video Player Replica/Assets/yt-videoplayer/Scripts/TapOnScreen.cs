using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class TapOnScreen : MonoBehaviour , IPointerClickHandler
{
    private float lastTimeClick;
    public GameObject[] videos;

    public void OnPointerClick(PointerEventData eventData)
    {
        float currentTimeClick = eventData.clickTime;
        if (Mathf.Abs(currentTimeClick - lastTimeClick) < 0.75f)
        {
            Debug.Log("inside");


            if (Input.mousePosition.x < Screen.width / 2)
            {

                Debug.Log("left");
                if (videos[0].activeInHierarchy)
                {
                    videos[0].GetComponent<VideoPlayer>().time -= 5f;
                }

                else if (videos[1].activeInHierarchy)
                {
                    videos[1].GetComponent<VideoPlayer>().time -= 5f;
                }
                if (videos[2].activeInHierarchy)
                {
                    videos[2].GetComponent<VideoPlayer>().time -= 5f;
                }
            }
            else if (Input.mousePosition.x > Screen.width / 2)

            {
                Debug.Log("right");

                if (videos[0].activeInHierarchy)
                {
                    videos[0].GetComponent<VideoPlayer>().time += 5f;
                }

                else if (videos[1].activeInHierarchy)
                {
                    videos[1].GetComponent<VideoPlayer>().time += 5f;
                }
                if (videos[2].activeInHierarchy)
                {
                    videos[2].GetComponent<VideoPlayer>().time += 5f;
                }
            }
        }
        lastTimeClick = currentTimeClick;

    }
}
