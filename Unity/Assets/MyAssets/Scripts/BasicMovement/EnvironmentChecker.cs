using UnityEngine;

public class EnvironmentChecker
{
    private LayerMask landLayer;
    public bool landed = true;
    public bool canJump;
    private readonly Rigidbody2D thisObject;

    private float lookDir = 1.0f;
    private Vector2 direction;
    private float distance;
    private Vector3 startPos;
    private Vector3 boxSize;

    public EnvironmentChecker(Rigidbody2D newThisObject, Vector3 StartPos, Vector3 BoxSize, float Distance, Vector2 Direction, LayerMask LandLayer, bool newCanJump = true)
    {
        SetCheckerValues(StartPos, BoxSize, Distance, Direction, LandLayer);
        thisObject = newThisObject;
        canJump = newCanJump;
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
        var rot = GlobalFuncs.AroundZero(thisObject.transform.eulerAngles.y) ? 1.0f:-1.0f;
        Vector2 startWithRotation = new Vector2(thisObject.transform.position.x + startPos.x*rot*direction.x, thisObject.transform.position.y + startPos.y);
        return Physics2D.BoxCast(startWithRotation, boxSize, 0.0f, direction, 0.01f, landLayer);
    }
    public void Land()
    {
        landed = true;
    }

    public void AdjustLookDir()
    {
        lookDir = (GlobalFuncs.AroundZero(thisObject.transform.eulerAngles.y) ? 1.0f : -1.0f);
    }
    public void Visualize()
    {
        var rot = GlobalFuncs.AroundZero(thisObject.transform.eulerAngles.y) ? 1.0f : -1.0f;
        Vector3 startWithRotation = new Vector2(startPos.x * rot, startPos.y);
        Debug.DrawRay(thisObject.transform.position + startWithRotation - new Vector3(lookDir * boxSize.x / 2, -boxSize.y/2), lookDir * Vector2.right*boxSize.x, Color.green);
        Debug.DrawRay(thisObject.transform.position + startWithRotation - new Vector3(lookDir * boxSize.x / 2, -boxSize.y/2), Vector2.down*boxSize.y, Color.green);
        Debug.DrawRay(thisObject.transform.position + startWithRotation + new Vector3(lookDir * boxSize.x / 2, -boxSize.y/2), lookDir * Vector2.left*boxSize.x, Color.green);
        Debug.DrawRay(thisObject.transform.position + startWithRotation + new Vector3(lookDir * boxSize.x / 2, -boxSize.y/2), Vector2.up*boxSize.y, Color.green);
    }
}
