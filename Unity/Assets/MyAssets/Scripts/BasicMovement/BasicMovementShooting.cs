using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BasicMovement : MonoBehaviour
{
    public Transform shootingCenter;
    public int projectileIndex=0;

    public void SetProjectileIndex(int index)
    {
        projectileIndex = index;
    }

    public virtual void Shoot()
    {
        anim.SetVar("Shoot", true);
        WM.Shoot(this.gameObject, GetCenterOfShootPartRotation(), GetShootingDirection(), projectileIndex);
    }

    public virtual Vector3 GetCenterOfShootPartRotation()
    {
        Vector3 cent;
        cent = shootingCenter.position;
        return cent;
    }

    public Vector3 GetShootingPosition()
    {
        return GetCenterOfShootPartRotation();
    }

    public Vector3 GetShootingDirection()
    {
        Vector3 dir, mousePos = WM.mainCamera.ScreenToWorldPoint(Input.mousePosition), centerPos = GetCenterOfShootPartRotation();
        dir = new Vector3(mousePos.x - centerPos.x, mousePos.y - centerPos.y);
        return dir;
    }
}
