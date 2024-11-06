using UnityEngine;

public class ShootingWeapon : MonoBehaviour {

    [SerializeField]
    private Projectile projectilePrefab;

    public void Shoot(GameObject ignore, Vector3 projectileStartPos, Vector3 projectileDirection)
    {
        Projectile projectile = Instantiate(projectilePrefab,
            projectileStartPos, Quaternion.identity);
        projectile.Init(ignore, projectileDirection);
        Destroy(projectile, projectile.lifeTime);
    }
}
