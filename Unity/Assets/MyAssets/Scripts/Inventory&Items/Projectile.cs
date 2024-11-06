using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{
    private GameObject ignore;
    private BasicMovement target;

    [SerializeField]
    private float _lifeTime = 400;

    [SerializeField]
    private float _damage = 5;

    [SerializeField]
    private float _speed = 5f;

    [SerializeField]
    private Rigidbody2D _thisBody;

    void Start()
    {
        _thisBody.velocity = transform.right * _speed;
    }

    public void Init(GameObject newIgnore, Vector3 movingDirection)
    {
        ignore = newIgnore;
        transform.right = movingDirection;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (_damage > 0)
        {
            if (collision.gameObject == ignore) return;
            target = collision.gameObject.GetComponent<BasicMovement>();
            if (target != null)
                _damage -= target.GetDamaged(_damage);
            if (_damage <= 0)
                Destroy(gameObject);
        }
    }

    public float lifeTime => _lifeTime;
}
