using UnityEngine;

public class CoinRotate : MonoBehaviour
{
    public float rotateSpeed = 50f;
    

    private void Start()
    {
        
    }

    private void Update()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Increase Coin Score
            GameManager.Instance.coinScore++;
            Debug.Log("Coin:" + GameManager.Instance.coinScore);

            //destroy + particle effect + sound play
            Destroy(this.gameObject);

        }
    }

   
}
