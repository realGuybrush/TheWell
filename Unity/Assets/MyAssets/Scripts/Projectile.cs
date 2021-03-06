using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject ignore;
    public int lifeTime = 100;
    public int atk = 5;
    public float velocity = 5f;
    public Rigidbody2D thisBody;

    void Start()
    {
        Physics2D.IgnoreCollision(this.gameObject.GetComponent<Collider2D>(), ignore.GetComponent<Collider2D>(), true);
        thisBody.velocity = this.transform.right * velocity;
    }

    void Update()
    {
        lifeTime--;
        if (lifeTime < 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void Init(GameObject newIgnore, Vector3 movingDirection)
    {
        ignore = newIgnore;
        transform.right = movingDirection;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int i1=0;
        if (atk > 0)
        {
            if ((collision.gameObject.layer == 8) || (collision.gameObject.layer == 9) || (collision.gameObject.layer == 12) || collision.gameObject == ignore)
            { return; }
            if (collision.gameObject.GetComponent<Health>() != null)
            {
                i1 = collision.gameObject.GetComponent<Health>().HealthAmount();
                collision.gameObject.GetComponent<Health>().Substract(atk);
            }
            if (collision.gameObject.GetComponent<BasicMovement>() != null)
            {
                collision.gameObject.GetComponent<BasicMovement>().thisHealth.Substract(atk);
            }
            atk -= i1;
            if (atk <= 0)
                Destroy(this.gameObject);
        }
    }
}
