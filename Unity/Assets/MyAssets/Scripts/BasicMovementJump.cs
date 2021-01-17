using UnityEngine;

public partial class BasicMovement:MonoBehaviour
{
    public float jumpSpeedX = 7.0f;
    public float jumpSpeedY = 5.0f;
    public void BasicJump(float movingMultiplierX = 0.0f)
    {
        var midAir = IsMidAir();
        var canJumpFromCurrentLand = land.landed && land.canJump || ledge.landed && ledge.canJump ||
                                     wall.landed && wall.canJump || step.landed && step.canJump;
        if (!midAir && canJumpFromCurrentLand && !crawling)
        {
            Jump(movingMultiplierX);
            anim.SetVar("Jump", true);
            Unhold();
        }
    }

    public void Jump(float localMovingDirection, float jumpSpeedX = 0.0f)
    {
        var jumpVector = new Vector2(jumpSpeedX * localMovingDirection, jumpSpeedY);
        thisObject.velocity = jumpVector;
    }
}
