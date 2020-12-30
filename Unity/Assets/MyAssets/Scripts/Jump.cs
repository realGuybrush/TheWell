using UnityEngine;

public class BasicJump
{
    public float jumpSpeedX = 7.0f;
    public float jumpSpeedY = 5.0f;
    private Rigidbody2D thisObject;

    public void SetThisObject(Rigidbody2D newThisObject)
    {
        thisObject = newThisObject;
    }

    public bool CheckJump(bool canJump = true, float movingMultiplierX = 0.0f)
    {
        if (canJump)
        {
            Jump(movingMultiplierX);
            return true;
        }

        return false;
    }

    public void Jump(float localMovingDirection, float jumpSpeedX = 0.0f)
    {
        var jumpVector = new Vector2(jumpSpeedX * localMovingDirection, jumpSpeedY);
        thisObject.velocity = jumpVector;
    }
}