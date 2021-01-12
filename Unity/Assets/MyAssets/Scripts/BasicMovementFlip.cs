using UnityEngine;

public partial class BasicMovement:MonoBehaviour
{
    public bool facingRight = true;

    public void BasicCheckFlip(float movingDirection)
    {
        if (facingRight && movingDirection < 0)
        {
            Flip();
        }
        else if (!facingRight && movingDirection > 0)
        {
            Flip();
        }
    }
    public void BasicCheckFlip()
    {
        if (facingRight && (FacingDirection() > 0))
        {
            Flip();
        }
        else if (!facingRight && (FacingDirection() < 0))
        {
            Flip();
        }
    }

    public void Flip()
    {
        var rotateAroundY = new Vector3(0.0f, 180.0f, 0.0f);
        thisObject.transform.Rotate(rotateAroundY);
        facingRight = !facingRight;
    }

    public float FacingDirection()
    {
        if (thisObject.name == "Player")
            return (GlobalFuncs.AroundZero(thisObject.transform.eulerAngles.y) ? 1.0f : -1.0f);
        else
            return (GlobalFuncs.AroundZero(thisObject.transform.eulerAngles.y) ? -1.0f : 1.0f);
    }
}
