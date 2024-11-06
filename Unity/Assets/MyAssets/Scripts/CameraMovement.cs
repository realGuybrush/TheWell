using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 cameraStart;
    private float smoothXOffset, smoothYOffset, smoothTimer, smoothTimerDelay, smoothMovementTime;

    [SerializeField]
    private Transform player;

    private void Start()
    {
        cameraStart = transform.position - player.position;
    }

    private void Update()
    {
        if (smoothTimerDelay <= 0)
        {
            if (smoothTimer <= 0)
            {
                transform.position = new Vector3(player.position.x + cameraStart.x,
                    player.position.y + cameraStart.y, transform.position.z);
            } else
            {
                smoothTimer -= Time.deltaTime;
                transform.position += new Vector3(smoothXOffset, smoothYOffset) * (Time.deltaTime / smoothMovementTime);
            }
        } else
            smoothTimerDelay -= Time.deltaTime;
    }

    public void StartSmoothMovement(float x, float y, float time, float delay)
    {
        smoothXOffset = x;
        smoothYOffset = y;
        smoothTimerDelay = delay;
        smoothMovementTime = time - delay;
        smoothTimer = smoothMovementTime;
    }
}
