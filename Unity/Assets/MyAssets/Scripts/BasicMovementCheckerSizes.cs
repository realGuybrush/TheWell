using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BasicMovement : MonoBehaviour
{
    public Vector3 ledgeStart, ledgeSize, wallStart, wallSize, stepStart, stepSize, landStart, landSize;
    public void SetLandChecker()
    {
        landSize = new Vector2(thisCollider.bounds.size.x, thisCollider.bounds.size.y * 0.1f);
        landStart = new Vector2(thisCollider.bounds.center.x, thisCollider.bounds.center.y - thisCollider.bounds.extents.y);
        land = new EnvironmentChecker(thisObject, landStart, landSize, thisCollider.bounds.size.y * 0.1f, Vector2.down, whatIsGround);
    }
    public void SetStepChecker()
    {
        stepSize = new Vector2(thisCollider.bounds.size.y / 10.0f, thisCollider.bounds.size.y*0.2f );
        stepStart = new Vector2(thisCollider.bounds.center.x + thisCollider.bounds.extents.x + stepSize.x / 2, thisCollider.bounds.center.y - thisCollider.bounds.extents.y * 0.8f);
        step = new EnvironmentChecker(thisObject, stepStart, stepSize, thisCollider.bounds.size.y * 0.1f, facingRight? Vector2.right: Vector2.left, landLayer);
    }
    public void SetWallChecker()
    {
        wallSize = new Vector2(thisCollider.bounds.size.y / 10.0f, thisCollider.bounds.size.y*0.4f);
        wallStart = new Vector2(thisCollider.bounds.center.x + thisCollider.bounds.extents.x + wallSize.x / 2, thisCollider.bounds.center.y + thisCollider.bounds.extents.y * 0.1f);
        wall = new EnvironmentChecker(thisObject, wallStart, wallSize, thisCollider.bounds.size.y * 0.1f, facingRight ? Vector2.right : Vector2.left, landLayer);
    }
    public void SetLedgeChecker()
    {
        ledgeSize = new Vector2(thisCollider.bounds.size.y / 10.0f, thisCollider.bounds.size.y*0.4f);
        ledgeStart = new Vector2(thisCollider.bounds.center.x + thisCollider.bounds.extents.x + ledgeSize.x / 2, thisCollider.bounds.center.y + thisCollider.bounds.extents.y * 0.9f);
        ledge = new EnvironmentChecker(thisObject, ledgeStart, ledgeSize, thisCollider.bounds.size.y * 0.1f, facingRight ? Vector2.right : Vector2.left, landLayer);
    }
}
