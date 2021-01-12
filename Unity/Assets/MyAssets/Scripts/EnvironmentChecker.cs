using UnityEngine;

public class EnvironmentChecker
{
    private LayerMask landLayer;
    public bool canClimb;
    private readonly bool canHold;
    public bool canJump;
    public bool holding;
    public int holdingMaximumTime;
    public bool landed = true;
    private int landTimer;
    private readonly Rigidbody2D thisObject;
    private Vector3 PosPreHold;

    private float lookDir = 1.0f;
    private Vector2 direction;
    private float distance;
    private Vector3 startPos;
    private Vector3 boxSize;

    public EnvironmentChecker(Rigidbody2D newThisObject, Vector3 StartPos, Vector3 BoxSize, float Distance, Vector2 Direction, LayerMask LandLayer,
        int newHoldingMaximumTime = 0, bool newCanJump = true, bool newCanClimb = false)
    {
        SetCheckerValues(StartPos, BoxSize, Distance, Direction, LandLayer);
        thisObject = newThisObject;
        holdingMaximumTime = newHoldingMaximumTime;
        landTimer = holdingMaximumTime;
        canHold = holdingMaximumTime != 0;
        canJump = newCanJump;
        canClimb = newCanClimb;
    }

    public void SetCheckerValues(Vector3 StartPos, Vector3 BoxSize, float Distance, Vector2 Direction, LayerMask LandLayer)
    {
        startPos = StartPos;
        boxSize = BoxSize;
        distance = Distance;
        direction = Direction;
        landLayer = LandLayer;
    }
    public bool IsLanded()
    {
        AdjustLookDir();
        Visualize();
        return Physics2D.BoxCast(thisObject.transform.position+startPos, boxSize, 0.0f, direction, distance, landLayer);
    }
    public void Land()
    {
        landed = true;
        holding = false;
    }

    public void CheckThisLand(bool landChecker)
    {
        if (landChecker)
        {
            if (!holding)
            {
                Land();
                if (canHold)
                {
                    Hold();
                }
            }
        }
        else
        {
            landed = false;
        }
    }

    public bool UpdateHold()
    {
        if (holding)
        {
            if (landTimer == 0)
            {
                Unhold();
                return false;
            }
            landTimer--;
            thisObject.transform.position = PosPreHold;
        }
        else
        {
            Unhold();
            return false;
        }

        return true;
    }

    public void Hold()
    {
        landTimer = holdingMaximumTime;
        holding = true;
        PosPreHold = thisObject.transform.position;
        thisObject.velocity = new Vector2(0.0f, 0.0f);
    }

    public void Unhold()
    {
        landTimer = 0;
        holding = false;
    }

    public void AdjustLookDir()
    {
        lookDir = (GlobalFuncs.AroundZero(thisObject.transform.eulerAngles.y) ? 1.0f : -1.0f);
    }
    public void Visualize()
    {
        Debug.DrawRay(thisObject.transform.position + startPos, new Vector2(Mathf.Abs(direction.x)*lookDir, direction.y)*distance);
        Vector2 distanceMultiplyer = direction * distance;
        Vector3 secondPointOffset = new Vector3(lookDir*distance*direction.x, 0.0f);
        Debug.DrawRay(thisObject.transform.position + startPos - new Vector3(lookDir * boxSize.x / 2, -boxSize.y/2), lookDir * Vector2.right*(boxSize.x+ distanceMultiplyer.x), Color.green);
        Debug.DrawRay(thisObject.transform.position + startPos - new Vector3(lookDir * boxSize.x / 2, -boxSize.y/2), Vector2.down*(boxSize.y+ distanceMultiplyer.y), Color.green);
        Debug.DrawRay(thisObject.transform.position + startPos + secondPointOffset + new Vector3(lookDir * boxSize.x / 2, -boxSize.y/2), lookDir * Vector2.left*(boxSize.x+ distanceMultiplyer.x), Color.green);
        Debug.DrawRay(thisObject.transform.position + startPos + secondPointOffset + new Vector3(lookDir * boxSize.x / 2, -boxSize.y/2), Vector2.up*(boxSize.y+ distanceMultiplyer.y), Color.green);
    }
}
