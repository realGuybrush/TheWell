using UnityEngine;

public partial class PlayerControls : BasicMovement
{
    public override void InitValues()
    {
        base.InitValues();
        InitInventory();
    }
    private void Update()
    {
        UpdatePlacingObject();
        CheckNumbersInput();
        if (!IsClimbing())
        {
            MovePicker();
            CheckRotateInput();
            ProcessEnvCheckersCollisions();
            BasicHandleMidAir();
            PlayerCheckMove();
            CheckClimbLadder();
            ReactOnSlope();
            if (!CheckFallPlatformInput()&&!fallingFromPlatform)
                CheckJumpInput();
            UpdateHold();
            CheckFlip();
            BasicHandleHold();
            CheckClimbInput();
            CheckAtkInput();
            if(!CheckPickUpInput())
                CheckActionInput();
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
        if (movingDirection != 0)
        {
            if (!BasicHandleHold())
            {
                BasicHandleMove();
            }
        }
        else
        {
            if (!land.landed)
            {
                SlowDown();
            }
            else
            {
            //    var stopInstantly = 1.0f;
            //    move.SlowDown(stopInstantly);
            }
            if (onLadder)
            {
                SlowDown();
            }
        }

        if (!GlobalFuncs.AroundZero(thisObject.velocity.x) || !GlobalFuncs.AroundZero(thisObject.velocity.y))
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
        if (!BasicHandleHold())
        {
            BasicHandleFlip(movingDirection);
        }
    }
}
