using UnityEngine;

public class BasicLand
{
    public bool canClimb;
    private readonly bool canHold;
    public bool canJump;
    public BasicClimb climb;
    public bool holding;
    public int holdingMaximumTime;
    public BasicJump jump;
    public bool landed = true;
    private int landTimer;
    private readonly Rigidbody2D thisObject;
    private Vector3 PosPreHold;

    public BasicLand(Rigidbody2D newThisObject, BasicJump newJump, BasicClimb newClimb, int newHoldingMaximumTime = 0,
        bool newCanJump = true, bool newCanClimb = false)
    {
        thisObject = newThisObject;
        jump = newJump;
        climb = newClimb;
        holdingMaximumTime = newHoldingMaximumTime;
        landTimer = holdingMaximumTime;
        canHold = holdingMaximumTime != 0;
        canJump = newCanJump;
        canClimb = newCanClimb;
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
}
