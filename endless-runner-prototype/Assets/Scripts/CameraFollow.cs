using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Vector3 offsetPos;

    private void Start()
    {
        offsetPos = transform.position - player.transform.position;
    }

    private void LateUpdate()
    {
        Vector3 newPos = new Vector3 (offsetPos.x + player.transform.position.x, transform.position.y, offsetPos.z + player.transform.position.z);
        
        transform.position = Vector3.Lerp(transform.position, newPos, 8 * Time.deltaTime);

    }
}
