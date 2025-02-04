using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public GameObject platformPrefab1;
    public GameObject platformPrefab2;
    public int poolSize = 5;
    public float minPlatformLength = 8f;
    public float maxPlatformLength = 12f;
    public float minYOffset = -1f;
    public float maxYOffset = 1f;
    public float minXOffset = -5f;
    public float maxXOffset = 5f;

    private List<GameObject> platformPool;
    private float nextSpawnZ = 0f;

    void Start()
    {
        platformPool = new List<GameObject>();

        GameObject firstPlatform = Instantiate(platformPrefab1, new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform);
        platformPool.Add(firstPlatform);
        firstPlatform.SetActive(true);

        nextSpawnZ = Random.Range(minPlatformLength, maxPlatformLength);

        for (int i = 1; i < poolSize; i++)
        {
            float randomXOffset = Random.Range(minXOffset, maxXOffset);
            float randomYOffset = Random.Range(minYOffset, maxYOffset);
            float randomPlatformLength = Random.Range(minPlatformLength, maxPlatformLength);

            GameObject platformPrefab = (i < 3) ? platformPrefab1 : RandomizePlatformPrefab();

            Quaternion rotation = Quaternion.identity;
            if (platformPrefab == platformPrefab2)
            {
                float randomYRotation = Random.Range(0f, 360f);
                rotation = Quaternion.Euler(0f, randomYRotation, 0f);
            }

            GameObject platform = Instantiate(platformPrefab, new Vector3(randomXOffset, randomYOffset, nextSpawnZ), rotation, this.transform);
            platformPool.Add(platform);
            nextSpawnZ += randomPlatformLength;
            platform.SetActive(true);
        }
    }

    void Update()
    {
        for (int i = 0; i < platformPool.Count; i++)
        {
            if (!platformPool[i].activeInHierarchy)
            {
                ReusePlatform(platformPool[i]);
            }
        }
    }

    void ReusePlatform(GameObject platform)
    {
        float randomXOffset = Random.Range(minXOffset, maxXOffset);
        float randomYOffset = Random.Range(minYOffset, maxYOffset);
        float randomPlatformLength = Random.Range(minPlatformLength, maxPlatformLength);

        GameObject platformPrefab = RandomizePlatformPrefab();

        Quaternion rotation = Quaternion.identity;
        if (platformPrefab == platformPrefab2)
        {
            float randomYRotation = Random.Range(0f, 360f);
            rotation = Quaternion.Euler(0f, randomYRotation, 0f);
        }

        platform.transform.position = new Vector3(randomXOffset, randomYOffset, nextSpawnZ);
        platform.transform.rotation = rotation;
        nextSpawnZ += randomPlatformLength;

        Platform platformScript = platform.GetComponent<Platform>();
        if (platformScript != null)
        {
            platformScript.bounceValue = UnityEngine.Random.Range(1, 6);
            platformScript.bounceValue_txt.text = platformScript.bounceValue.ToString();
        }

        platform.SetActive(true);
    }

    GameObject RandomizePlatformPrefab()
    {
        return (UnityEngine.Random.Range(0, 2) == 0) ? platformPrefab1 : platformPrefab2;
    }
}
