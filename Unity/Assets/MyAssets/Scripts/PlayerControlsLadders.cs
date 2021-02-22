using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerControls : BasicMovement
{
    bool onLadder = false;
    public void GetOnLadder()
    {
        onLadder = true;
        ClimbLadder();
    }
    public void GetOffLadder()
    {
        SetKinematic(false);
        onLadder = false;
    }

    public void ClimbLadder()
    {
        if (onLadder)
        {
            SetKinematic(false);
            Vector3 climbVelocity = new Vector3(thisObject.velocity.x/2, Input.GetAxis("Vertical"), 0.0f);
            thisObject.velocity = climbVelocity;
        }
    }

    public void UndoFalling()
    {
        thisObject.velocity = new Vector3(thisObject.velocity.x, 0.0f, 0.0f);
        SetKinematic(true);
    }
}
