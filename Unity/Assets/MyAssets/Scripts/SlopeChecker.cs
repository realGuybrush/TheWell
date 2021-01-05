using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeChecker
{
    public PhysicsMaterial2D fullFriction;
    public PhysicsMaterial2D normFriction;
    public Vector3 colliderSize;
    public Vector3 colliderOffset;
    public Vector2 slopeNormalPerp;
    private float slopeDownAngle;
    private float slopeSideAngle;
    private float lastSlopeAngle;
    public float slopeCheckDistance = 0.1f;
    public LayerMask whatIsGround;
    public float maxSlopeAngle = 60.0f;
    private bool isOnSlope;
    public bool canWalkOnSlope;

    public bool IsOnSlope()
    {
        return isOnSlope;
    }
    public void GetColliderSize(BoxCollider2D c)
    {
        colliderSize = c.size;
        colliderOffset = c.offset;
        whatIsGround = LayerMask.GetMask("Environment");
    }
    public void GetColliderSize(PhysicsMaterial2D f, PhysicsMaterial2D n, CapsuleCollider2D c)
    {
    fullFriction = f;
    normFriction = n;
    colliderSize = c.size;
        colliderOffset = c.offset;
        whatIsGround = LayerMask.GetMask("Environment");
    }
    public bool SlopeCheck(Transform transf)
    {
        Vector2 checkPos = transf.position - new Vector3(0.0f, colliderSize.y/2.0f-colliderOffset.y);
        return (SlopeCheckVertical(checkPos));//||SlopeCheckHorizontal(transf.right, checkPos)
    }
    private void SlopeCheckHorizontal(Transform transf, Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transf.right, slopeCheckDistance, whatIsGround);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transf.right, slopeCheckDistance, whatIsGround);

        if (slopeHitFront)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);

        }
        else if (slopeHitBack)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }

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
