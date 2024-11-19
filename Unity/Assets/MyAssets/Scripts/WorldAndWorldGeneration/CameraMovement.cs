using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 cameraStart;
    private Vector3 smoothOffset;
    private float  smoothTimer, smoothMovementDelay, smoothMovementTime;

    [SerializeField]
    private Camera myCamera;

    [SerializeField]
    private Transform player;

    private void Start()
    {
        cameraStart = transform.position - player.position;
    }

    private void Update()
    {
        UpdateSmoothMovementDelayTimer();
    }

    private void UpdateSmoothMovementDelayTimer()
    {
        if (smoothMovementDelay <= 0)
        {
            UpdateSmoothMovementTimer();
        } else
            smoothMovementDelay -= Time.deltaTime;
    }

    private void UpdateSmoothMovementTimer()
    {
        if (smoothTimer <= 0)
            transform.position = new Vector3(player.position.x + cameraStart.x,
                                             player.position.y + cameraStart.y,
                                             transform.position.z);
        else
        {
            smoothTimer -= Time.deltaTime;
            transform.position += smoothOffset * (Time.deltaTime / smoothMovementTime);
        }
    }

    public void StartSmoothMovement(Vector2 moveOffset, float time, float delay)
    {
        smoothOffset = moveOffset;
        smoothMovementDelay = delay;
        smoothMovementTime = time - delay;
        smoothTimer = smoothMovementTime;
    }

    public Vector3 ScreenToWorldPoint(Vector3 position)
    {
        return myCamera.ScreenToWorldPoint(position);
    }
}
