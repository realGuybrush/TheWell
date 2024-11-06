using UnityEngine;

public class ShootingHandler : MonoBehaviour
{
    public Transform shootingCenter;

    [SerializeField]
    private ShootingWeapon _shootingWeapon;

    //todo: move all this into gun

    protected virtual void Shoot()
    {
        //anim.SetVar("Shoot", true);
        _shootingWeapon.Shoot(gameObject, GetCenterOfShootPartRotation(), GetShootingDirection());
    }

    protected Vector3 GetCenterOfShootPartRotation()
    {
        return shootingCenter.position;
    }

    private Vector3 GetShootingDirection()
    {
        //fix: return mainCamera, or fix somehow otherwise
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition),
            centerPos = GetCenterOfShootPartRotation();
        return new Vector3(mousePos.x - centerPos.x, mousePos.y - centerPos.y);
    }
}
