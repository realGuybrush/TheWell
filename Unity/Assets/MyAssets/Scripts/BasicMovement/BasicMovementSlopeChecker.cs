using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BasicMovement:MonoBehaviour
{
    public Vector3 colliderSize;
    public Vector3 colliderOffset;
    public Vector2 slopeNormalPerp;
    private float slopeDownAngle;
    private float slopeSideAngle = 0f;
    private float lastSlopeAngle;
    public float slopeCheckDistance = 0.1f;
    public float maxSlopeAngle = 60.0f;
    private bool isOnSlope;
    public bool canWalkOnSlope;
    public bool IsOnSlope()
    {
        return isOnSlope;
    }
    public bool BasicHandleSlope()
    {
        Vector2 checkPos = transform.position - new Vector3(0.0f, thisCollider.size.y/2.0f-thisCollider.offset.y);
        return (SlopeCheckVertical(checkPos));
    }
    private bool SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, whatIsGround);
        if (hit)
        {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;
            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeDownAngle != lastSlopeAngle)
            {
                isOnSlope = true;
            }
            lastSlopeAngle = slopeDownAngle;
            Debug.DrawRay(hit.point, slopeNormalPerp, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }
        if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
        {
            canWalkOnSlope = false;
        }
        else
        {
            canWalkOnSlope = true;
        }
        return isOnSlope;
    }
}
