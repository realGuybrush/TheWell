using UnityEngine;

public class Weapon : Item {

    [SerializeField]
    private Projectile projectilePrefab;

    public virtual void Attack(GameObject ignore, Vector3 projectileStartPos, Vector3 projectileDirection)
    {
        Projectile projectile = Instantiate(projectilePrefab,
            projectileStartPos, Quaternion.identity);
        projectile.Init(ignore, projectileDirection);
        Amount--;
        Destroy(projectile, projectile.LifeTime);
    }
}
