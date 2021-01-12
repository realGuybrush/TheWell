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
    public bool BasicCheckHold()
    {
        var holding = land.holding || ledge.holding || wall.holding || step.holding;
        if (holding)
        {
            AdjustSlopeFriction();
            anim.SetVar("Grab", holding);
        }
        else
        {
            AdjustMidAirFriction();
            anim.SetVar("Grab", holding);
        }
        return holding;
    }
    public void ReactOnSlope()
    {
        if (BasicCheckSlope())
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

    public void CheckLand()
    {
        var touchingLand = land.IsLanded();
        var touchingTop = ledge.IsLanded();
        var touchingMid = wall.IsLanded();
        var touchingBot = step.IsLanded();
        var onHold = wall.holding || ledge.holding;
        if (!onHold)
        {
            var pressingTowardsWall = movingDirection < 0 && !facingRight ||
                                      movingDirection > 0 && facingRight;
            land.CheckThisLand(touchingLand);
            if (!land.landed && pressingTowardsWall)
            {
                ledge.CheckThisLand(!touchingTop && touchingMid);
                wall.CheckThisLand(touchingTop && touchingMid && touchingBot);
                step.CheckThisLand(!touchingTop && !touchingMid && touchingBot);
            }
            else
            {
                ledge.landed = false;
                wall.landed = false;
                step.landed = false;
            }
        }
        else
        {
            ledge.CheckThisLand(!touchingTop && touchingMid);
            wall.CheckThisLand(touchingTop && touchingMid && touchingBot);
            step.CheckThisLand(!touchingTop && !touchingMid && touchingBot);
        }
    }
}
