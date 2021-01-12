using UnityEngine;

public partial class BasicMovement: MonoBehaviour
{
    private readonly float climbXChange = 0.0f;
    private readonly float climbYChange = 0.0f;
    public void BasicCheckClimb()
    {
        var holding = land.holding || ledge.holding || wall.holding || step.holding;
        var canClimb = land.holding && land.canClimb || ledge.holding && ledge.canClimb ||
                       wall.holding && wall.canClimb || step.holding && step.canClimb;
        if (holding)
        {
            if (canClimb&&anim.a.GetBool("Grab"))
            {
                var newPositionX = thisObject.transform.position.x + climbXChange * -thisObject.transform.forward.z;
                var newPositionY = thisObject.transform.position.y + climbYChange;
                var newPosition = new Vector2(newPositionX, newPositionY);
                thisObject.transform.position = newPosition;
                anim.SetVar("Climb", true);
                ReleaseHolds();
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
    public void ReleaseHolds()
    {
        land.Unhold();
        ledge.Unhold();
        wall.Unhold();
        step.Unhold();
    }
}
