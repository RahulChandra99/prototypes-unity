using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text coinScore_txt;

    public Image[] hearts;

    public Sprite redHeart;
    public Sprite blackHeart;

    public Text distCovered_txt;
    int dist;

    float TimeTaken;

    public GameObject resultPage;
    public Text distance_Result;
    public Text score_Result;
    public Text Time_Result;

    float dist_temp = 0f;



    private void Start()
    {
        //invokes the distance method after every 1/speed secs
        //as speed inc -> dist inc
        if (SoundManager.Instance.isStarted)
        {
            InvokeRepeating("DistanceManager", 0, 1 / PlayerController.moveSpeed);
        }
            
        

        resultPage.SetActive(false);
    }
    private void Update()
    {
        if (SoundManager.Instance.isStarted)
        {
            CoinManager();

            HealthManager();

            TimeManager();


        }

    }

    private void DistanceManager()
    {

        dist += 1;
        distCovered_txt.text = dist.ToString() + "m";
        dist_temp = dist;
    }

    private void CoinManager()
    {
        //Coin Score Update
        coinScore_txt.text = GameManager.Instance.coinScore.ToString();
    }

    private void HealthManager()
    {
        switch (GameManager.Instance.playerHealth)
        {

            case 0:

                StartCoroutine(Dead());

                break;
            case 1:
                hearts[0].sprite = hearts[1].sprite = blackHeart;
                break;

            case 2:
                hearts[0].sprite = blackHeart;
                break;

            default:
                hearts[0].sprite = hearts[1].sprite = hearts[2].sprite = redHeart;
                break;

        }
    }

    IEnumerator Dead()
    {
        SoundManager.Instance.isStarted = false;
        hearts[0].sprite = hearts[1].sprite = hearts[2].sprite = blackHeart;
        Time.timeScale = 0.2f;
        
        yield return new WaitForSeconds(0.7f);
        resultPage.SetActive(true);
        Time.timeScale = 0f;
        score_Result.text = GameManager.Instance.coinScore.ToString();


        distance_Result.text = dist_temp.ToString();
        float t = TimeTaken;
        Time_Result.text = t.ToString("F2");
    }

    private void TimeManager()
    {
        TimeTaken += Time.deltaTime;
        //Debug.Log(TimeTaken.ToString("F2"));
    }


}
