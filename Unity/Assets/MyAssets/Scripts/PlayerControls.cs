using UnityEngine;

public partial class PlayerControls : BasicMovement
{
    private void Start()
    {
        thisObject = gameObject.GetComponent<Rigidbody2D>();
        thisHealth = thisObject.GetComponent<Health>();
        anim.a = GetComponent<Animator>();
        move.SetThisObject(thisObject);
        flip.SetThisObject(thisObject);
        jump.SetThisObject(thisObject);
        land = new BasicLand(thisObject, jump, climb);
        ledge = new BasicLand(thisObject, jump, climb, 100000, false, true);
        wall = new BasicLand(thisObject, jump, climb, 0, false);
        step = new BasicLand(thisObject, jump, climb, 0, false);
        climb.SetThisObject(thisObject);
        InitInventory();
    }
    private void Update()
    {
        if (!IsClimbing())
        {
            CheckLand();
            BasicCheckMidAir();
            PlayerCheckMove();
            CheckJumpInput();
            wall.UpdateHold();
            ledge.UpdateHold();
            CheckFlip();
            CheckClimbInput();
            CheckAtkInput();
            CheckActionInput();
            BasicCheckHold();
        }
        BasicCheckHealth();
        CheckEsc();
    }

    private void PlayerCheckMove()
    {
        CheckMovementDirection();
        CheckSpeedUp();
        CheckRunInput();
        CheckCrawlInput();
        if (move.movingDirection != 0)
        {
            if (!BasicCheckHold())
            {
                move.Move();
            }
        }
        else
        {
            if (!land.landed)
            {
                move.SlowDown();
            }
            else
            {
                var stopInstantly = 1.0f;
                move.SlowDown(stopInstantly);
            }
        }

        if (thisObject.velocity.x != 0 || thisObject.velocity.y != 0)
        {
            anim.SetVar("Moving", true);
        }
        else
        {
            anim.SetVar("Moving", false);
        }
    }

    private void CheckFlip()
    {
        if (!BasicCheckHold())
        {
            flip.CheckFlip(move.movingDirection);
        }
    }

    public void Res()
    {
        landChecker.Res();
        wallChecker.Res();
        stepChecker.Res();
        ledgeChecker.Res();
    }
}
