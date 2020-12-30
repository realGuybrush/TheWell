using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    private Vector3 cameraStart;

    public Transform player;
    
    private void Start()
    {
        cameraStart = transform.position - player.position;
    }
    
    private void Update()
    {
        transform.position = new Vector3(player.position.x + cameraStart.x, 
            player.position.y + cameraStart.y, transform.position.z);
    }
}
