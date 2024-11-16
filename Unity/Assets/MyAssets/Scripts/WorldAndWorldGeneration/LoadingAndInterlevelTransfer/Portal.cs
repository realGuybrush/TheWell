using UnityEngine;

public class Portal: MonoBehaviour
{
    [SerializeField]
    private Vector2Int direction;

    public void SetScaleAndPosition(Vector2 newPosition, Vector2 newSize)
    {
        transform.position = newPosition;
        transform.localScale = newSize;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<BasicMovement>() != null)
        {
            MapManager.Instance.TransferToOtherLevelEntrance(direction, other.gameObject);
            if (other.gameObject.tag == "Player")
                MapManager.Instance.LoadLevel(direction);
        }
    }
}
