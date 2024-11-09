using System;
using UnityEngine;

public class EnvInteractor : MonoBehaviour
{
    [SerializeField]
    private LayerMask landLayer;

    [SerializeField]
    private LayerMask platformLayer;

    [SerializeField]
    private LayerMask whatIsGround;

    private Vector3 ledgeStart, ledgeSize, wallStart, wallSize, stepStart, stepSize, landStart, landSize, upperStart, upperSize;
    public float magicNumber1 = 10f, magicNumber2 = 0.2f, magicNumber3 = 1.3f;
    protected Vector3 rotateAroundY = new Vector3(0.0f, 180.0f, 0.0f);

    [SerializeField]
    private bool facingRight = true;

    [SerializeField]
    protected Rigidbody2D thisObject;

    [SerializeField]
    protected Collider2D thisCollider;

    private bool isAirborne;

    public event Action OnLanding = delegate { };

    private EnvironmentChecker land, ledge, step, wall, upperPassage;

    private void Awake() {
        Awaking();
    }

    protected virtual void Awaking()
    {
        SetEnvironmentCheckers(thisCollider.bounds);
    }

    private void Start()
    {
        Starting();
    }

    protected virtual void Starting() { }

    private void Update()
    {
        Updating();
    }

    protected virtual void Updating()
    {
        ProcessEnvCheckersCollisions();
    }

    private void SetEnvironmentCheckers(Bounds bounds) {
        Vector2 size = bounds.size;
        Vector2 center = bounds.center;
        Vector2 extents = bounds.extents;
        SetLandChecker(size, center, extents);
        SetStepChecker(size, center, extents);
        SetWallChecker(size, center, extents);
        SetLedgeChecker(size, center, extents);
        SetUpperPassageChecker(size, center, extents);
    }

    //todo: get rid of magic numbers
    private void SetLandChecker(Vector2 size, Vector2 center, Vector2 extents)
    {
        landSize = new Vector2(size.x * 0.9f, size.y * 0.1f);
        landStart = new Vector2(center.x, center.y - extents.y);
        land = new EnvironmentChecker(thisObject.transform, landStart, landSize, Vector2.down, whatIsGround);
    }
    private void SetStepChecker(Vector2 size, Vector2 center, Vector2 extents)
    {
        stepSize = new Vector2(size.y / 10.0f, size.y*0.2f );
        stepStart = new Vector2(center.x + extents.x + stepSize.x / 2, center.y - extents.y * 0.8f);
        step = new EnvironmentChecker(thisObject.transform, stepStart, stepSize, facingRight? Vector2.right: Vector2.left, landLayer);
    }
    private void SetWallChecker(Vector2 size, Vector2 center, Vector2 extents)
    {
        wallSize = new Vector2(size.y / 10.0f, size.y*0.4f);
        wallStart = new Vector2(center.x + extents.x + wallSize.x / 2, center.y + extents.y * 0.1f);
        wall = new EnvironmentChecker(thisObject.transform, wallStart, wallSize, facingRight ? Vector2.right : Vector2.left, landLayer);
    }
    private void SetLedgeChecker(Vector2 size, Vector2 center, Vector2 extents)
    {
        ledgeSize = new Vector2(size.y / 10.0f, size.y*0.4f);
        ledgeStart = new Vector2(center.x + extents.x + ledgeSize.x / 2, center.y + extents.y * 0.9f);
        ledge = new EnvironmentChecker(thisObject.transform, ledgeStart, ledgeSize, facingRight ? Vector2.right : Vector2.left, landLayer);
    }
    private void SetUpperPassageChecker(Vector2 size, Vector2 center, Vector2 extents)
    {
        upperSize = new Vector2(size.y / 5.0f, size.y*0.2f);
        upperStart = new Vector2(center.x + extents.x + upperSize.x / 2, center.y + extents.y * 0.3f);
        upperPassage = new EnvironmentChecker(thisObject.transform, upperStart, upperSize, facingRight ? Vector2.right : Vector2.left, landLayer);
    }

    protected void ProcessEnvCheckersCollisions()
    {
        land.CheckForCollision();
        ledge.CheckForCollision();
        wall.CheckForCollision();
        step.CheckForCollision();
        upperPassage.CheckForCollision();
        if (land.landed && isAirborne)
        {
            isAirborne = false;
            thisObject.gravityScale = 1f;
            OnLanding?.Invoke();
        }
    }

    protected void SetKinematic(bool val)
    {
        thisObject.isKinematic = val;
    }

    protected bool IsItPlatform(Collision2D collider)
    {
        return (platformLayer & (1 << collider.gameObject.layer)) != 0 ;
    }

    protected bool IsAgainstLedge()
    {
        return !ledge.IsLanded() && wall.IsLanded();
    }

    protected bool IsOnLedge()
    {
        return ledge.IsLanded() && wall.IsLanded();
    }

    protected bool CanClimbInStanding()
    {
        upperPassage.YStart += thisCollider.bounds.size.y;
        upperPassage.CheckForCollision();
        bool canQue = !upperPassage.landed;
        upperPassage.YStart -= thisCollider.bounds.size.y;
        return canQue;
    }

    protected bool CanClimbInCrawling()
    {
        float deltaY = thisCollider.bounds.size.y / 2;
        upperPassage.YStart += deltaY;
        upperPassage.CheckForCollision();
        bool canQue = !upperPassage.landed;
        upperPassage.YStart -= deltaY;
        return canQue;
    }

    protected bool IsAgainstStep()
    {
        return !ledge.IsLanded() && !wall.IsLanded() && step.IsLanded();
    }

    protected bool IsLanded()
    {
        return land.IsLanded();
    }

    protected void LiftOff()
    {
        isAirborne = true;
        thisObject.gravityScale = 0;
    }

    protected void HandleFlip(float movingDirection)
    {
        if (!DirectionMatchesFacing(movingDirection))
        {
            thisObject.transform.Rotate(rotateAroundY);
            facingRight = !facingRight;
        }
    }

    protected bool DirectionMatchesFacing(float movingDirection)
    {
        return !facingRight && movingDirection <= 0 ||
               facingRight && movingDirection >= 0;
    }

    protected Vector3 CalculateHangingPosY()
    {
        float defaultY = ledge.YStart, deltaY = 0f;
        while (!ledge.IsLanded() && deltaY > -thisCollider.bounds.size.y)
        {
            ledge.MoveYStart(-0.01f);
            deltaY -= 0.01f;
            ledge.CheckForCollision();
        }
        ledge.YStart = defaultY;
        return new Vector3(0f, deltaY, 0f);
    }
    protected bool FacingRight => facingRight;

    protected float FacingRightFloat => facingRight?1f:-1f;

    protected bool IsAirborne => isAirborne;

    protected LayerMask WhatIsGround  => whatIsGround;
}
