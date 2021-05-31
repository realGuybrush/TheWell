using UnityEngine;

public partial class BasicMovement:MonoBehaviour
{
    public float movementMultiplier = 0.0f;
    public float movingDirection = 0.0f;
    public float walkSpeed = 3.0f;
    public float baseWalkSpeed = 3.0f;

    public void BasicHandleMove()
    {
        var bonusMultiplier = 1.0f;
        if (running)
        {
            bonusMultiplier *= runMultiplier;
        }

        if (crawling)
        {
            bonusMultiplier *= crawlingMultiplier;
        }

        var walkVector = new Vector2(bonusMultiplier * walkSpeed * movementMultiplier, thisObject.velocity.y);
        thisObject.velocity = walkVector;
    }

    public void SlowDown(float slowingAmount = 0.01f, float dontSlowY = 0.0f)
    {
        if (thisObject.velocity.x != 0)
        {
            var counterDirection = -Mathf.Sign(thisObject.velocity.x);
            Vector2 walkVector;
            if (GlobalFuncs.Dif(thisObject.velocity.x, walkSpeed * slowingAmount) < Mathf.Abs(walkSpeed * slowingAmount))
            {
                walkVector = new Vector2(-thisObject.velocity.x, dontSlowY);
            }
            else
            {
                walkVector = new Vector2(walkSpeed * slowingAmount * counterDirection, dontSlowY);
            }

            thisObject.velocity += walkVector;
        }
    }
}
