using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BasicMovement : MonoBehaviour
{
    public PhysicsMaterial2D fullFriction;
    public PhysicsMaterial2D normFriction;
    public PhysicsMaterial2D noFriction;
    public LayerMask landLayer;
    public LayerMask platformLayer;
    private LayerMask whatIsGround;
    private CapsuleCollider2D thisCollider;

    public EnvironmentChecker land;
    public EnvironmentChecker ledge;
    public EnvironmentChecker step;
    public EnvironmentChecker wall;

    private void DefineGroundLayer()
    {
        whatIsGround = landLayer + platformLayer;
    }
    public void ReactOnSlope()
    {
        if (BasicHandleSlope())
            AdjustSlopeFriction();
        else
            AdjustLandFriction();
    }

    public void AdjustSlopeFriction()
    {
        thisObject.sharedMaterial = fullFriction;
    }
    public void AdjustLandFriction()
    {
        thisObject.sharedMaterial = normFriction;
    }
    public void AdjustMidAirFriction()
    {
        thisObject.sharedMaterial = fullFriction;
    }

    public void ProcessEnvCheckersCollisions()
    {
        var touchingLand = land.IsLanded();
        var touchingTop = ledge.IsLanded();
        var touchingMid = wall.IsLanded();
        var touchingBot = step.IsLanded();
        if (!holding)
        {
            var pressingTowardsWall = movingDirection < 0 && !facingRight ||
                                      movingDirection > 0 && facingRight;
            if (touchingLand)
            {
                land.Land();
            }
            else
            {
                land.landed = false;
            }
            if (!land.landed && pressingTowardsWall)
            {
                if (!touchingTop && touchingMid)
                {
                    if (!holding)
                    {
                        if (canHold)
                        {
                            Hold();
                        }
                    }
                }
                else
                {
                    ledge.landed = false;
                }
                if (touchingTop && touchingMid && touchingBot)
                {
                    wall.Land();
                }
                else
                {
                    wall.landed = false;
                }
                if (!touchingTop && !touchingMid && touchingBot)
                {
                    step.Land();
                }
                else
                {
                    step.landed = false;
                }
            }
            else
            {
                ledge.landed = false;
                wall.landed = false;
                step.landed = false;
            }
        }
    }
}
