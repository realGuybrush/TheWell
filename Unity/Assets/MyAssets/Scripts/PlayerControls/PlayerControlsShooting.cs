using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerControls : BasicMovement
{
    public override void Shoot()
    {
        anim.SetVar("Aiming", true);
        WM.Shoot(this.gameObject, (Vector3)GetMousePositionInRange(0.5f, GetCenterOfShootPartRotation()), GetShootingDirection(), projectileIndex);
    }
}
