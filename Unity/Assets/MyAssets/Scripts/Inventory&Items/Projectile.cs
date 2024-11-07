using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{
    private GameObject ignore;
    private BasicMovement target;

    [SerializeField]
    private float lifeTime = 400;

    [SerializeField]
    private float damage = 5;

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private Rigidbody2D thisBody;

    void Start()
    {
        thisBody.velocity = transform.right * speed;
    }

    public void Init(GameObject newIgnore, Vector3 movingDirection)
    {
        ignore = newIgnore;
        transform.right = movingDirection;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (damage > 0)
        {
            if (collision.gameObject == ignore) return;
            target = collision.gameObject.GetComponent<BasicMovement>();
            if (target != null)
                damage -= target.GetDamaged(damage);
            if (damage <= 0)
                Destroy(gameObject);
        }
    }

    public float LifeTime => lifeTime;
}
