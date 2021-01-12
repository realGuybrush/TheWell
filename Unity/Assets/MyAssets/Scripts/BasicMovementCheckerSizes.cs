using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BasicMovement : MonoBehaviour
{
    public void SetLandChecker()
    {
        Vector2 bottom = new Vector2(thisCollider.bounds.center.x, thisCollider.bounds.center.y - thisCollider.bounds.extents.y);
        Vector2 size = new Vector2(thisCollider.bounds.size.x, thisCollider.bounds.size.y*0.1f);
        land = new EnvironmentChecker(thisObject, bottom, size, size.y, Vector2.down, whatIsGround);
    }
    public void SetStepChecker()
    {
        Vector2 bottom = new Vector2(thisCollider.bounds.center.x, thisCollider.bounds.center.y - thisCollider.bounds.extents.y*0.8f);
        Vector2 size = new Vector2(thisCollider.bounds.size.y / 10.0f, thisCollider.bounds.size.y*0.2f );
        step = new EnvironmentChecker(thisObject, bottom, size, thisCollider.bounds.extents.x+size.x, facingRight? Vector2.right: Vector2.left, landLayer, 0, false);
    }
    public void SetWallChecker()
    {
        Vector2 middle = new Vector2(thisCollider.bounds.center.x, thisCollider.bounds.center.y+ thisCollider.bounds.extents.y * 0.1f);
        Vector2 size = new Vector2(thisCollider.bounds.size.y / 10.0f, thisCollider.bounds.size.y*0.6f);
        wall = new EnvironmentChecker(thisObject, middle, size, thisCollider.bounds.extents.x + size.x, facingRight ? Vector2.right : Vector2.left, landLayer, 0, false);
    }
    public void SetLedgeChecker()
    {
        Vector2 top = new Vector2(thisCollider.bounds.center.x, thisCollider.bounds.center.y + thisCollider.bounds.extents.y * 1.2f);
        Vector2 size = new Vector2(thisCollider.bounds.size.y / 10.0f, thisCollider.bounds.size.y*0.4f);
        ledge = new EnvironmentChecker(thisObject, top, size, thisCollider.bounds.extents.x + size.x, facingRight ? Vector2.right : Vector2.left, landLayer, 100000, false, true);
    }
}
