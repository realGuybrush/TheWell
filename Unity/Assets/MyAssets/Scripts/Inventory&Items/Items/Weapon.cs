using UnityEngine;

public class Weapon : Item
{
    [SerializeField]
    private Projectile projectilePrefab;

    public virtual void Attack(GameObject ignore, Vector3 projectileDirection)
    {
        Projectile projectile = Instantiate(projectilePrefab,
            transform.position, Quaternion.identity);
        projectile.Init(ignore, projectileDirection);
        Amount--;
        Destroy(projectile, projectile.LifeTime);
    }
}
