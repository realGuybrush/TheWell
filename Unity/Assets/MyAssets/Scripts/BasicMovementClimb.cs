using UnityEngine;

public partial class BasicMovement: MonoBehaviour
{
    private readonly float climbXChange = 0.0f;
    private readonly float climbYChange = 0.0f;
    public bool canClimb = true;
    private readonly bool canHold = true;
    public bool holding;
    public int holdingMaximumTime = 10000;
    private int landTimer;
    private Vector3 PosPreHold;
    public void BasicCheckClimb()
    {
        if (holding)
        {
            if (canClimb&&anim.a.GetBool("Grab"))
            {
                var newPositionX = thisObject.transform.position.x + climbXChange * -thisObject.transform.forward.z;
                var newPositionY = thisObject.transform.position.y + climbYChange;
                var newPosition = new Vector2(newPositionX, newPositionY);
                thisObject.transform.position = newPosition;
                anim.SetVar("Climb", true);
                Unhold();
            }
            else
            {
                anim.SetVar("Climb", false);
            }
        }
        else
        {
            anim.SetVar("Climb", false);
        }
    }

    public bool IsClimbing()
    {
        return anim.a.GetBool("Climb");
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
        SetKinematic(true);
        SetHangPosition();
        PosPreHold = thisObject.transform.position;
        thisObject.velocity = new Vector2(0.0f, 0.0f);
    }
    public void SetHangPosition()
    {
        while (!ledge.IsLanded() && wall.IsLanded())
        {
            thisObject.transform.position -= new Vector3(0.0f, 0.01f);
        }
        thisObject.transform.position += new Vector3(0.0f, 0.01f);
    }

    public void SetKinematic(bool val)
    {
        thisObject.isKinematic = val;
    }

    public void Unhold()
    {
        landTimer = 0;
        holding = false;
    }
}
