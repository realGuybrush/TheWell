using UnityEngine;

public class EnvironmentChecker
{
    private LayerMask landLayer;
    public bool landed = true;
    private readonly Transform transform;
    private float lookDir = 1.0f;
    private Vector2 direction;
    private Vector3 startPos;
    private Vector3 boxSize;

    public EnvironmentChecker(Transform newTransform, Vector3 StartPos, Vector3 BoxSize, Vector2 Direction, LayerMask LandLayer)
    {
        startPos = StartPos;
        boxSize = BoxSize;
        direction = Direction;
        landLayer = LandLayer;
        transform = newTransform;
    }
    public bool IsLanded()
    {
        return landed;
    }
    public void CheckForCollision()
    {
        AdjustLookDir();
        Visualize();
        float rot = GlobalFuncs.AroundZero(transform.eulerAngles.y) ? 1.0f:-1.0f;
        Vector2 startWithRotation = new Vector2(transform.position.x + startPos.x*rot*direction.x, transform.position.y + startPos.y);
        landed =  Physics2D.BoxCast(startWithRotation, boxSize, 0.0f, direction, 0.01f, landLayer);
    }

    private void AdjustLookDir()
    {
        lookDir = (GlobalFuncs.AroundZero(transform.transform.eulerAngles.y) ? 1.0f : -1.0f);
    }
    private void Visualize()
    {
        float rot = GlobalFuncs.AroundZero(transform.transform.eulerAngles.y) ? 1.0f : -1.0f;
        Vector3 startWithRotation = new Vector2(startPos.x * rot, startPos.y);
        Debug.DrawRay(transform.position + startWithRotation - new Vector3(lookDir * boxSize.x / 2, -boxSize.y/2), lookDir * Vector2.right*boxSize.x, Color.green);
        Debug.DrawRay(transform.position + startWithRotation - new Vector3(lookDir * boxSize.x / 2, -boxSize.y/2), Vector2.down*boxSize.y, Color.green);
        Debug.DrawRay(transform.position + startWithRotation + new Vector3(lookDir * boxSize.x / 2, -boxSize.y/2), lookDir * Vector2.left*boxSize.x, Color.green);
        Debug.DrawRay(transform.position + startWithRotation + new Vector3(lookDir * boxSize.x / 2, -boxSize.y/2), Vector2.up*boxSize.y, Color.green);
    }

    public void MoveYStart(float deltaY)
    {
        startPos = new Vector3(startPos.x, startPos.y + deltaY, startPos.z);
    }

    public float YStart
    {
        get => startPos.y;
        set { startPos = new Vector3(startPos.x, value, startPos.z); }
    }
}
