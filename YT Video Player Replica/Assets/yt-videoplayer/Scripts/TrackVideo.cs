using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class TrackVideo : MonoBehaviour, IPointerDownHandler, IPointerUpHandler , IDragHandler
{
    public VideoPlayer[] video;
    Slider tracking;
    bool slide = false;
    public RawImage previewImg;
    private void Start()
    {
        tracking = GetComponent<Slider>();
        previewImg.gameObject.SetActive(false);
    }



    private void Update()
    {
        if (!slide)
        {
            
            if (video[0].isPlaying || video[0].isPaused)
            {
               
                tracking.value = (float)video[0].frame / video[0].frameCount;

            }
           
            else if (video[1].isPlaying || video[1].isPaused)
            {
                
                tracking.value = (float)video[1].frame / video[1].frameCount;

            }
            else if (video[2].isPlaying || video[2].isPaused)
            {
                
                tracking.value = (float)video[2].frame / video[2].frameCount;

            }
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        slide = true;
    }

    

    public void OnPointerUp(PointerEventData eventData)
    {
       

        if (video[0].isPlaying || video[0].isPaused)
        {
            video[0].Play();
            previewImg.gameObject.SetActive(false);

            float frame = (float)tracking.value * (float)video[0].frameCount;
            video[0].frame = (long)frame;
        }   
        else if (video[1].isPlaying || video[1].isPaused)
        {
            video[1].Play();
            previewImg.gameObject.SetActive(false);

            float frame = (float)tracking.value * (float)video[1].frameCount;
            video[1].frame = (long)frame;
        }
        else if (video[2].isPlaying || video[2].isPaused)
        {
            video[2].Play();
            previewImg.gameObject.SetActive(false);

            float frame = (float)tracking.value * (float)video[2].frameCount;
            video[2].frame = (long)frame;
        }


        slide = false;

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (video[0].isPlaying || video[0].isPaused)
        {
            video[0].Pause();
            previewImg.texture = video[0].texture;
            
            previewImg.gameObject.SetActive(true);
            
            
        }
        else if (video[1].isPlaying || video[1].isPaused)
        {
            video[1].Pause();
            previewImg.texture = video[1].texture;
            
            previewImg.gameObject.SetActive(true);


        }
        else if (video[2].isPlaying || video[2].isPaused)
        {
            video[2].Pause();
            previewImg.texture = video[2].texture;
           
            previewImg.gameObject.SetActive(true);


        }

    }
}
