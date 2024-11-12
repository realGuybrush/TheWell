using UnityEngine;

public class Portal: MonoBehaviour
{
    [SerializeField]
    private Vector2 direction;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<BasicMovement>() != null)
        {
            if (other.gameObject.tag == "Player")
                WorldManager.Instance.LoadLevel(direction);
            WorldManager.Instance.TransferTo(direction, other.gameObject);
        }
    }
}
